using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace TreeLine.Sagas.DependencyInjection
{
    public interface IConfiguration
    {
        IServiceCollection Configure(IServiceCollection services);
    }

    internal sealed class Configuration : IConfiguration
    {
        private readonly IList<Action<IServiceCollection>> _configurations = new List<Action<IServiceCollection>>();

        public IServiceCollection Configure(IServiceCollection services)
        {
            foreach (var configuration in _configurations)
            {
                configuration.Invoke(services);
            }

            return services;
        }

        internal void Add(Action<IServiceCollection> configuration)
        {
            _configurations.Add(configuration);
        }
    }
}
