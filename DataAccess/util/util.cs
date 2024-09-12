using System.Security.Cryptography;
using System.Text;

namespace DataAccess.Util
{
    public static class GetNameStoreProcedure
    {
        public const string bi_ElectronicReceipt_Insert = "bi_ElectronicReceipt_Insert";
        public const string bi_ElectronicReceipt_Update = "bi_ElectronicReceipt_Update";
        public const string bi_ElectronicReceipt_GetAllPending = "bi_ElectronicReceipt_GetAllPending";
        public const string bi_ElectronicReceipt_GetAllBy = "bi_ElectronicReceipt_GetAllBy";
        public const string bi_ElectronicReceipt_GetOne = "bi_ElectronicReceipt_GetOne";
        public const string bi_User_Login = "bi_User_Login";
        public const string bi_ElectronicReceiptReport_GetAllBy = "bi_ElectronicReceiptReport_GetAllBy";
        public const string bi_ElectronicReceipt_Cancel = "bi_ElectronicReceipt_Cancel";
    }

    public static class AppSettings
    {
        public static string cnxBillingBD = System.Configuration.ConfigurationManager.ConnectionStrings["cnxBillingBD"].ConnectionString;
    }

    public class Encrypt
    {
        public static string GetSHA256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
    }
}
