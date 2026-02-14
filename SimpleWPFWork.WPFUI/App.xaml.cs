using Microsoft.Extensions.DependencyInjection;
using SimpleWPFWork.WPFUI.Services;
using System;
using System.Net.Http;
using System.Windows;

namespace SimpleWPFWork.WPFUI
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var baseUrl = "https://localhost:7213";

            // HttpClient Factory
            services.AddHttpClient("TodoApi", client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            });

            // API Client - Factory pattern ile
            services.AddTransient<IClient>(serviceProvider =>
            {
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("TodoApi");
                return new Client(baseUrl, httpClient);
            });

            // ViewModels
            services.AddTransient<MainViewModel>();

            // Windows
            services.AddTransient<MainWindow>();
        }
    }
}