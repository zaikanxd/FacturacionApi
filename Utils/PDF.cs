using Dto.Modelos;
using System;
using System.Diagnostics;
using System.IO;

namespace FacturacionApi.Utils
{
    public static class AppSettings
    {
        public static string filePath = System.Configuration.ConfigurationManager.AppSettings["filePath"];
        public static string projectsPath = System.Configuration.ConfigurationManager.AppSettings["projectsPath"];
        public static string cePath = System.Configuration.ConfigurationManager.AppSettings["cePath"];
        public static string cesvfPath = System.Configuration.ConfigurationManager.AppSettings["cesvfPath"];
        public static string tcsvfPath = System.Configuration.ConfigurationManager.AppSettings["tcsvfPath"];
        public static string certificadosPath = System.Configuration.ConfigurationManager.AppSettings["certificadosPath"];
        public static string companyLogoPath = System.Configuration.ConfigurationManager.AppSettings["companyLogoPath"];
        public static string logosPath = System.Configuration.ConfigurationManager.AppSettings["logosPath"];
    }

    public static class PDF
    {
        public static string ObtenerRutaPDFGenerado(DocumentoElectronico documento, string projectPath)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Plantillas\\";
            string HTMLTempPath = path + "HTMLTemp" + documento.Emisor.NroDocumento + documento.IdDocumento + ".html";

            // Verificar y guardar archivos duplicados
            if (File.Exists(HTMLTempPath))
            {
                int i = 1;
                while (File.Exists(HTMLTempPath.Replace(".html", $"({i}).html")))
                {
                    i++;
                }
                HTMLTempPath = HTMLTempPath.Replace(".html", $"({i}).html");
            }

            string HTMLPlantillaPath = path + ((Formato.A4 == documento.formato) ? "A4.html" : "TICKET.html");
            
            string sHtml = GetStringOfFile(HTMLPlantillaPath);
            string resultHtml = RazorEngine.Razor.Parse(sHtml, documento);

            File.WriteAllText(HTMLTempPath, resultHtml);

            string WKHTMLTOPDFPath = AppDomain.CurrentDomain.BaseDirectory + "\\wkhtmltopdf\\wkhtmltopdf.exe";
            
            string pdfPath = projectPath + AppSettings.cePath + $"{documento.Emisor.NroDocumento}\\PDF\\";
            if (!Directory.Exists(AppSettings.filePath + pdfPath))
            {
                Directory.CreateDirectory(AppSettings.filePath + pdfPath);
            }
            string savePDFPath = pdfPath + $"{documento.IdDocumento}.pdf";

            // Verificar y guardar archivos repetidos
            if (File.Exists(AppSettings.filePath + savePDFPath))
            {
                int i = 1;
                while (File.Exists(AppSettings.filePath + savePDFPath.Replace(".pdf", $"({i}).pdf")))
                {
                    i++;
                }
                savePDFPath = savePDFPath.Replace(".pdf", $"({i}).pdf");
            }

            ProcessStartInfo oProcessStartInfo = new ProcessStartInfo();
            oProcessStartInfo.UseShellExecute = false;
            oProcessStartInfo.FileName = WKHTMLTOPDFPath;

            if (Formato.A4 == documento.formato)
            {
                oProcessStartInfo.Arguments = $"{HTMLTempPath}" + " " + $"{AppSettings.filePath + savePDFPath}";
            } else
            {
                oProcessStartInfo.Arguments = $"-T 0 -B 0 --margin-left 0 --margin-right 0 --page-width 80mm --page-height {180 + (documento.Items.Count * 25)}mm" + " " + $"{HTMLTempPath}" + " " + $"{AppSettings.filePath + savePDFPath}";
            }

            using (Process oProcess = Process.Start(oProcessStartInfo))
            {
                oProcess.WaitForExit();
            }

            File.Delete(HTMLTempPath);

            return savePDFPath;
        }

