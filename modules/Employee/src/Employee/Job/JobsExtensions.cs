
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Employee
{
    public static class JobsExtensions
    {
        public static void CreateRecurringJob2(this ApplicationInitializationContext context)
        {
            RecurringJob.AddOrUpdate<TestJob2>("测试Job2", e => e.ExecuteAsync(), CronType.Minute(1), new RecurringJobOptions()
            {
                TimeZone = TimeZoneInfo.Local
            });
        }
    }
}
