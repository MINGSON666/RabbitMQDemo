using System;
using System.Collections.Generic;
using System.Text;

namespace DemandManagement.MessageContracts
{
   public class RabbitMqConsts
    {
        public const string RabbitMqUri = "rabbitmq://localhost/demand/";// 需要手动新增虚拟主机 demand
        public const string UserName = "admin";
        public const string Password = "admin";
        public const string RegisterDemandServiceQueue = "registerdemand.service";
        public const string NotificationServiceQueue = "notification.service";
        public const string ThirdPartyServiceQueue = "thirdparty.service";
    }
}
