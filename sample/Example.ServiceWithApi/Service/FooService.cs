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
    public FooService(IBusClient busClient) : base(busClient) { }

    public override async Task StartAsync(CancellationToken ct = default(CancellationToken))
    {
      await SubscribeAsync<PerformFoo>(HandlePerformFoo, ct: ct);
    }

    private async Task HandlePerformFoo(PerformFoo message, ConceptContext context)
    {
      // handle message
      await PublishAsync(new FooPerformed {Success = true});
    }
  }

  public class PerformFoo { }

  public class FooPerformed
  {
    public bool Success { get; set; }
  }
}