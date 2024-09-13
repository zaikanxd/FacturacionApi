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
using BusinessLogic;
using BusinessEntity.Dtos;
using System.Collections.Generic;
using BusinessEntity;
using System.Linq;
using Newtonsoft.Json;

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

        private const string urlSunat = UrlSunatPrueba;

        [AllowAnonymous]
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
                if (documento.TipoDocumento == ElectronicReceipt.ReceiptType.boleta && documento.Receptor.TipoDocumento == null && documento.Receptor.NroDocumento == null)
                {
                    if (documento.TotalVenta > ElectronicReceipt.montoMaximoBoletaSimple)
                    {
                        throw new Exception($"El monto de una boleta simple debe ser menor igual a {ElectronicReceipt.montoMaximoBoletaSimple}");
                    }
                    documento.Receptor.TipoDocumento = "0";
                    documento.Receptor.NroDocumento = "00000000";
                    documento.Receptor.NombreLegal = "Otros";
                }

                string projectPath = Array.Find(Project.projects, e => e == documento.Project);

                if (projectPath == null)
                {
                    throw new Exception("No existe una carpeta para el proyecto");
                } 
                else
                {
                    projectPath = AppSettings.projectsPath + $"{projectPath}\\";
                }

                decimal montoTotalDescuento = documento.Items.Sum(e => e.Descuento);
                montoTotalDescuento += documento.DescuentoGlobal;
                documento.MontoTotalDescuento = montoTotalDescuento;

                // 1: GENERAR XML

                var invoice = _documentoXml.Generar(documento);
                documentoResponse.TramaXmlSinFirma = await _serializador.GenerarXml(invoice);
                var serieCorrelativo = documento.IdDocumento.Split('-');
                documentoResponse.ValoresParaQr =
                    $"{documento.Emisor.NroDocumento}|{documento.TipoDocumento}|{serieCorrelativo[0]}|{serieCorrelativo[1]}|{documento.TotalIgv:N2}|{documento.TotalVenta:N2}|{Convert.ToDateTime(documento.FechaEmision):yyyy-MM-dd}|{documento.Receptor.TipoDocumento}|{documento.Receptor.NroDocumento}|";

                documentoResponse.Exito = true;

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

                        // Verificar y guardar archivos repetidos
                        if (File.Exists(AppSettings.filePath + saveQRPath))
                        {
                            int i = 1;
                            while (File.Exists(AppSettings.filePath + saveQRPath.Replace(".png", $"({i}).png")))
                            { 
                                i++; 
                            }
                            saveQRPath = saveQRPath.Replace(".png", $"({i}).png");
                        }

                        imagen.Save(AppSettings.filePath + saveQRPath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }

                string xmlPath = projectPath + AppSettings.cePath + $"{documento.Emisor.NroDocumento}\\FacturaXML\\";

                if (!Directory.Exists(AppSettings.filePath + xmlPath))
                {
                    Directory.CreateDirectory(AppSettings.filePath + xmlPath);
                }

                string saveXMLPath = xmlPath + $"{documento.IdDocumento}.xml";

                // Verificar y guardar archivos repetidos
                if (File.Exists(AppSettings.filePath + saveXMLPath))
                {
                    int i = 1;
                    while (File.Exists(AppSettings.filePath + saveXMLPath.Replace(".xml", $"({i}).xml")))
                    {
                        i++;
                    }
                    saveXMLPath = saveXMLPath.Replace(".xml", $"({i}).xml");
                }
                
                File.WriteAllBytes(AppSettings.filePath + saveXMLPath, Convert.FromBase64String(firmadoResponse.TramaXmlFirmado));

                string logoPath = AppSettings.logosPath + $"{documento.Emisor.NroDocumento}.png";

                if (File.Exists(AppSettings.filePath + logoPath))
                {
                    documento.Logo = String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(File.ReadAllBytes(AppSettings.filePath + logoPath)));
                }

                documento.QRFirmado = String.Format("data:image/gif;base64,{0}", firmadoResponse.CodigoQr);

                string pdfPath = PDF.ObtenerRutaPDFGenerado(documento, projectPath, false);

                var documentoRequest = new EnviarDocumentoRequest
                {
                    Ruc = documento.Emisor.NroDocumento,
                    UsuarioSol = credencial.usuarioSol,
                    ClaveSol = credencial.claveSol,
                    EndPointUrl = urlSunat,
                    IdDocumento = documento.IdDocumento,
                    TipoDocumento = documento.TipoDocumento,
                    TramaXmlFirmado = firmadoResponse.TramaXmlFirmado
                };

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

                if (resultado.Exito)
                {
                    enviarDocumentoResponse = await _serializador.GenerarDocumentoRespuesta(resultado.ConstanciaDeRecepcion);
                    enviarDocumentoResponse.NombreArchivo = nombreArchivo;

                    string zipPath = projectPath + AppSettings.cePath + $"{documento.Emisor.NroDocumento}\\TramaZipCdr\\";

                    if (!Directory.Exists(AppSettings.filePath + zipPath))
                    {
                        Directory.CreateDirectory(AppSettings.filePath + zipPath);
                    }

                    string saveZIPPath = zipPath + $"{documento.IdDocumento}.zip";

                    File.WriteAllBytes(AppSettings.filePath + saveZIPPath, Convert.FromBase64String(enviarDocumentoResponse.TramaZipCdr));

                    enviarDocumentoResponse.cdrPath = saveZIPPath;
                } 
                else
                {
                    enviarDocumentoResponse.Exito = false;
                    enviarDocumentoResponse.MensajeError = resultado.MensajeError;
                }

                enviarDocumentoResponse.qrCode = documentoResponse.ValoresParaQr;
                enviarDocumentoResponse.xmlPath = saveXMLPath;
                enviarDocumentoResponse.pdfPath = pdfPath;

                // Guardar JSON

                string jsonPath = projectPath + AppSettings.cePath + $"{documento.Emisor.NroDocumento}\\JSON\\";

                if (!Directory.Exists(AppSettings.filePath + jsonPath))
                {
                    Directory.CreateDirectory(AppSettings.filePath + jsonPath);
                }

                string saveJSONPath = jsonPath + $"{documento.IdDocumento}.json";

                // Verificar y guardar archivos repetidos
                if (File.Exists(AppSettings.filePath + saveJSONPath))
                {
                    int i = 1;
                    while (File.Exists(AppSettings.filePath + saveJSONPath.Replace(".json", $"({i}).json")))
                    {
                        i++;
                    }
                    saveJSONPath = saveJSONPath.Replace(".json", $"({i}).json");
                }
                
                File.WriteAllText(AppSettings.filePath + saveJSONPath, JsonConvert.SerializeObject(documento, Formatting.Indented));

                oElectronicReceiptBL.insertElectronicReceipt(enviarDocumentoResponse, documento, saveJSONPath);

                if (resultado.MensajeError != null)
                {
                    if (resultado.MensajeError.Contains("0111"))
                    {
                        enviarDocumentoResponse.Exito = true;
                        enviarDocumentoResponse.MensajeError = null;
                    }
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

        [AllowAnonymous]
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

                decimal montoTotalDescuento = documento.Items.Sum(e => e.Descuento);
                montoTotalDescuento += documento.DescuentoGlobal;
                documento.MontoTotalDescuento = montoTotalDescuento;

                decimal cantidadTotalProductos = 0;
                documento.Items.ForEach((e) => {
                    cantidadTotalProductos += e.Cantidad;
                });
                documento.CantidadTotalProductos = cantidadTotalProductos;
                documento.MontoEnLetras = Conversion.Enletras(documento.TotalVenta);

                // Guardar JSON

                string jsonPath = projectPath + $"{documento.CompanyId}\\";

                if (!Directory.Exists(AppSettings.filePath + jsonPath))
                {
                    Directory.CreateDirectory(AppSettings.filePath + jsonPath);
                }

                string saveJSONPath = jsonPath + $"{documento.IdDocumento}.json";

                // Verificar y guardar archivos repetidos
                if (File.Exists(AppSettings.filePath + saveJSONPath))
                {
                    int i = 1;
                    while (File.Exists(AppSettings.filePath + saveJSONPath.Replace(".json", $"({i}).json")))
                    {
                        i++;
                    }
                    saveJSONPath = saveJSONPath.Replace(".json", $"({i}).json");
                }

                File.WriteAllText(AppSettings.filePath + saveJSONPath, JsonConvert.SerializeObject(documento, Formatting.Indented));

                string pdfPath = PDF.ObtenerRutaPDFGenerado(documento, projectPath, true);

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

        [AllowAnonymous]
        [HttpPost, Route("vistaPrevia")]
        public FilePreview vistaPrevia([FromBody] FilePreviewRequest filePreviewRequest)
        {
            FilePreview filePreview = new FilePreview();

            DocumentoElectronico documento = filePreviewRequest.documento;

            documento.EsVistaPrevia = true;

            if (filePreviewRequest.sinValorFiscal)
            {
                // SIN VALOR FISCAL
                decimal montoTotalDescuento = documento.Items.Sum(e => e.Descuento);
                montoTotalDescuento += documento.DescuentoGlobal;
                documento.MontoTotalDescuento = montoTotalDescuento;

                decimal cantidadTotalProductos = 0;
                documento.Items.ForEach((e) => {
                    cantidadTotalProductos += e.Cantidad;
                });
                documento.CantidadTotalProductos = cantidadTotalProductos;
            } else
            {
                // FACTURA O BOLETA
                if (documento.TipoDocumento == ElectronicReceipt.ReceiptType.boleta && documento.Receptor.TipoDocumento == null && documento.Receptor.NroDocumento == null)
                {
                    if (documento.TotalVenta > ElectronicReceipt.montoMaximoBoletaSimple)
                    {
                        throw new Exception($"El monto de una boleta simple debe ser menor igual a {ElectronicReceipt.montoMaximoBoletaSimple}");
                    }
                    documento.Receptor.TipoDocumento = "0";
                    documento.Receptor.NroDocumento = "00000000";
                    documento.Receptor.NombreLegal = "Otros";
                }

                decimal montoTotalDescuento = documento.Items.Sum(e => e.Descuento);
                montoTotalDescuento += documento.DescuentoGlobal;
                documento.MontoTotalDescuento = montoTotalDescuento;

                var serieCorrelativo = documento.IdDocumento.Split('-');
                string valoresParaQr =
                    $"{documento.Emisor.NroDocumento}|{documento.TipoDocumento}|{serieCorrelativo[0]}|{serieCorrelativo[1]}|{documento.TotalIgv:N2}|{documento.TotalVenta:N2}|{Convert.ToDateTime(documento.FechaEmision):yyyy-MM-dd}|{documento.Receptor.TipoDocumento}|{documento.Receptor.NroDocumento}|";

                string codigoQr = QrHelper.GenerarImagenQr($"{valoresParaQr}");

                string logoPath = AppSettings.logosPath + $"{documento.Emisor.NroDocumento}.png";

                if (File.Exists(AppSettings.filePath + logoPath))
                {
                    documento.Logo = String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(File.ReadAllBytes(AppSettings.filePath + logoPath)));
                }

                documento.QRFirmado = String.Format("data:image/gif;base64,{0}", codigoQr);
            }

            documento.MontoEnLetras = Conversion.Enletras(documento.TotalVenta);
            filePreview.bytes = PDF.ObtenerBytesPDFGenerado(documento, filePreviewRequest.sinValorFiscal);
            filePreview.name = "VistaPrevia-" + documento.IdDocumento;

            return filePreview;
        }

        [AllowAnonymous]
        [HttpPost, Route("comunicacionBaja")]
        public async Task<EnviarResumenResponse> comunicacionBaja([FromBody] ComunicacionBaja comunicacionBaja)
        {
            // 1: GENERAR XML
            IDocumentoXml _documentoXml = new ComunicacionBajaXml();
            ISerializador _serializador = new Serializador();
            var documentoResponse = new DocumentoResponse();

            // 2: FIRMAR XML
            ICertificador _certificador = new Certificador();
            var firmadoResponse = new FirmadoResponse();

            // 3: ENVIAR COMUNICACION DE BAJA
            IServicioSunatDocumentos _servicioSunatDocumentos = new ServicioSunatDocumentos();
            var enviarResumenResponse = new EnviarResumenResponse();

            try
            {
                string projectPath = Array.Find(Project.projects, e => e == comunicacionBaja.Project);

                if (projectPath == null)
                {
                    throw new Exception("No existe una carpeta para el proyecto");
                }
                else
                {
                    projectPath = AppSettings.projectsPath + $"{projectPath}\\";
                }

                // 1: GENERAR XML

                var voidedDocument = _documentoXml.Generar(comunicacionBaja);
                documentoResponse.TramaXmlSinFirma = await _serializador.GenerarXml(voidedDocument);
                documentoResponse.Exito = true;

                // 2: FIRMAR XML

                string certificadoPath = AppSettings.certificadosPath + $"{comunicacionBaja.Emisor.NroDocumento}.pfx";

                if (!File.Exists(AppSettings.filePath + certificadoPath))
                {
                    throw new Exception("La empresa no cuenta con certificado");
                }

                Credencial credencial = Array.Find(CredencialEmpresa.credenciales, e => e.ruc == comunicacionBaja.Emisor.NroDocumento);

                if (credencial == null)
                {
                    throw new Exception("La empresa no cuenta con las credenciales SOL");
                }

                var firmadoRequest = new FirmadoRequest
                {
                    TramaXmlSinFirma = documentoResponse.TramaXmlSinFirma,
                    CertificadoDigital = Convert.ToBase64String(File.ReadAllBytes(AppSettings.filePath + certificadoPath)),
                    PasswordCertificado = credencial.passwordCertificado,
                };

                firmadoResponse = await _certificador.FirmarXml(firmadoRequest);
                firmadoResponse.Exito = true;

                string bajaXmlPath = projectPath + AppSettings.cePath + $"{comunicacionBaja.Emisor.NroDocumento}\\ComunicacionBajaXML\\";

                if (!Directory.Exists(AppSettings.filePath + bajaXmlPath))
                {
                    Directory.CreateDirectory(AppSettings.filePath + bajaXmlPath);
                }

                string saveBajaXMLPath = bajaXmlPath + $"{comunicacionBaja.IdDocumento}.xml";

                // Verificar y guardar archivos repetidos
                if (File.Exists(AppSettings.filePath + saveBajaXMLPath))
                {
                    int i = 1;
                    while (File.Exists(AppSettings.filePath + saveBajaXMLPath.Replace(".xml", $"({i}).xml")))
                    {
                        i++;
                    }
                    saveBajaXMLPath = saveBajaXMLPath.Replace(".xml", $"({i}).xml");
                }

                File.WriteAllBytes(AppSettings.filePath + saveBajaXMLPath, Convert.FromBase64String(firmadoResponse.TramaXmlFirmado));

                var documentoRequest = new EnviarDocumentoRequest
                {
                    Ruc = comunicacionBaja.Emisor.NroDocumento,
                    UsuarioSol = credencial.usuarioSol,
                    ClaveSol = credencial.claveSol,
                    EndPointUrl = urlSunat,
                    IdDocumento = comunicacionBaja.IdDocumento,
                    TramaXmlFirmado = firmadoResponse.TramaXmlFirmado
                };

                // 3: ENVIAR COMUNICACION DE BAJA

                var nombreArchivo = $"{documentoRequest.Ruc}-{documentoRequest.IdDocumento}";
                var tramaZip = await _serializador.GenerarZip(documentoRequest.TramaXmlFirmado, nombreArchivo);

                _servicioSunatDocumentos.Inicializar(new ParametrosConexion
                {
                    Ruc = documentoRequest.Ruc,
                    UserName = documentoRequest.UsuarioSol,
                    Password = documentoRequest.ClaveSol,
                    EndPointUrl = documentoRequest.EndPointUrl
                });

                var resultado = _servicioSunatDocumentos.EnviarResumen(new DocumentoSunat
                {
                    NombreArchivo = $"{nombreArchivo}.zip",
                    TramaXml = tramaZip
                });

                if (resultado.Exito)
                {
                    CancelElectronicReceiptRequest cancelElectronicReceiptRequest = new CancelElectronicReceiptRequest();

                    enviarResumenResponse.NroTicket = resultado.NumeroTicket;
                    enviarResumenResponse.Exito = true;
                    enviarResumenResponse.NombreArchivo = nombreArchivo;

                    _servicioSunatDocumentos.Inicializar(new ParametrosConexion
                    {
                        Ruc = comunicacionBaja.Emisor.NroDocumento,
                        UserName = credencial.usuarioSol,
                        Password = credencial.claveSol,
                        EndPointUrl = urlSunat
                    });

                    var resultadoTicket = _servicioSunatDocumentos.ConsultarTicket(resultado.NumeroTicket);

                    if (resultadoTicket.Exito)
                    {
                        var enviarDocumentoResponse = new EnviarDocumentoResponse();
                        enviarDocumentoResponse = await _serializador.GenerarDocumentoRespuesta(resultadoTicket.ConstanciaDeRecepcion);

                        string comunicacionBajaZipPath = projectPath + AppSettings.cePath + $"{comunicacionBaja.Emisor.NroDocumento}\\ComunicacionBajaZipCdr\\";

                        if (!Directory.Exists(AppSettings.filePath + comunicacionBajaZipPath))
                        {
                            Directory.CreateDirectory(AppSettings.filePath + comunicacionBajaZipPath);
                        }

                        string saveZIPPath = comunicacionBajaZipPath + $"{comunicacionBaja.IdDocumento}.zip";

                        File.WriteAllBytes(AppSettings.filePath + saveZIPPath, Convert.FromBase64String(enviarDocumentoResponse.TramaZipCdr));

                        cancelElectronicReceiptRequest.canceledCdrLink = saveZIPPath;
                    }

                    string jsonPath = projectPath + AppSettings.cePath + $"{comunicacionBaja.Emisor.NroDocumento}\\JSON\\";

                    jsonPath = jsonPath + "FFF3-0000001.json";

                    string jsonString = File.ReadAllText(AppSettings.filePath + jsonPath);

                    DocumentoElectronico documento = JsonConvert.DeserializeObject<DocumentoElectronico>(jsonString);

                    documento.EstaAnulado = true;

                    string pdfPath = PDF.ObtenerRutaPDFGenerado(documento, projectPath, false);

                    cancelElectronicReceiptRequest.project = comunicacionBaja.Project;
                    cancelElectronicReceiptRequest.nroRUC = comunicacionBaja.Emisor.NroDocumento;
                    cancelElectronicReceiptRequest.series = comunicacionBaja.Bajas.First().Serie;
                    cancelElectronicReceiptRequest.correlative = comunicacionBaja.Bajas.First().Correlativo;
                    cancelElectronicReceiptRequest.cancellationReason = comunicacionBaja.Bajas.First().MotivoBaja;
                    cancelElectronicReceiptRequest.cancellationName = comunicacionBaja.IdDocumento;
                    cancelElectronicReceiptRequest.canceledPdfLink = pdfPath;
                    cancelElectronicReceiptRequest.canceledXmlLink = saveBajaXMLPath;
                    cancelElectronicReceiptRequest.canceledTicketNumber = resultado.NumeroTicket;

                    oElectronicReceiptBL.cancelElectronicReceipt(cancelElectronicReceiptRequest);
                }
                else
                {
                    enviarResumenResponse.MensajeError = resultado.MensajeError;
                    enviarResumenResponse.Exito = false;
                }
            }
            catch (Exception ex)
            {
                enviarResumenResponse.MensajeError = ex.Message;
                enviarResumenResponse.Pila = ex.StackTrace;
                enviarResumenResponse.Exito = false;
            }

            return enviarResumenResponse;
        }

        public async Task<EnviarDocumentoResponse> consultarTicket(TicketRequest ticketRequest)
        {
            // 1: ENVIAR DOCUMENTO
            IServicioSunatDocumentos _servicioSunatDocumentos = new ServicioSunatDocumentos();
            var enviarDocumentoResponse = new EnviarDocumentoResponse();

            // 2: GENERAR RESPUESTA
            ISerializador _serializador = new Serializador();

            try
            {
                string projectPath = Array.Find(Project.projects, e => e == ticketRequest.Project);

                if (projectPath == null)
                {
                    throw new Exception("No existe una carpeta para el proyecto");
                }
                else
                {
                    projectPath = AppSettings.projectsPath + $"{projectPath}\\";
                }

                Credencial credencial = Array.Find(CredencialEmpresa.credenciales, e => e.ruc == ticketRequest.NroRUC);

                if (credencial == null)
                {
                    throw new Exception("La empresa no cuenta con las credenciales SOL");
                }

                _servicioSunatDocumentos.Inicializar(new ParametrosConexion
                {
                    Ruc = ticketRequest.NroRUC,
                    UserName = credencial.usuarioSol,
                    Password = credencial.claveSol,
                    EndPointUrl = urlSunat
                });

                var resultado = _servicioSunatDocumentos.ConsultarTicket(ticketRequest.NroTicket);

                if (!resultado.Exito)
                {
                    enviarDocumentoResponse.Exito = false;
                    enviarDocumentoResponse.MensajeError = resultado.MensajeError;
                }
                else
                {
                    enviarDocumentoResponse = await _serializador.GenerarDocumentoRespuesta(resultado.ConstanciaDeRecepcion);
                }
            }
            catch (Exception ex)
            {
                enviarDocumentoResponse.MensajeError = ex.Source == "DotNetZip" ? "El Ticket no existe" : ex.Message;
                enviarDocumentoResponse.Pila = ex.StackTrace;
                enviarDocumentoResponse.Exito = false;
            }

            return enviarDocumentoResponse;
        }

        [Authorize]
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
                    EndPointUrl = urlSunat,
                    IdDocumento = IdDocumento,
                    TipoDocumento = sendXMLRequest.receiptTypeId.ToString("D2"),
                    TramaXmlFirmado = firmadoResponse.TramaXmlFirmado
                };

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
                    enviarDocumentoResponse.cdrPath = saveZIPPath;
                }

                oElectronicReceiptBL.updateElectronicReceipt(sendXMLRequest.id, enviarDocumentoResponse);
            }
            catch (Exception ex)
            {
                enviarDocumentoResponse.MensajeError = ex.Message;
                enviarDocumentoResponse.Pila = ex.StackTrace;
                enviarDocumentoResponse.Exito = false;
            }

            return enviarDocumentoResponse;
        }

        [AllowAnonymous]
        [HttpPost, Route("sendAllPendingXMLtoSUNAT")]
        public void sendAllPendingXMLtoSUNAT()
        {
            List<ElectronicReceiptBE> list = oElectronicReceiptBL.getListPending();
            if (list.Count > 0)
            {
                list.ForEach(async (electronicReceipt) => {
                    SendXMLRequest sendXMLRequest = new SendXMLRequest();
                    sendXMLRequest.id = electronicReceipt.id;
                    sendXMLRequest.project = electronicReceipt.project;
                    sendXMLRequest.senderDocument = electronicReceipt.senderDocument;
                    sendXMLRequest.series = electronicReceipt.series;
                    sendXMLRequest.correlative = electronicReceipt.correlative;
                    sendXMLRequest.xmlPath = electronicReceipt.xmlLink;
                    sendXMLRequest.pdfPath = electronicReceipt.pdfLink;
                    sendXMLRequest.qrCode = electronicReceipt.qrCode;
                    sendXMLRequest.receiptTypeId = electronicReceipt.receiptTypeId;
                    await sendXMLtoSUNAT(sendXMLRequest);
                });
            }
        }

        [Authorize]
        [HttpGet, Route("listBy")]
        public IEnumerable<ElectronicReceiptBE> getListBy(DateTime date, string filter = null)
        {
            return oElectronicReceiptBL.getListBy(date, filter);
        }

        [Authorize]
        [HttpGet, Route("get")]
        public ElectronicReceiptBE get(int id)
        {
            return oElectronicReceiptBL.get(id);
        }
    }
}