using System;
using Microsoft.Extensions.DependencyInjection;

namespace Concept.Service.HostBuilder.Abstractions
{
  public interface IServiceHostBuilder
  {
    IServiceHostBuilder ConfigureServices(Action<IServiceCollection> configure);
    IServiceHost Build();
  }
}