        public static string ObtenerRutaPDFGeneradoSinValorFiscal(DocumentoElectronico documento, string projectPath)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Plantillas\\";
            string HTMLTempPath = path + "HTMLTemp" + $"{documento.CompanyId}" + documento.IdDocumento + ".html";

            // Verificar y guardar archivos duplicados
            if (File.Exists(HTMLTempPath))
            {
                int i = 1;
                while (File.Exists(HTMLTempPath.Replace(".html", $"({i}).html")))
                {
                    i++;
                }
                HTMLTempPath = HTMLTempPath.Replace(".html", $"({i}).html");
            }

            string HTMLPlantillaPath = path + "TICKET_SIN_VALOR_FISCAL.html";

            string sHtml = GetStringOfFile(HTMLPlantillaPath);
            string resultHtml = RazorEngine.Razor.Parse(sHtml, documento);

            File.WriteAllText(HTMLTempPath, resultHtml);

            string WKHTMLTOPDFPath = AppDomain.CurrentDomain.BaseDirectory + "\\wkhtmltopdf\\wkhtmltopdf.exe";

            string pdfPath = projectPath + $"{documento.CompanyId}\\";
            if (!Directory.Exists(AppSettings.filePath + pdfPath))
            {
                Directory.CreateDirectory(AppSettings.filePath + pdfPath);
            }
            string savePDFPath = pdfPath + $"{documento.IdDocumento}.pdf";

            // Verificar y guardar archivos repetidos
            if (File.Exists(AppSettings.filePath + savePDFPath))
            {
                int i = 1;
                while (File.Exists(AppSettings.filePath + savePDFPath.Replace(".pdf", $"({i}).pdf")))
                {
                    i++;
                }
                savePDFPath = savePDFPath.Replace(".pdf", $"({i}).pdf");
            }

            ProcessStartInfo oProcessStartInfo = new ProcessStartInfo();
            oProcessStartInfo.UseShellExecute = false;
            oProcessStartInfo.FileName = WKHTMLTOPDFPath;
            
            oProcessStartInfo.Arguments = $"-T 0 -B 0 --margin-left 0 --margin-right 0 --page-width 80mm --page-height {100 + (documento.Items.Count * 25)}mm" + " " + $"{HTMLTempPath}" + " " + $"{AppSettings.filePath + savePDFPath}";

            using (Process oProcess = Process.Start(oProcessStartInfo))
            {
                oProcess.WaitForExit();
            }

            File.Delete(HTMLTempPath);

