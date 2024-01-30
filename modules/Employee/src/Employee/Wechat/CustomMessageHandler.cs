using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageContexts;
using Senparc.Weixin.MP.MessageHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee
{
    public partial class CustomMessageHandler : MessageHandler<DefaultMpMessageContext>
    {
        public CustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0,
             bool onlyAllowEncryptMessage = false, IServiceProvider serviceProvider = null)
             : base(inputStream, postModel, maxRecordCount, onlyAllowEncryptMessage, null, serviceProvider)
        {
        }

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>(); //ResponseMessageText也可以是News等其他类型
            responseMessage.Content = "这条消息来自DefaultResponseMessage。";
            return responseMessage;
        }

        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            //CreateResponseMessage<类型>根据当前的RequestMessage创建指定类型的ResponseMessage；创建相应消息.
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "您的OpenID是：" + requestMessage.FromUserName + "。\r\t您发送了文字信息：" +
                                      requestMessage.Content;
            return responseMessage;
        }
    }
}
