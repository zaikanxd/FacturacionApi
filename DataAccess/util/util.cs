namespace DataAccess.Util
{
    public static class GetNameStoreProcedure
    {
        public const string bi_ElectronicReceipt_Insert = "bi_ElectronicReceipt_Insert";
    }

    public static class AppSettings
    {
        public static string cnxBillingBD = System.Configuration.ConfigurationManager.AppSettings["cnxBillingBD"];
    }
}
