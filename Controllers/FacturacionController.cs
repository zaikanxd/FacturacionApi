﻿using Dto.Intercambio;
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
using BusinessLogic;
using BusinessEntity.Dtos;

namespace FacturacionApi.Controllers
{
    [RoutePrefix("facturacion")]
    public class FacturacionController : ApiController
    {
        private ElectronicReceiptBL _ElectronicReceiptBL;
        private ElectronicReceiptBL oElectronicReceiptBL
        {
            get { return (_ElectronicReceiptBL == null ? _ElectronicReceiptBL = new ElectronicReceiptBL() : _ElectronicReceiptBL); }
        }

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
                string projectPath = Array.Find(Project.projects, e => e == documento.Project);

                if (projectPath == null)
                {
                    throw new Exception("No existe una carpeta para el proyecto");
                } 
                else 
                {
                    projectPath = AppSettings.projectsPath + $"{projectPath}\\";
                }

                // 1: GENERAR XML

                var invoice = _documentoXml.Generar(documento);
                documentoResponse.TramaXmlSinFirma = await _serializador.GenerarXml(invoice);
                var serieCorrelativo = documento.IdDocumento.Split('-');
                documentoResponse.ValoresParaQr =
                    $"{documento.Emisor.NroDocumento}|{documento.TipoDocumento}|{serieCorrelativo[0]}|{serieCorrelativo[1]}|{documento.TotalIgv:N2}|{documento.TotalVenta:N2}|{Convert.ToDateTime(documento.FechaEmision):yyyy-MM-dd}|{documento.Receptor.TipoDocumento}|{documento.Receptor.NroDocumento}|";

                documentoResponse.Exito = true;
                // --

                // 2: FIRMAR XML

                string certificadoPath = AppSettings.certificadosPath + $"{documento.Emisor.NroDocumento}.pfx";

                if (!File.Exists(AppSettings.filePath + certificadoPath))
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
                    CertificadoDigital = Convert.ToBase64String(File.ReadAllBytes(AppSettings.filePath + certificadoPath)),
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
                        string qrPath = projectPath + AppSettings.cePath + $"{documento.Emisor.NroDocumento}\\QR\\";
                        if (!Directory.Exists(AppSettings.filePath + qrPath))
                        {
                            Directory.CreateDirectory(AppSettings.filePath + qrPath);
                        }
                        var imagen = Image.FromStream(mem);
                        string saveQRPath = qrPath + $"{documento.IdDocumento}.png";
                        imagen.Save(AppSettings.filePath + saveQRPath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }

                string xmlPath = projectPath + AppSettings.cePath + $"{documento.Emisor.NroDocumento}\\FacturaXML\\";
                if (!Directory.Exists(AppSettings.filePath + xmlPath))
                {
                    Directory.CreateDirectory(AppSettings.filePath + xmlPath);
                }

                string saveXMLPath = xmlPath + $"{documento.IdDocumento}.xml";
                File.WriteAllBytes(AppSettings.filePath + saveXMLPath, Convert.FromBase64String(firmadoResponse.TramaXmlFirmado));

                string logoPath = AppSettings.companyLogoPath + $"{documento.Emisor.NroDocumento}.png";

                if (File.Exists(AppSettings.filePath + logoPath))
                {
                    documento.Logo = String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(File.ReadAllBytes(AppSettings.filePath + logoPath)));
                }

                documento.QRFirmado = String.Format("data:image/gif;base64,{0}", firmadoResponse.CodigoQr);

                string pdfPath = PDF.ObtenerRutaPDFGenerado(documento, projectPath);

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

                    string zipPath = projectPath + AppSettings.cePath + $"{documento.Emisor.NroDocumento}\\TramaZipCdr\\";
                    if (!Directory.Exists(AppSettings.filePath + zipPath))
                    {
                        Directory.CreateDirectory(AppSettings.filePath + zipPath);
                    }

                    string saveZIPPath = zipPath + $"{documento.IdDocumento}.zip";

                    File.WriteAllBytes(AppSettings.filePath + saveZIPPath, Convert.FromBase64String(enviarDocumentoResponse.TramaZipCdr));

                    enviarDocumentoResponse.qrCode = documentoResponse.ValoresParaQr;
                    enviarDocumentoResponse.xmlPath = saveXMLPath;
                    enviarDocumentoResponse.pdfPath = pdfPath;
                }

                oElectronicReceiptBL.insertElectronicReceipt(enviarDocumentoResponse, documento);
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
                string projectPath = Array.Find(Project.projects, e => e == documento.Project);

                if (projectPath == null)
                {
                    throw new Exception("No existe una carpeta para el proyecto");
                }
                else
                {
                    projectPath = AppSettings.projectsPath + $"{projectPath}\\" + ((documento.EsTicketConsumo) ? AppSettings.tcsvfPath : AppSettings.cesvfPath);
                }

                documento.MontoEnLetras = Conversion.Enletras(documento.TotalVenta);

                string pdfPath = PDF.ObtenerRutaPDFGeneradoSinValorFiscal(documento, projectPath);

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


        [HttpPost, Route("sendXMLtoSUNAT")]
        public async Task<EnviarDocumentoResponse> sendXMLtoSUNAT([FromBody] SendXMLRequest sendXMLRequest)
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
                string projectPath = Array.Find(Project.projects, e => e == sendXMLRequest.project);

                if (projectPath == null)
                {
                    throw new Exception("No existe una carpeta para el proyecto");
                }
                else
                {
                    projectPath = AppSettings.projectsPath + $"{projectPath}\\";
                }

                string certificadoPath = AppSettings.certificadosPath + $"{sendXMLRequest.senderDocument}.pfx";

                if (!File.Exists(AppSettings.filePath + certificadoPath))
                {
                    throw new Exception("La empresa no cuenta con certificado");
                }

                Credencial credencial = Array.Find(CredencialEmpresa.credenciales, e => e.ruc == sendXMLRequest.senderDocument);

                if (credencial == null)
                {
                    throw new Exception("La empresa no cuenta con las credenciales SOL");
                }

                firmadoResponse.TramaXmlFirmado = Convert.ToBase64String(File.ReadAllBytes(AppSettings.filePath + sendXMLRequest.xmlPath));

                string IdDocumento = sendXMLRequest.series.ToString() + "-" + sendXMLRequest.correlative.ToString("D6");

                var documentoRequest = new EnviarDocumentoRequest
                {
                    Ruc = sendXMLRequest.senderDocument,
                    UsuarioSol = credencial.usuarioSol,
                    ClaveSol = credencial.claveSol,
                    EndPointUrl = UrlSunatPrueba,
                    IdDocumento = IdDocumento,
                    TipoDocumento = sendXMLRequest.receiptTypeId.ToString("D2"),
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

                    string zipPath = projectPath + AppSettings.cePath + $"{sendXMLRequest.senderDocument}\\TramaZipCdr\\";
                    if (!Directory.Exists(AppSettings.filePath + zipPath))
                    {
                        Directory.CreateDirectory(AppSettings.filePath + zipPath);
                    }

                    string saveZIPPath = zipPath + $"{IdDocumento}.zip";

                    File.WriteAllBytes(AppSettings.filePath + saveZIPPath, Convert.FromBase64String(enviarDocumentoResponse.TramaZipCdr));

                    enviarDocumentoResponse.qrCode = sendXMLRequest.qrCode;
                    enviarDocumentoResponse.xmlPath = sendXMLRequest.xmlPath;
                    enviarDocumentoResponse.pdfPath = sendXMLRequest.pdfPath;
                }

                // TODO Actualizar datos de facturacion
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