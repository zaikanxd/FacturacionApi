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

namespace FacturacionApi.Controllers
{
    [RoutePrefix("facturacion")]
    public class FacturacionController : ApiController
    {
        private const string UrlSunat = "https://e-beta.sunat.gob.pe/ol-ti-itcpfegem-beta/billService";

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

                string pathCertificado = AppDomain.CurrentDomain.BaseDirectory + $"/Certificados/{documento.Emisor.NroDocumento}/{documento.Emisor.NroDocumento}.pfx";

                if (!File.Exists(pathCertificado))
                {
                    throw new Exception("La empresa no cuenta con certificado");
                }

                var firmadoRequest = new FirmadoRequest
                {
                    TramaXmlSinFirma = documentoResponse.TramaXmlSinFirma,
                    CertificadoDigital = Convert.ToBase64String(File.ReadAllBytes(pathCertificado)),
                    PasswordCertificado = string.Empty,
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
                        var imagen = Image.FromStream(mem);
                        string saveQRPath = AppDomain.CurrentDomain.BaseDirectory + $"/ArchivosGenerados/QRs/{documento.IdDocumento}.png";
                        imagen.Save(saveQRPath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }

                string saveXMLPath = AppDomain.CurrentDomain.BaseDirectory + $"/ArchivosGenerados/FacturaXML/{documento.IdDocumento}.xml";
                File.WriteAllBytes(saveXMLPath, Convert.FromBase64String(firmadoResponse.TramaXmlFirmado));

                PDF.GenerarPDF(documento, Convert.FromBase64String(firmadoResponse.CodigoQr));

                var documentoRequest = new EnviarDocumentoRequest
                {
                    Ruc = documento.Emisor.NroDocumento,
                    UsuarioSol = "MODDATOS",
                    ClaveSol = "MODDATOS",
                    EndPointUrl = UrlSunat,
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

                    string saveZIPPath = AppDomain.CurrentDomain.BaseDirectory + $"/ArchivosGenerados/TramaZipCdr/{documento.IdDocumento}.zip";

                    File.WriteAllBytes(saveZIPPath, Convert.FromBase64String(enviarDocumentoResponse.TramaZipCdr));
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
    }
}