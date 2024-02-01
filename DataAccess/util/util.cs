﻿namespace DataAccess.Util
{
    public static class GetNameStoreProcedure
    {
        public const string bi_ElectronicReceipt_Insert = "bi_ElectronicReceipt_Insert";
        public const string bi_ElectronicReceipt_Update = "bi_ElectronicReceipt_Update";
        public const string bi_ElectronicReceipt_GetAllPending = "bi_ElectronicReceipt_GetAllPending";
    }

    public static class AppSettings
    {
        public static string cnxBillingBD = System.Configuration.ConfigurationManager.ConnectionStrings["cnxBillingBD"].ConnectionString;
    }
}
