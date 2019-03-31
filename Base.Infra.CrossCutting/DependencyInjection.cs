using Base.Domain.Interfaces.Repositories;
using Base.Domain.Interfaces.Services;
using Base.Domain.Services;
using Base.Infra.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Base.Infra.CrossCutting
{
    public static class DependencyInjection
    {

        public static ServiceProvider Map(IConfiguration Configuration)
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddSingleton(_ => Configuration)

                .AddSingleton<ICityService, CityService>()
                .AddSingleton<IStateService, StateService>()


                .AddSingleton<ICityRepository, CityRepository>()
                .AddSingleton<IStateRepository, StateRepository>()

                .BuildServiceProvider();

            return serviceProvider;

        }

       
    }
}
