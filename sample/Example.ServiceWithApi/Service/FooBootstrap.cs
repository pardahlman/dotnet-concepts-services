using Concept.Service.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Example.ServiceWithApi
{
  public class FooBootstrap : AspNetCoreBootstrap<FooService>
  {
    public override void ConfigureLogging(ILoggingBuilder builder)
    {
      builder.AddConsole();
    }

    public override void ConfigureServices(IServiceCollection services)
    {
      services
        .AddSingleton<FooService>()
        .AddLogging()
        .AddMvc();
    }

    public override void ConfigureAppConfiguration(IConfigurationBuilder configuration)
    {
      configuration.AddJsonFile("appsettings.json");
    }

    public override void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      app.UseMvc();
    }
  }
}