using System;
using MassTransietReciever1;
using MassTransit;

namespace MassTransietReciever1
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var myHost = cfg.Host(new Uri("rabbitmq://localhost:15672"), host =>
                {
                    host.Username("guest");
                    host.Password("guest");
                });

                cfg.ReceiveEndpoint(myHost, "hello", ep =>
                {
                    ep.Handler<YourMessage>(context =>
                    {
                        return Console.Out.WriteLineAsync($"Received: {context.Message.Text}");
                    });
                });
            });

            bus.Start();

            bus.Publish(new YourMessage { Text = "Hi" });

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            bus.Stop();

            Console.WriteLine("Hello World!");
        }
    }
    public class YourMessage
    {
        public string Text { get; set; }
    }
}
