using System;
using Microsoft.Extensions.DependencyInjection;

namespace Concept.Service.HostBuilder
{
  public interface IServiceHostBuilder
  {
    IServiceHostBuilder ConfigureServices(Action<IServiceCollection> configure);
    IServiceHost Build();
  }
}