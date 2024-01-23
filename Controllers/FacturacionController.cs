using Dto.Intercambio;
using Dto.Modelos;
using Firmado;
using Servicio;
using SOAP;
using System.Threading.Tasks;
using System.Web.Http;
using XML;
using System;
using System.Drawing;
using System.IO;
using FacturacionApi.Utils;
using Comun;

namespace FacturacionApi.Controllers
{
    [RoutePrefix("facturacion")]
    public class FacturacionController : ApiController
    {
        //private const string UrlSunatProd = "https://e-factura.sunat.gob.pe/ol-ti-itcpfegem/billService";
        private const string UrlSunatPrueba = "https://e-beta.sunat.gob.pe/ol-ti-itcpfegem-beta/billService";

        [HttpPost, Route("facturar")]
        public async Task<EnviarDocumentoResponse> facturar([FromBody] DocumentoElectronico documento)
        {
            // 1: GENERAR XML
            IDocumentoXml _documentoXml = new FacturaXml();
            ISerializador _serializador = new Serializador();
            var documentoResponse = new DocumentoResponse();

            // 2: FIRMAR XML
            ICertificador _certificador = new Certificador();
            var firmadoResponse = new FirmadoResponse();

            // 3: ENVIAR DOCUMENTO
            IServicioSunatDocumentos _servicioSunatDocumentos = new ServicioSunatDocumentos();
            var enviarDocumentoResponse = new EnviarDocumentoResponse();

            try
            {
                // 1: GENERAR XML

                var invoice = _documentoXml.Generar(documento);
                documentoResponse.TramaXmlSinFirma = await _serializador.GenerarXml(invoice);
                var serieCorrelativo = documento.IdDocumento.Split('-');
                documentoResponse.ValoresParaQr =
                    $"{documento.Emisor.NroDocumento}|{documento.TipoDocumento}|{serieCorrelativo[0]}|{serieCorrelativo[1]}|{documento.TotalIgv:N2}|{documento.TotalVenta:N2}|{Convert.ToDateTime(documento.FechaEmision):yyyy-MM-dd}|{documento.Receptor.TipoDocumento}|{documento.Receptor.NroDocumento}|";

                documentoResponse.Exito = true;
                // --

                // 2: FIRMAR XML

                string pathCertificado = AppSettings.pathCertificados + $"{documento.Emisor.NroDocumento}.pfx";

                if (!File.Exists(AppSettings.pathFile  + pathCertificado))
                {
                    throw new Exception("La empresa no cuenta con certificado");
                }

                Credencial credencial = Array.Find(CredencialEmpresa.credenciales, e => e.ruc == documento.Emisor.NroDocumento);

                if (credencial == null)
                {
                    throw new Exception("La empresa no cuenta con las credenciales SOL");
                }

                var firmadoRequest = new FirmadoRequest
                {
                    TramaXmlSinFirma = documentoResponse.TramaXmlSinFirma,
                    CertificadoDigital = Convert.ToBase64String(File.ReadAllBytes(AppSettings.pathFile + pathCertificado)),
                    PasswordCertificado = credencial.passwordCertificado,
                    ValoresQr = documentoResponse.ValoresParaQr
                };

                firmadoResponse = await _certificador.FirmarXml(firmadoRequest);
                firmadoResponse.Exito = true;
                if (!string.IsNullOrEmpty(firmadoRequest.ValoresQr))
                    firmadoResponse.CodigoQr = QrHelper.GenerarImagenQr($"{firmadoRequest.ValoresQr}{firmadoResponse.ResumenFirma}");

                if (!string.IsNullOrEmpty(firmadoResponse.CodigoQr))
                {
                    using (var mem = new MemoryStream(Convert.FromBase64String(firmadoResponse.CodigoQr)))
                    {
                        string qrPath = AppSettings.pathCE + $"{documento.Emisor.NroDocumento}\\QR\\";
                        if (!Directory.Exists(AppSettings.pathFile + qrPath))
                        {
                            Directory.CreateDirectory(AppSettings.pathFile + qrPath);
                        }
                        var imagen = Image.FromStream(mem);
                        string saveQRPath = qrPath + $"{documento.IdDocumento}.png";
                        imagen.Save(AppSettings.pathFile + saveQRPath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }

                string xmlPath = AppSettings.pathCE + $"{documento.Emisor.NroDocumento}\\FacturaXML\\";
                if (!Directory.Exists(AppSettings.pathFile + xmlPath))
                {
                    Directory.CreateDirectory(AppSettings.pathFile + xmlPath);
                }

                string saveXMLPath = xmlPath + $"{documento.IdDocumento}.xml";
                File.WriteAllBytes(AppSettings.pathFile + saveXMLPath, Convert.FromBase64String(firmadoResponse.TramaXmlFirmado));

                string logoPath = AppSettings.pathCompanyLogo + $"{documento.Emisor.NroDocumento}.png";

                if (File.Exists(AppSettings.pathFile + logoPath))
                {
                    documento.Logo = String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(File.ReadAllBytes(AppSettings.pathFile + logoPath)));
                }

                documento.QRFirmado = String.Format("data:image/gif;base64,{0}", firmadoResponse.CodigoQr);

                string pdfPath = PDF.ObtenerRutaPDFGenerado(documento);

                var documentoRequest = new EnviarDocumentoRequest
                {
                    Ruc = documento.Emisor.NroDocumento,
                    UsuarioSol = credencial.usuarioSol,
                    ClaveSol = credencial.claveSol,
                    EndPointUrl = UrlSunatPrueba,
                    IdDocumento = documento.IdDocumento,
                    TipoDocumento = documento.TipoDocumento,
                    TramaXmlFirmado = firmadoResponse.TramaXmlFirmado
                };

                // --

                // 3: ENVIAR DOCUMENTO

                var nombreArchivo = $"{documentoRequest.Ruc}-{documentoRequest.TipoDocumento}-{documentoRequest.IdDocumento}";
                var tramaZip = await _serializador.GenerarZip(documentoRequest.TramaXmlFirmado, nombreArchivo);

                _servicioSunatDocumentos.Inicializar(new ParametrosConexion
                {
                    Ruc = documentoRequest.Ruc,
                    UserName = documentoRequest.UsuarioSol,
                    Password = documentoRequest.ClaveSol,
                    EndPointUrl = documentoRequest.EndPointUrl
                });

                var resultado = _servicioSunatDocumentos.EnviarDocumento(new DocumentoSunat
                {
                    TramaXml = tramaZip,
                    NombreArchivo = $"{nombreArchivo}.zip"
                });

                if (!resultado.Exito)
                {
                    enviarDocumentoResponse.Exito = false;
                    enviarDocumentoResponse.MensajeError = resultado.MensajeError;
                }
                else
                {
                    enviarDocumentoResponse = await _serializador.GenerarDocumentoRespuesta(resultado.ConstanciaDeRecepcion);
                    // Quitamos la R y la extensión devueltas por el Servicio.
                    enviarDocumentoResponse.NombreArchivo = nombreArchivo;

                    string zipPath = AppSettings.pathCE + $"{documento.Emisor.NroDocumento}\\TramaZipCdr\\";
                    if (!Directory.Exists(AppSettings.pathFile + zipPath))
                    {
                        Directory.CreateDirectory(AppSettings.pathFile + zipPath);
                    }

                    string saveZIPPath = zipPath + $"{documento.IdDocumento}.zip";

                    File.WriteAllBytes(AppSettings.pathFile + saveZIPPath, Convert.FromBase64String(enviarDocumentoResponse.TramaZipCdr));

                    enviarDocumentoResponse.qrCode = documentoResponse.ValoresParaQr;
                    enviarDocumentoResponse.xmlPath = saveXMLPath;
                    enviarDocumentoResponse.pdfPath = pdfPath;
                }
            }
            catch (Exception ex)
            {
                enviarDocumentoResponse.MensajeError = ex.Message;
                enviarDocumentoResponse.Pila = ex.StackTrace;
                enviarDocumentoResponse.Exito = false;
            }

            return enviarDocumentoResponse;
        }

        [HttpPost, Route("comprobanteSinValorFiscal")]
        public ComprobanteSinValorFiscalResponse comprobanteSinValorFiscal([FromBody] DocumentoElectronico documento)
        {
            var comprobanteSinValorFiscalResponse = new ComprobanteSinValorFiscalResponse();

            try
            {
                documento.MontoEnLetras = Conversion.Enletras(documento.TotalVenta);

                string folderPath = (documento.EsTicketConsumo) ? AppSettings.pathTCSVF : AppSettings.pathCESVF;

                string pdfPath = PDF.ObtenerRutaPDFGeneradoSinValorFiscal(documento, folderPath);

                comprobanteSinValorFiscalResponse.Exito = true;
                comprobanteSinValorFiscalResponse.pdfPath = pdfPath;
            }
            catch (Exception ex)
            {
                comprobanteSinValorFiscalResponse.MensajeError = ex.Message;
                comprobanteSinValorFiscalResponse.Pila = ex.StackTrace;
                comprobanteSinValorFiscalResponse.Exito = false;
            }

            return comprobanteSinValorFiscalResponse;
        }
    }
}