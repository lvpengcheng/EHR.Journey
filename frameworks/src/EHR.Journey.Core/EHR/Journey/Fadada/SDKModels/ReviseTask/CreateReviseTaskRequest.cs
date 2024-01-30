using FDD.OpenAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.ReviseTask
{
    /// <summary>
    /// 创建模板定稿任务
    /// </summary>
    [RemoteService("/reviseTask/createReviseTask", "POST")]
    public class CreateReviseTaskRequest : BaseReqeust<CreateReviseTaskResponse>
    {
        public string templateId { get; set; }
        public string taskSubject { get; set; }
        public int finalizeWay { get; set; }
        public int taskStatus { get; set; }
        public int sort { get; set; }
        public List<FillRoles> fillRoles { get; set; }
        public List<TemplateFiles> templateFiles { get; set; }
        public class FillRoles
        {
            public string roleName { get; set; }
            public Notice notice { get; set; }
            public Externaler externaler { get; set; }
            public List<FillTemplateFiles> fillTemplateFiles { get; set; }
        }
        public class Notice
        {
            public int notifyWay { get; set; }
            public string notifyAddress { get; set; }
        }
        public class Externaler
        {
            public string mobile { get; set; }
            public string name { get; set; }
        }
        public class FillTemplateFiles
        {
            public string fileId { get; set; }
            public string formFields { get; set; }
        }
        public class TemplateFiles
        {
            public string fileId { get; set; }
            public string fileName { get; set; }
        }
    }
}
