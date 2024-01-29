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

        public static string cnxBillingBD = System.Configuration.ConfigurationManager.AppSettings["cnxBillingBD"];
    }

    public static class PDF
    {
        public static string ObtenerRutaPDFGenerado(DocumentoElectronico documento, string projectPath)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Plantillas\\";
            string HTMLTempPath = path + "HTMLTemp" + documento.Emisor.NroDocumento + documento.IdDocumento + ".html";

            string HTMLPlantillaPath = path + ((Formato.A4 == documento.formato) ? "A4.html" : "TICKET.html");
            
            string sHtml = GetStringOfFile(HTMLPlantillaPath);
            string resultHtml = "";

            resultHtml = RazorEngine.Razor.Parse(sHtml, documento);

            File.WriteAllText(HTMLTempPath, resultHtml);

            string WKHTMLTOPDFPath = AppDomain.CurrentDomain.BaseDirectory + "\\wkhtmltopdf\\wkhtmltopdf.exe";

            string pdfPath = projectPath + AppSettings.cePath + $"{documento.Emisor.NroDocumento}\\PDF\\";
            if (!Directory.Exists(AppSettings.filePath + pdfPath))
            {
                Directory.CreateDirectory(AppSettings.filePath + pdfPath);
            }
            string savePDFPath = pdfPath + $"{documento.IdDocumento}.pdf";

            ProcessStartInfo oProcessStartInfo = new ProcessStartInfo();
            oProcessStartInfo.UseShellExecute = false;
            oProcessStartInfo.FileName = WKHTMLTOPDFPath;

            if (Formato.A4 == documento.formato)
            {
                oProcessStartInfo.Arguments = $"{HTMLTempPath}" + " " + $"{AppSettings.filePath + savePDFPath}";
            } else
            {
                oProcessStartInfo.Arguments = $"-T 0 -B 0 --margin-left 0 --margin-right 0 --page-width 80mm --page-height {160 + (documento.Items.Count * 15)}mm" + " " + $"{HTMLTempPath}" + " " + $"{AppSettings.filePath + savePDFPath}";
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

            string HTMLPlantillaPath = path + "TICKET_SIN_VALOR_FISCAL.html";

            string sHtml = GetStringOfFile(HTMLPlantillaPath);
            string resultHtml = "";

            resultHtml = RazorEngine.Razor.Parse(sHtml, documento);

            File.WriteAllText(HTMLTempPath, resultHtml);

            string WKHTMLTOPDFPath = AppDomain.CurrentDomain.BaseDirectory + "\\wkhtmltopdf\\wkhtmltopdf.exe";

            string pdfPath = projectPath + $"{documento.CompanyId}\\";
            if (!Directory.Exists(AppSettings.filePath + pdfPath))
            {
                Directory.CreateDirectory(AppSettings.filePath + pdfPath);
            }
            string savePDFPath = pdfPath + $"{documento.IdDocumento}.pdf";

            ProcessStartInfo oProcessStartInfo = new ProcessStartInfo();
            oProcessStartInfo.UseShellExecute = false;
            oProcessStartInfo.FileName = WKHTMLTOPDFPath;
            
            oProcessStartInfo.Arguments = $"-T 0 -B 0 --margin-left 0 --margin-right 0 --page-width 80mm --page-height {80 + (documento.Items.Count * 15)}mm" + " " + $"{HTMLTempPath}" + " " + $"{AppSettings.filePath + savePDFPath}";

            using (Process oProcess = Process.Start(oProcessStartInfo))
            {
                oProcess.WaitForExit();
            }

            File.Delete(HTMLTempPath);

            return savePDFPath;
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

    public struct CredencialEmpresa
    {
        public static readonly Credencial[] credenciales = {
            new Credencial("20603099126", "MODDATOS", "MODDATOS", string.Empty),
            new Credencial("22222222222", "MODDATOS", "MODDATOS", string.Empty),
            new Credencial("88888888888", "MODDATOS", "MODDATOS", string.Empty),
            new Credencial("20607076805", "MODDATOS", "MODDATOS", string.Empty),
            new Credencial("20609800811", "MODDATOS", "MODDATOS", string.Empty),
            //new Credencial("20603099126", "RM2024PE", "Gurklansi24", "gurklansi206"),
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

    public static class GetNameStoreProcedure
    {
        public const string bi_ElectronicReceipt_Insert = "bi_ElectronicReceipt_Insert";
    }
}