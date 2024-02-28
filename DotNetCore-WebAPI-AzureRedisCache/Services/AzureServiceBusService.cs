using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace DotNetCore_WebAPI_AzureRedisCache.Services
{
	public class AzureServiceBusService
	{
        private readonly string _connectionstring;
        private readonly string _queuename;
        public AzureServiceBusService(string connstring, string queuename)
        {
            _connectionstring = connstring;
            _queuename = queuename;                
        }
        public async Task SendProductToQueueAsync<T>(T messageobject)
        {
            IQueueClient queueClient = new QueueClient(_connectionstring, _queuename);
            string jsonmessage = JsonConvert.SerializeObject(messageobject);
            var message = new Message(Encoding.UTF8.GetBytes(jsonmessage));
            await queueClient.SendAsync(message);
            await queueClient.CloseAsync();
        }
    }
}
