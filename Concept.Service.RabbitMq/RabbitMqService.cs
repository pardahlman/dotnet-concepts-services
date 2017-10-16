using System;
using System.Threading;
using System.Threading.Tasks;
using Concept.Service.RabbitMq.Bus;
using EnsureThat;
using RawRabbit;
using RawRabbit.Operations.Publish.Context;
using RawRabbit.Operations.Subscribe.Context;
using Serilog;
using Serilog.Context;
using SerilogTimings;

namespace Concept.Service.RabbitMq
{
  public abstract class RabbitMqService : Service
  {
    private readonly IBusClient _busClient;
    private readonly ILogger _logger = Log.ForContext<RabbitMqService>();

    protected RabbitMqService(IBusClient busClient)
    {
      _busClient = busClient;
    } 

    protected async Task SubscribeAsync<TMessage>(Func<TMessage, ConceptContext, Task> handler, Action<ISubscribeContext> context = null,
      CancellationToken ct = default(CancellationToken))
    {
      Ensure.That(handler, nameof(handler)).IsNotNull();
      _logger.Information("Subscribing to message {MessageType} in handler {MessageHandler}", typeof(TMessage).Name, handler.Method?.Name ?? "Anonymous");
      await _busClient.SubscribeAsync<TMessage, ConceptContext>(async (msg, ctx) =>
      {
        using (LogContext.PushProperty(nameof(ctx.GlobalExecutionId), ctx.GlobalExecutionId))
        using (LogContext.PushProperty(nameof(ctx.MessageId), ctx.MessageId))
        using (Operation.Time("Consuming message {MessageType} with id {MessageId}", typeof(TMessage).Name, ctx.MessageId))
        {
          try
          {
            await handler(msg, ctx);
          }
          catch (Exception e)
          {
            _logger.Warning(e, "An unhandled exception occured when consuming message {MessageId} of type {MessageType}", typeof(TMessage).Name);
          }
        }
      }, context, ct);
    }

    protected async Task PublishAsync<TMessage>(TMessage message = default(TMessage), Action<IPublishContext> context = null,
      CancellationToken ct = default(CancellationToken))
    {
      _logger.Information("Publishing message of type {MessageType}", typeof(TMessage).Name);
      await _busClient.PublishAsync(message, context, ct);
    }
  }
}
