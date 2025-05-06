using MassTransit;
using OrderService.Models;

namespace OrderService.Messaging
{
    public class OrderPublisher
    {
        private readonly IPublishEndpoint _endpoint;

        public OrderPublisher(IPublishEndpoint endpoint)
        {
            _endpoint = endpoint;   
        }

        public async Task PublishOrderAsync(Order order)
        {
            await _endpoint.Publish(order);
        }
    }
}
