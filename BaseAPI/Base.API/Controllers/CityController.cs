using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Base.Infra.CrossCutting;
using Microsoft.Extensions.Configuration;
using System.IO;
using Base.Domain.Interfaces.Services;
using Base.Domain.Entities;

namespace Base.API.Controllers
{
    public class CityController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Program));

        [HttpPost]
        public void AddCity([FromBody]dynamic city)
        {
            try
            {
                string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings." + environment + ".json");
                var Configuration = builder.Build();

                var serviceDI = DependencyInjection.Map(Configuration);
                ICityService cityService = (ICityService)serviceDI.GetService(typeof(ICityService));

            }
            catch (Exception ex)
            {
                log.Error("Msg: " + ex.Message + " - Stack: " + ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost]
        public IEnumerable<Cities> AllCities([FromBody] Cities city)
        {
            try
            {
                string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings." + environment + ".json");
                var Configuration = builder.Build();

                var serviceDI = DependencyInjection.Map(Configuration);
                ICityService cityService = (ICityService)serviceDI.GetService(typeof(ICityService));


                var dataFile = cityService.GetAll();

                return dataFile;
            }
            catch (Exception ex)
            {
                log.Error("Msg: " + ex.Message + " - Stack: " + ex.StackTrace);
                throw ex;
            }

        }

    }

}
