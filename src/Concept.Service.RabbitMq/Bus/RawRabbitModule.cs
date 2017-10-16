using System.Reflection;
using Autofac;
using RawRabbit.Configuration;
using RawRabbit.DependencyInjection.Autofac;
using RawRabbit.Enrichers.MessageContext;
using RawRabbit.Instantiation;
using Module = Autofac.Module;

namespace Concept.Service.RabbitMq.Bus
{
  public class RawRabbitModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder
        .RegisterType<ConceptContextEnrichmentMiddleware>()
        .AsSelf();
      builder
        .RegisterRawRabbit(new RawRabbitOptions
        {
          ClientConfiguration = RawRabbitConfiguration.Local,
          Plugins = p => p
            .UseMessageContext(context => new ConceptContext {Origin = Assembly.GetExecutingAssembly().GetName().Name})
            .Register(pipe => pipe.Use<ConceptContextEnrichmentMiddleware>())
        });
    }
  }
}
