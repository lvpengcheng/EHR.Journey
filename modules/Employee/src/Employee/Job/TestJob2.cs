using EHR.Journey.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee
{
    public class TestJob2 : IBaseRecurringJob
    {
        public Task ExecuteAsync()
        {

            Console.WriteLine($"job2 测试");
            return Task.CompletedTask;
        }
    }
}
