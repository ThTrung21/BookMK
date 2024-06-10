using BookMK.Models;
using BookMK.ViewModels;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //initiate serilog
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
            .WriteTo.ApplicationInsights(new TelemetryConfiguration
            {
                InstrumentationKey = "your_instrumentation_key"
            }, TelemetryConverter.Traces)
            .CreateLogger();

            try
            {
                Log.Information("Application Starting Up");

                // Your application startup logic here
                // For example, initializing MVVM and other services

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
                throw;
            }
         
        }
        protected override void OnExit(ExitEventArgs e)
        {
            Log.Information("Application Shutting Down");
            Log.CloseAndFlush();
            base.OnExit(e);
        }
    }
}
