using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDD.OpenAPI.SDKModels.ReviseTask
{
   public class ReviseTaskDetailResponse
    {
        public string taskId { get; set; }
        public string templateId { get; set; }
        public string draftId { get; set; }
        public string taskSubject { get; set; }
        public int finalizeWay { get; set; }
        public int taskStatus { get; set; }
        public int usageStatus { get; set; }
        public int storageMode { get; set; }
        public int sort { get; set; }
        public string expireTime { get; set; }
        public List<FillRoles> fillRoles { get; set; }
        public List<SignRoles> signRoles { get; set; }
        public List<ReviseTaskFiles> reviseTaskFiles { get; set; }
        public class FillRoles
        {
            public string roleName { get; set; }
            public int  status { get; set; }
            public int fillSort { get; set; }
            public string unionId { get; set; }
            public Externaler externaler { get; set; }
            public class Externaler
            {
                public string mobile { get; set; }
                public string name { get; set; }
            }
        }
        public class ReviseTaskFiles
        {
            public string fileId { get; set; }
            public string fileName { get; set; }
            public string fileUuid { get; set; }
            public int fileType { get; set; }
            public List<RoleWidgets> roleWidgets { get; set; }
            public class RoleWidgets
            {
                public string roleName { get; set; }
                public List<Widgets> widgets { get; set; }
                public class Widgets
                {
                    public string widgetName { get; set; }
                    public string widgetValue { get; set; }
                    public int type { get; set; }
                    public int isRequired { get; set; }
                }
            }
        }
        public class SignRoles
        {
            public string roleName { get; set; }
            public int roleType { get; set; }
            public int signSort { get; set; }
        }
    }
}
