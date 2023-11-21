using Dto.Modelos;
using System;
using System.Diagnostics;
using System.IO;

namespace FacturacionApi.Utils
{
    public static class PDF
    {
        public static void GenerarPDF(DocumentoElectronico documento)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "/Plantillas/";
            string pathHTMLTemp = path + "HTMLTemp.html";

            string pathHTMLPlantilla = path + ((Formato.A4 == documento.formato) ? "A4.html" : "TICKET.html");
            
            string sHtml = GetStringOfFile(pathHTMLPlantilla);
            string resultHtml = "";

            resultHtml = RazorEngine.Razor.Parse(sHtml, documento);

            File.WriteAllText(pathHTMLTemp, resultHtml);

            string pathWKHTMLTOPDF = AppDomain.CurrentDomain.BaseDirectory + "/wkhtmltopdf/wkhtmltopdf.exe";
            string pathPDF = AppDomain.CurrentDomain.BaseDirectory + $"/ArchivosGenerados/PDFs/{documento.IdDocumento}.pdf";

            ProcessStartInfo oProcessStartInfo = new ProcessStartInfo();
            oProcessStartInfo.UseShellExecute = false;
            oProcessStartInfo.FileName = pathWKHTMLTOPDF;

            if (Formato.A4 == documento.formato)
            {
                oProcessStartInfo.Arguments = $"{pathHTMLTemp}" + " " + $"{pathPDF}";
            } else
            {
                oProcessStartInfo.Arguments = $"-T 0 -B 0 --margin-left 0 --margin-right 0 --page-width 80mm --page-height {160 + (documento.Items.Count * 15)}mm" + " " + $"{pathHTMLTemp}" + " " + $"{pathPDF}";
            }

            using (Process oProcess = Process.Start(oProcessStartInfo))
            {
                oProcess.WaitForExit();
            }

            File.Delete(pathHTMLTemp);
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
}