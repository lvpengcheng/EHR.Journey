using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.Template
{
   public class QueryCompanyTemplateListResponse
    {
        public string total { get; set; }
        public string totalPageCount { get; set; }
        public string currentPageNo { get; set; }
        public List<Lists> list { get; set; }
        public class Lists
        {
            public string templateId { get; set; }
            public string templateName { get; set; }
            public string createTime { get; set; }
            public string templateTypeName { get; set; }
            public string creatorName { get; set; }
            public int templateStatus { get; set; }
        }
    }
}
