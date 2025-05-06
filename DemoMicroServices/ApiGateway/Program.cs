using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("Ocelot.json");

            builder.Services.AddOcelot();

            var app = builder.Build();

            app.UseOcelot();

            app.Run();

        }
    }
}
