using EHR.Journey.Core;
using Microsoft.Extensions.Configuration;
using Senparc.CO2NET.RegisterServices;
using Senparc.CO2NET;
using Senparc.Weixin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AutoMapper;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp;
using Senparc.Weixin;
using Microsoft.Extensions.DependencyInjection;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.MessageHandlers.Middleware;
using Senparc.Weixin.RegisterServices;
using Volo.Abp.Domain.Entities.Caching;
using Hangfire;
using Volo.Abp.BackgroundJobs;
using Hangfire.Redis.StackExchange;


namespace Employee
{
    public class EmployeeModule:BaseModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var config = context.GetConfiguration();
            SenparcSetting senparcSetting = new SenparcSetting();
            SenparcWeixinSetting senparcWeixinSetting = new SenparcWeixinSetting();
            senparcSetting.IsDebug = bool.Parse(config["SenparcSetting:IsDebug"]);
            senparcSetting.Cache_Memcached_Configuration = config["SenparcSetting:Cache_Memcached_Configuration"];
            senparcSetting.Cache_Redis_Configuration = config["SenparcSetting:Cache_Redis_Configuration"];
            senparcSetting.DefaultCacheNamespace = config["SenparcSetting:DefaultCacheNamespace"];
            senparcSetting.SenparcUnionAgentKey = config["SenparcSetting:SenparcUnionAgentKey"];
            //公众号设置
            senparcWeixinSetting.Token = config["SenparcWeixinSetting:Token"];
            senparcWeixinSetting.EncodingAESKey = config["SenparcWeixinSetting:EncodingAESKey"];
            senparcWeixinSetting.WeixinAppId = config["SenparcWeixinSetting:WeixinAppId"];
            senparcWeixinSetting.WeixinAppSecret = config["SenparcWeixinSetting:WeixinAppSecret"];


            IRegisterService register = RegisterService.Start(senparcSetting)
                                                .UseSenparcGlobal(false, null);

            //开始注册微信信息，必须！
            register.UseSenparcWeixin(senparcWeixinSetting);
            register.RegisterMpAccount(senparcWeixinSetting, "CtalentDemo");
            //可以对微信的对话进行劫持
            app.UseMessageHandlerForMp("/WeixinAsync",
            (stream, postModel, maxRecordCount, serviceProvider)
              => new CustomMessageHandler(stream, postModel, maxRecordCount, false, serviceProvider),
             options
            =>
             {
                 options.AccountSettingFunc = context => Senparc.Weixin.Config.SenparcWeixinSetting;
             });
            base.OnApplicationInitialization(context);
        }

        public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
            // 应用程序初始化的时候注册hangfire
            context.CreateRecurringJob2();
            base.OnPostApplicationInitialization(context);
        }


        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = BuildConfiguration();
            context.Services.AddSenparcGlobalServices(configuration).AddSenparcWeixinServices(configuration);
            context.Services.AddAssemblyOf<EmployeeModule>();
            ConfigureHangfire(context);
            Configure<AbpAutoMapperOptions>(options =>
            {
                //Add all mappings defined in the assembly of the MyModule class
                options.AddMaps<EmployeeModule>();
            });
            context.Services.AddAbpDbContext<EmployeeDbcontext>(options =>
            {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            //context.Services.AddSingleton<ISqlExecuter<TestDbContext>>();
            context.Services.AddEntityCache<Employee, Guid>();
            Configure<AbpDbContextOptions>(options =>
            {
                /* The main point to change your DBMS.
                 * See also CompanyNameProjectNameMigrationsDbContextFactory for EF Core tooling. */
                options.UseMySQL();
                //options.UseSqlServer();
            });


            var connectionString = configuration.GetConnectionString("Onboarding");
            var freeSql = new FreeSql.FreeSqlBuilder()
                .UseConnectionString(FreeSql.DataType.MySql, connectionString)
                .Build();

            context.Services.AddSingleton<IFreeSql>(freeSql);

            context.Services.AddSingleton<SqlExecuter<EmployeeDbcontext>>();

            ////Register a factory method that resolves from IServiceProvider
            context.Services.AddScoped<ISqlExecuter<EmployeeDbcontext>>(
                sp => sp.GetRequiredService<SqlExecuter<EmployeeDbcontext>>()
            );
            //Configure<AbpAutoMapperOptions>(options =>
            //{
            //    //Add all mappings defined in the assembly of the MyModule class
            //    options.AddMaps<TestModule>();
            //    options.AddProfile<CustomDtoMapper>(validate: true);
            //});
            //context.Services.AddAutoMapperObjectMapper<TestModule>();

            //Configure<AbpAutoMapperOptions>(options =>
            //{
            //    options.AddMaps<TestModule>(validate: true);
            //});
            //context.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
            context.Services.AddControllers().AddNewtonsoftJson();
            //context.Services.AddControllers().AddNewtonsoftJson().AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.IgnoreNullValues = true;
            //    options.JsonSerializerOptions.WriteIndented = true;
            //});

            ConfigureConventionalControllers();

        }




        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }

        private void ConfigureHangfire(ServiceConfigurationContext context)
        {
            var redisStorageOptions = new RedisStorageOptions()
            {
                Db = context.Services.GetConfiguration().GetValue<int>("Hangfire:Redis:DB")
            };

            Configure<AbpBackgroundJobOptions>(options => { options.IsJobExecutionEnabled = true; });

            context.Services.AddHangfire(config =>
            {
                config.UseRedisStorage(
                        context.Services.GetConfiguration().GetValue<string>("Hangfire:Redis:Host"), redisStorageOptions)
                    .WithJobExpirationTimeout(TimeSpan.FromDays(7));
                var delaysInSeconds = new[] { 10, 60, 60 * 3 }; // 重试时间间隔
                const int Attempts = 3; // 重试次数
                config.UseFilter(new AutomaticRetryAttribute() { Attempts = Attempts, DelaysInSeconds = delaysInSeconds });
            });
            context.Services.AddHangfireServer();
        }

        private void ConfigureConventionalControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(EmployeeModule).Assembly, setting =>
                {
                    setting.UseV3UrlStyle = true;
                    //setting.UrlControllerNameNormalizer = context => context.ControllerName?.Pluralize().Kebaberize();
                    //setting.UrlActionNameNormalizer = context => context.ActionNameInUrl?.Dasherize();
                });
            });
        }
    }
}
