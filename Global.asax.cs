using FacturacionApi.Controllers;
using Hangfire;
using Hangfire.SqlServer;
using System;
using System.Collections.Generic;

namespace FacturacionApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private IEnumerable<IDisposable> GetHangfireServers()
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(System.Configuration.ConfigurationManager.ConnectionStrings["cnxHangfireBillingBD"].ConnectionString);

            yield return new BackgroundJobServer();
        }

        protected void Application_Start()
        {
            System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);

            HangfireAspNet.Use(GetHangfireServers);

            RecurringJob.AddOrUpdate<FacturacionController>("sendAllPendingXMLtoSUNAT", x => x.sendAllPendingXMLtoSUNAT(), "*/1 * * * *");
        }
    }
}
