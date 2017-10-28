using Autofac;
using Concept.Service;
using Concept.Service.Opinionated.AspNetCore;
using Concept.Service.RabbitMq.Bus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Example.ServiceWithApi
{
  public class OpinionatedFooBootstrap : OpinionatedAspNetCoreBootstrap<FooService>
  {
    public override ServiceMetadata CreateMetadata()
    {
      return new ServiceMetadata
      {
        Name = nameof(FooService),
        Type = typeof(FooService)
      };
    }

    public override void ConfigureServices(IServiceCollection collection)
    {
      collection.AddMvc();
    }

    public override void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      app.UseMvc();
    }

    protected override void RegisterDependencies(ContainerBuilder builder)
    {
      builder
        .RegisterType<FooService>()
        .As<FooService>();
      builder
        .RegisterModule<RawRabbitModule>();
    }
  }
}