            return savePDFPath;
        }

        public static byte[] ObtenerBytesPDFGenerado(DocumentoElectronico documento, bool sinValorFiscal)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Plantillas\\";
            
            string HTMLTempPath = sinValorFiscal 
                ? path + "HTMLTemp" + $"{documento.CompanyId}" + documento.IdDocumento + ".html"
                : path + "HTMLTemp" + documento.Emisor.NroDocumento + documento.IdDocumento + ".html";

            // Verificar y guardar archivos duplicados
            if (File.Exists(HTMLTempPath))
            {
                int i = 1;
                while (File.Exists(HTMLTempPath.Replace(".html", $"({i}).html")))
                {
                    i++;
                }
                HTMLTempPath = HTMLTempPath.Replace(".html", $"({i}).html");
            }

            string HTMLPlantillaPath = sinValorFiscal
                ? path + "TICKET_SIN_VALOR_FISCAL.html"
                : path + (Formato.A4 == documento.formato ? "A4.html" : "TICKET.html");

            string sHtml = GetStringOfFile(HTMLPlantillaPath);
            string resultHtml = RazorEngine.Razor.Parse(sHtml, documento);

            File.WriteAllText(HTMLTempPath, resultHtml);

            string WKHTMLTOPDFPath = AppDomain.CurrentDomain.BaseDirectory + "\\wkhtmltopdf\\wkhtmltopdf.exe";

            string PDFTempPath = sinValorFiscal
               ? path + "PDFTemp" + $"{documento.CompanyId}" + documento.IdDocumento + ".pdf"
               : path + "PDFTemp" + documento.Emisor.NroDocumento + documento.IdDocumento + ".pdf";

            // Verificar y guardar archivos duplicados
            if (File.Exists(PDFTempPath))
            {
                int i = 1;
                while (File.Exists(PDFTempPath.Replace(".pdf", $"({i}).pdf")))
                {
                    i++;
                }
                PDFTempPath = PDFTempPath.Replace(".pdf", $"({i}).pdf");
            }

            ProcessStartInfo oProcessStartInfo = new ProcessStartInfo();
            oProcessStartInfo.UseShellExecute = false;
            oProcessStartInfo.RedirectStandardOutput = true;
            oProcessStartInfo.FileName = WKHTMLTOPDFPath;

            if (sinValorFiscal)
            {
                oProcessStartInfo.Arguments = $"-T 0 -B 0 --margin-left 0 --margin-right 0 --page-width 80mm --page-height {100 + (documento.Items.Count * 25)}mm" + " " + $"{HTMLTempPath}" + " " + $"{PDFTempPath}";
            }
            else
            {
                oProcessStartInfo.Arguments = (Formato.A4 == documento.formato)
                    ? $"{HTMLTempPath}" + " " + $"{PDFTempPath}"
                    : $"-T 0 -B 0 --margin-left 0 --margin-right 0 --page-width 80mm --page-height {180 + (documento.Items.Count * 25)}mm" + " " + $"{HTMLTempPath}" + " " + $"{PDFTempPath}";
            }

            using (Process oProcess = Process.Start(oProcessStartInfo))
            {
                oProcess.WaitForExit();
            }

            byte[] pdfBytes = File.ReadAllBytes(PDFTempPath);

            File.Delete(PDFTempPath);
            File.Delete(HTMLTempPath);

            return pdfBytes;
        }

        private static string GetStringOfFile(string pathFile)
        {
            string contenido = File.ReadAllText(pathFile);
            return contenido;
        }

        public struct Formato
        {
            public const string A4 = "A4";
            public const string TICKET = "TICKET";
        }
    }

    public struct ElectronicReceipt
    {
        public struct ReceiptType
        {
            public const string boleta = "03";
        }

        public const decimal montoMaximoBoletaSimple = 700;
    }
    
    public struct CredencialEmpresa
    {
        public static readonly Credencial[] credenciales = {
            // PRUEBAS
            new Credencial("20603099126", "MODDATOS", "MODDATOS", string.Empty),
            new Credencial("22222222222", "MODDATOS", "MODDATOS", string.Empty),
            new Credencial("88888888888", "MODDATOS", "MODDATOS", string.Empty),
            new Credencial("20609800811", "MODDATOS", "MODDATOS", string.Empty),
            new Credencial("20607076805", "MODDATOS", "MODDATOS", string.Empty),
            new Credencial("20607157082", "MODDATOS", "MODDATOS", string.Empty),
            new Credencial("20612812145", "MODDATOS", "MODDATOS", string.Empty),
            new Credencial("21667899876", "MODDATOS", "MODDATOS", string.Empty),
            // PRODUCCION
            /*new Credencial("20603099126", "RM2024PE", "Gurklansi24", "gurklansi206"),
            new Credencial("20607076805", "HA2024PE", "Hreadfaxy24", "hreadfaxy206"),
            new Credencial("20607157082", "LL2024PE", "Tescressi24", "tescressi206"),
            new Credencial("20612812145", "JR2024PE", "Nsennerpu24", "nsennerpu206"),
            */
        };
    }

    public class Credencial
    {
        public string ruc { get; set; }
        public string usuarioSol { get; set; }
        public string claveSol { get; set; }
        public string passwordCertificado { get; set; }
        public Credencial(string ruc, string usuarioSol, string claveSol, string passwordCertificado)
        {
            this.ruc = ruc;
            this.usuarioSol = usuarioSol;
            this.claveSol = claveSol;
            this.passwordCertificado = passwordCertificado;
        }
    }

    public class Project
    {
        public static readonly string[] projects = {
            "Creditos",
            "Commerce",
        };
    }
}