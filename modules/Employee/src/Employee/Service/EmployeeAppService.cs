using EHR.Journey.Core;
using FDD.OpenAPI.SDKModels.Accounts;
using FDD.OpenAPI;
using Newtonsoft.Json;
using Senparc.Weixin.MP.AdvancedAPIs.GroupMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP.AdvancedAPIs;

namespace Employee.Service
{
    public class EmployeeAppService : BaseService<Employee, EmployeeDbcontext, EmployeeInput>
    {
        private readonly IEntityCache<Employee, Guid> _empCache;
        private readonly ICurrentUser _currentUser;
        public EmployeeAppService(IRepository<Employee, Guid> repository, IEntityCache<Employee, Guid> empCache, ISqlExecuter<EmployeeDbcontext> db, ICurrentUser currentUser) : base(repository, db)
        {
            _empCache = empCache;
            _currentUser = currentUser;
        }

        public async Task<string> TestWechat()
        {
            //var templateData = new MsgTemplateQueue()
            //{
            //    first = new TemplateDataItem($"{first}", "#CC0033"),
            //    keyword1 = new TemplateDataItem(name, "#99CC00"),
            //    keyword2 = new TemplateDataItem(cardnum, "#99CC00"),
            //    keyword3 = new TemplateDataItem(money, "#99CC00"),
            //    keyword4 = new TemplateDataItem(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "#99CC00"),
            //    remark = new TemplateDataItem("充值金额成功", "#0099CC")
            //};
            List<Article> articleList = new List<Article>();
            articleList.Add(new Article()
            {
                Title = "测试更多标题12",
                Description = "测试更多描述",
                Url = "https://www.baidu.com",
                PicUrl = "https://sdk.weixin.senparc.com/Images/qrcode.jpg"
            });

            var news = articleList.Select(z => new NewsModel()
            {
                title = z.Title,
                content = "点击【阅读原文】访问",//内容暂时无法获取到
                digest = z.Description,
                content_source_url = z.Url,
                thumb_url = z.PicUrl,
                thumb_media_id=Guid.NewGuid().ToString(),
            }).ToArray();
            //上传临时素材
            //var newsResult = MediaApi.UploadTemporaryNews("wx58c8a3f9e8bd0abb", news: news);
            //var news1 = MediaApi.UploadForeverMedia("wx58c8a3f9e8bd0abb", "c:\\1704677980928.png", Senparc.Weixin.MP.UploadForeverMediaType.image, 1000);
            //await CustomApi.SendMpNewsAsync("wx58c8a3f9e8bd0abb", "oF7rtwrZQ51FOVJfTkVVdBN3CITc", "enW_xRywm7quleK8OdHr-wxtjl--sc-P-9dXHXUf53UJWwcUot9SwSJuLNt14Ugh", 10000, "");
            await CustomApi.SendTextAsync("wx58c8a3f9e8bd0abb", "oF7rtwrZQ51FOVJfTkVVdBN3CITc", "hello");
            await TemplateApi.SendTemplateMessageAsync("wx58c8a3f9e8bd0abb", "oF7rtwrZQ51FOVJfTkVVdBN3CITc", "UbZqcD8CG4krEHnZAcEx4jTJsMeECdqe3HsjiZks1aQ", "", "", null, 1000);
            return "aaa1112";
        }

        public string TestFadada()
        {
            var ServerUrl = "https://sandboxapi.fadada.com/api/v3";
            var AppId = "FA67694018";
            var AppKey = "UNPRNJ8M35RUBJCTVOTJSL2AXQRLGMZS";
            //var client = new OpenClient(ServerUrl, AppId, AppKey);
            var client = new EcologicalClient(ServerUrl, AppId, AppKey);

            var result3 = client.Execute(new GetCompanyUnionIdUrlRequest()
            {
                clientId = "6666666666666",
                company = new GetCompanyUnionIdUrlRequest.Company()
                {
                    companyName = "abccompany"
                },
                redirectUrl = "http://www.shouhu.com",
                applicant = new GetCompanyUnionIdUrlRequest.Applicant()
                {
                    applicantType = "1",
                    unionId = "28374"
                }
            });

            return JsonConvert.SerializeObject(result3);
        }
    }
}
