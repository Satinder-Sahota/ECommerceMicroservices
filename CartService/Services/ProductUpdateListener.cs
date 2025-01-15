using CartService.Data;
using ProductService.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CartService.Services
{
    public class ProductUpdateListener
    {
        private readonly IServiceProvider _serviceProvider;
        public ProductUpdateListener(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task StartListeningAsync()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };

            // Establish connection and channel
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            // Declare the queue
            await channel.QueueDeclareAsync(queue:"ProductUpdates",durable:false,
                exclusive:false,autoDelete:false,arguments:null);

            // Set up the consumer
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += HandleMessageAsync;

            // Start consuming
            await channel.BasicConsumeAsync(queue: "ProductUpdates",autoAck:true,consumer:consumer);
            //Console.WriteLine("Listening for Product updates...");
            //Console.ReadLine();
        }

        private async Task HandleMessageAsync(object sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var product = JsonSerializer.Deserialize<Product>(message);
            Console.WriteLine($"Received update for Product ID: {product?.Id}");

            // Use a scoped service for database updates
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CartDbContext>();
            // Update cart items with the new product price
            var cartItems = context.CartItems.Where(c => c.ProductId == product.Id).ToList();
            foreach (var item in cartItems)
                item.Price = product.Price; //Update Price
            await context.SaveChangesAsync();


        }
    }
}
