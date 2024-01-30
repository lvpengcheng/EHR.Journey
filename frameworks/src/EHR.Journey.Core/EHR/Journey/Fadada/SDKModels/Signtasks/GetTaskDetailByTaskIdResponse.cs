using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Signtasks
{
   public class GetTaskDetailByTaskIdResponse
    {
        public string taskId { get; set; }
        public string taskSubject { get; set; }
        public string taskStatus { get; set; }
        public List<TaskDetails> taskDetails { get; set; }
        public class TaskDetails
        {
            public string unionId { get; set; }
            public string mobile { get; set; }
            public string authorizedUnionId { get; set; }
            public string personName { get; set; }
            public string companyName { get; set; }
            public int signStatus { get; set; }
            public string locks { get; set; }
            public string resultDesc { get; set; }
            public int signOrder { get; set; }
        }
    }
}
