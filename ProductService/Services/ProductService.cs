using ProductService.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ProductService.Services
{
    public class ProductService
    {
        public async Task NotifyCartServiceAsync(Product product)
        {
            var factory = new ConnectionFactory() { HostName="localhost",Port=0};
            // Create an asynchronous connection
            using var connection = await factory.CreateConnectionAsync();
            // Create a channel asynchronously
            using var channel = await connection.CreateChannelAsync();
            // Declare the queue
            await channel.QueueDeclareAsync(queue:"ProductUpdates",durable:false,
                exclusive:false,autoDelete:false,arguments:null);

            // Serialize the product message
            var message = JsonSerializer.Serialize(product);
            var body = Encoding.UTF8.GetBytes(message);

            // Create basic properties for the message
            

            // Publish the messaage
            await channel.BasicPublishAsync(exchange:"",routingKey:"ProductUpdates",
            mandatory:false, basicProperties:new BasicProperties
            {
                ContentType="application/json"
            },body:body);
            Console.WriteLine($" [x] Sent {message}");
        }
    }
}
