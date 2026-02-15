using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using SimpleWPFWork.WPFUI.Services;
using System;
using System.IO;
using System.Net.Http;
using System.Windows;

namespace SimpleWPFWork.WPFUI
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            //  Serilog'u en başta configure et
            ConfigureSerilog();

            //  Global exception handler
            this.DispatcherUnhandledException += Application_DispatcherUnhandledException;
        }

        private void ConfigureSerilog()
        {
            //  Uygulamanın çalıştığı dizin
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var logPath = Path.Combine(appDirectory, "logs", "app-.txt");

            // Logs klasörünü oluştur
            var logFolder = Path.Combine(appDirectory, "logs");
            Directory.CreateDirectory(logFolder);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Debug(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    path: logPath,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}")
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "SimpleWPFWork.WPF")
                .Enrich.WithProperty("MachineName", Environment.MachineName)
                .CreateLogger();

            Log.Information("=== Application Starting ===");
            Log.Information("Log folder: {LogFolder}", logFolder);
            Console.WriteLine($"📁 Log files: {logFolder}");
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                Log.Information("Configuring services...");

                var services = new ServiceCollection();
                ConfigureServices(services);
                ServiceProvider = services.BuildServiceProvider();

                Log.Information("Services configured successfully");

                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();

                Log.Information("MainWindow displayed");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application failed to start");
                MessageBox.Show(
                    $"Application failed to start:\n\n{ex.Message}",
                    "Fatal Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Shutdown(1);
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var baseUrl = "https://localhost:7213";

            //  Serilog'u ILogger<T> ile kullanabilmek için
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(dispose: true);
            });

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

                var logger = serviceProvider.GetRequiredService<ILogger<Client>>();
                logger.LogInformation("API Client initialized with base URL: {BaseUrl}", baseUrl);

                return new Client(baseUrl, httpClient);
            });

            // ViewModels
            services.AddTransient<MainViewModel>();

            // Windows
            services.AddTransient<MainWindow>();

            Log.Information("All services registered");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Information("=== Application Shutting Down ===");

            //  ServiceProvider'ı dispose et
            if (ServiceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }

            //  Serilog'u flush et ve kapat
            Log.CloseAndFlush();

            base.OnExit(e);
        }

        private void Application_DispatcherUnhandledException(object sender,
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Error(e.Exception, "Unhandled exception occurred");

            MessageBox.Show(
                $"An unexpected error occurred:\n\n{e.Exception.Message}\n\nPlease check the log file for details.",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            e.Handled = true;
        }
    }
}