using Dto.Modelos;
using System;
using System.Diagnostics;
using System.IO;

namespace FacturacionApi.Utils
{
    public static class AppSettings
    {
        public static string pathFile = System.Configuration.ConfigurationManager.AppSettings["pathFile"];
        public static string pathCE = System.Configuration.ConfigurationManager.AppSettings["pathCE"];
        public static string pathCESVF = System.Configuration.ConfigurationManager.AppSettings["pathCESVF"];
        public static string pathTCSVF = System.Configuration.ConfigurationManager.AppSettings["pathTCSVF"];
        public static string pathCertificados = System.Configuration.ConfigurationManager.AppSettings["pathCertificados"];
        public static string pathCompanyLogo = System.Configuration.ConfigurationManager.AppSettings["pathCompanyLogo"];
    }
    
    public static class PDF
    {
        public static string ObtenerRutaPDFGenerado(DocumentoElectronico documento)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Plantillas\\";
            string pathHTMLTemp = path + "HTMLTemp" + documento.Emisor.NroDocumento + documento.IdDocumento + ".html";

            string pathHTMLPlantilla = path + ((Formato.A4 == documento.formato) ? "A4.html" : "TICKET.html");
            
            string sHtml = GetStringOfFile(pathHTMLPlantilla);
            string resultHtml = "";

            resultHtml = RazorEngine.Razor.Parse(sHtml, documento);

            File.WriteAllText(pathHTMLTemp, resultHtml);

            string pathWKHTMLTOPDF = AppDomain.CurrentDomain.BaseDirectory + "\\wkhtmltopdf\\wkhtmltopdf.exe";

            string pdfPath = AppSettings.pathCE + $"{documento.Emisor.NroDocumento}\\PDF\\";
            if (!Directory.Exists(AppSettings.pathFile + pdfPath))
            {
                Directory.CreateDirectory(AppSettings.pathFile + pdfPath);
            }
            string savePDFPath = pdfPath + $"{documento.IdDocumento}.pdf";

            ProcessStartInfo oProcessStartInfo = new ProcessStartInfo();
            oProcessStartInfo.UseShellExecute = false;
            oProcessStartInfo.FileName = pathWKHTMLTOPDF;

            if (Formato.A4 == documento.formato)
            {
                oProcessStartInfo.Arguments = $"{pathHTMLTemp}" + " " + $"{AppSettings.pathFile + savePDFPath}";
            } else
            {
                oProcessStartInfo.Arguments = $"-T 0 -B 0 --margin-left 0 --margin-right 0 --page-width 80mm --page-height {160 + (documento.Items.Count * 15)}mm" + " " + $"{pathHTMLTemp}" + " " + $"{AppSettings.pathFile + savePDFPath}";
            }

            using (Process oProcess = Process.Start(oProcessStartInfo))
            {
                oProcess.WaitForExit();
            }

            File.Delete(pathHTMLTemp);

            return savePDFPath;
        }

        public static string ObtenerRutaPDFGeneradoSinValorFiscal(DocumentoElectronico documento, string folderPath)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Plantillas\\";
            string pathHTMLTemp = path + "HTMLTemp" + $"{documento.CompanyId}" + documento.IdDocumento + ".html";

            string pathHTMLPlantilla = path + "TICKET_SIN_VALOR_FISCAL.html";

            string sHtml = GetStringOfFile(pathHTMLPlantilla);
            string resultHtml = "";

            resultHtml = RazorEngine.Razor.Parse(sHtml, documento);

            File.WriteAllText(pathHTMLTemp, resultHtml);

            string pathWKHTMLTOPDF = AppDomain.CurrentDomain.BaseDirectory + "\\wkhtmltopdf\\wkhtmltopdf.exe";

            string pdfPath = folderPath + $"{documento.CompanyId}\\";
            if (!Directory.Exists(AppSettings.pathFile + pdfPath))
            {
                Directory.CreateDirectory(AppSettings.pathFile + pdfPath);
            }
            string savePDFPath = pdfPath + $"{documento.IdDocumento}.pdf";

            ProcessStartInfo oProcessStartInfo = new ProcessStartInfo();
            oProcessStartInfo.UseShellExecute = false;
            oProcessStartInfo.FileName = pathWKHTMLTOPDF;
            
            oProcessStartInfo.Arguments = $"-T 0 -B 0 --margin-left 0 --margin-right 0 --page-width 80mm --page-height {80 + (documento.Items.Count * 15)}mm" + " " + $"{pathHTMLTemp}" + " " + $"{AppSettings.pathFile + savePDFPath}";

            using (Process oProcess = Process.Start(oProcessStartInfo))
            {
                oProcess.WaitForExit();
            }

            File.Delete(pathHTMLTemp);

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
}