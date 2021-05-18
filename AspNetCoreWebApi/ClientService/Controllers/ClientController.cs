using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using Models.Common;
using Models.DTO;
using System.Threading.Tasks;

namespace ClientService.Controllers
{
    /*
     * 这里假设有这样一个场景，客户通过浏览器提交了一个保单，这个保单中包含一些客户信息，
     * ClientService将这些信息处理后发送一个消息到RabbitMQ中，
     * NoticeService和ZAPEngineService订阅了这个消息。
     * NoticeService会将客户信息取出来并获取一些更多信息为客户发送Email，
     * 而ZAPEngineService则会根据客户的一些关键信息（比如：年龄，是否吸烟，学历，年收入等等）去数据库读取一些规则来生成一份Question List并存入数据库。
     */

    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IBus _bus;

        public ClientController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async Task<string> Post([FromBody] ClientDTO clientDto)
        {
            // Business Logic here...
            // eg.Add new client to your service databases via EF
            // Sample Publish
            ClientMessage message = new ClientMessage
            {
                ClientId = clientDto.Id.Value,
                ClientName = clientDto.Name,
                Sex = clientDto.Sex,
                Age = 29,
                SmokerCode = "N",
                Education = "Master",
                YearIncome = 100000
            };
            await _bus.PubSub.PublishAsync(message);

            return "Add Client Success! You will receive some letter later.";
        }
    }
}
