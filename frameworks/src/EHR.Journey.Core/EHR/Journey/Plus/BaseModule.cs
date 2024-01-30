using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Senparc.Weixin.RegisterServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Dapper;
using Volo.Abp.Domain;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Json;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Validation;

namespace EHR.Journey.Core
{
    [DependsOn(typeof(AbpAutofacModule))]
    [DependsOn(typeof(AbpJsonModule))]
    [DependsOn(typeof(AbpValidationModule))]
    [DependsOn(typeof(AbpDddDomainModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    [DependsOn(typeof(AbpObjectMappingModule))]
    [DependsOn(typeof(AbpDapperModule))]
    public class BaseModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            Configure<AbpDbContextOptions>(options =>
            {
                /* The main point to change your DBMS.
                 * See also CompanyNameProjectNameMigrationsDbContextFactory for EF Core tooling. */
                options.UseMySQL();
                //options.UseSqlServer();
            });

            Configure<AbpExceptionHandlingOptions>(options =>
            {

#if DEBUG
                options.SendExceptionsDetailsToClients = true;
                options.SendStackTraceToClients = true;
#else
             options.SendExceptionsDetailsToClients = false;
                options.SendStackTraceToClients = false;
#endif

            });

        }

       


    }
}
