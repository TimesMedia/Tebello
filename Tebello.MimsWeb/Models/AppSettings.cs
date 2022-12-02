using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.IO;
using Subs.Data;

namespace Tebello.MimsWeb.Models
{
    public class AppSettings
    {
        private static AppSettings _appSettings;
        public string apiEndpoint { get; set; }
        public string AdminEmail { get; set; }
        public string PayUReturnURL { get; set; }
        public string PayUSafeKey { get; set; }
        public string PayUUsername { get; set; }
        public string PayUPassword { get; set; }
        public string PayUUrl { get; set; }

        public AppSettings(IConfiguration config)
        {
            apiEndpoint = config.GetValue<string>("apiEndpoint");
            AdminEmail = config.GetValue<string>("AdminEmail");
            PayUReturnURL = config.GetValue<string>("PayUReturnURL");
            PayUSafeKey = config.GetValue<string>("PayUSafeKey");
            PayUUsername = config.GetValue<string>("PayUUsername");
            PayUPassword = config.GetValue<string>("PayUPassword");
            PayUUrl = config.GetValue<string>("PayUUrl");

            // Now set Current
            _appSettings = this;
        }

        public static AppSettings Current
        {
            get
            {
                if (_appSettings == null)
                {
                    _appSettings = GetCurrentSettings();
                }

                return _appSettings;
            }
        }

        public static AppSettings GetCurrentSettings()
        {
            try
            {

                var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                .AddEnvironmentVariables();

                IConfigurationRoot configuration = builder.Build();

                var settings = new AppSettings(configuration.GetSection("AppSettings"));
                return settings;
            }
            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, "AppSettings", "GetCurrentSettings", "");
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);
            }
            return new AppSettings(null);


        }
    }
}
