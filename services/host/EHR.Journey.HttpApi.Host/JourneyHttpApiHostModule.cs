using Employee;

namespace EHR.Journey
{
    [DependsOn(
        typeof(JourneyHttpApiModule),
        typeof(JourneySharedHostingMicroserviceModule),
        typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
        typeof(JourneyEntityFrameworkCoreModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpAccountWebModule),
        typeof(JourneyApplicationModule),
        typeof(JourneyCapModule),
        typeof(JourneyCapEntityFrameworkCoreModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule),
        typeof(EmployeeModule),
        typeof(AbpCachingStackExchangeRedisModule)
        //typeof(AbpBackgroundJobsHangfireModule)
    )]
    public partial class JourneyHttpApiHostModule : AbpModule
    {
        public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
            // 应用程序初始化的时候注册hangfire
            //context.CreateRecurringJob();
            base.OnPostApplicationInitialization(context);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            ConfigureCache(context);
            ConfigureHangfire(context);
            ConfigureSwaggerServices(context);
            ConfigureJwtAuthentication(context, configuration);

            ConfigureMiniProfiler(context);
            ConfigureIdentity(context);
            ConfigureCap(context);
            ConfigureAuditLog(context);
            ConfigurationSignalR(context);
            ConfigurationMultiTenancy();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var configuration = context.GetConfiguration();
            app.UseJourneyRequestLocalization();
            app.UseCorrelationId();
            app.UseStaticFiles();
            if (configuration.GetValue("MiniProfiler:Enabled", false))
            {
                app.UseMiniProfiler();
            }

            app.UseRouting();
            app.UseCors(JourneyHttpApiHostConst.DefaultCorsPolicyName);
            app.UseAuthentication();

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }

            app.UseAuthorization();
            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/Journey/swagger.json", "Journey API");
                options.DocExpansion(DocExpansion.None);
                options.DefaultModelsExpandDepth(-1);
            });

            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseUnitOfWork();
            app.UseConfiguredEndpoints(endpoints => { endpoints.MapHealthChecks("/health"); });
            // app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            // {
            //     Authorization = new[] { new CustomHangfireAuthorizeFilter() },
            //     IgnoreAntiforgeryToken = true
            // });

            if (configuration.GetValue("Consul:Enabled", false))
            {
                app.UseConsul();
            }
        }
    }
}