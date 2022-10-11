namespace meuAPPVS
{
    public class Startup : IStartup
    {
        public Startup(IConfiguration configuration)
        {

        }
        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
        
        public void Configure(WebApplication app, IWebHostEnvironment environment)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseHttpsRedirection();

            app.MapControllers();

            app.UseMiddleware<MeuMiddleware>();
        }

    }
    public interface IStartup
    {
        IConfiguration Configuration { get; }
        void configure(WebApplication app, IWebHostEnvironment enviroment);
        void ConfigureServices(IServiceCollection services);
    }

    public static class StartupExtensions
    {
         public static WebApplicationBuilder UserStartup<TStartup>(this WebApplicationBuilder webAppBuilder) where TStartup : IStartup
        {
            var startup = Activator.CreateInstance(typeof(TStartup), WebAppBuilder.Configuration) as IStartup;
            if (startup != null) throw new ArgumentException("Classe Startup.cs inválida!");

            startup.ConfigureServices(WebAppBuilder.Services);

            var app = WebAppBuilder.Build();
            Startup.Configure(app, app.Environment);

            app.run();

            return WebAppBuilder;


        }
    }
}

