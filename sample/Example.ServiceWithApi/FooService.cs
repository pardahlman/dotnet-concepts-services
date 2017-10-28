using System;
using System.Threading;
using System.Threading.Tasks;
using Concept.Service.RabbitMq;
using Concept.Service.RabbitMq.Bus;
using Microsoft.Extensions.Logging;
using RawRabbit;

namespace Example.ServiceWithApi
{
  public class FooService : RabbitMqService
  {
    private readonly ILogger<FooService> _logger;

    public FooService(ILogger<FooService> logger, IBusClient busClient) : base(busClient)
    {
      _logger = logger;
    }
    public override async Task StartAsync(CancellationToken ct = default(CancellationToken))
    {
      _logger.LogInformation("Started");
      await SubscribeAsync<PerformFoo>(HandlePerformFoo, ct: ct);
    }

    private static Task HandlePerformFoo(PerformFoo message, ConceptContext context)
    {
      throw new NotImplementedException();
    }
  }

  public class PerformFoo { }
}