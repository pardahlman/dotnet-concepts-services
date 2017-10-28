# RabbitMQ

[RabbitMQ](https://www.rabbitmq.com/) is a populare message broker for distributed systems. The package `Concept.Service.RabbitMq` contains `RabbitMqService` that has methods for subscribing and publishing messages. It uses [RawRabbit](https://github.com/pardahlman/RawRabbit) under the hood.

```csharp
public class FooService : RabbitMqService
{
  public FooService(IBusClient busClient) : base(busClient) { }

  public override async Task StartAsync(CancellationToken ct = default(CancellationToken))
  {
    // Method in base class
    await SubscribeAsync<PerformFoo>(HandleFooAsync, ct: ct);
  }

  private async Task HandleFooAsync(PerformFoo message, ConceptContext context)
  {
    /* Handle message */
    // Method in base class
    await PublishAsync(new FooPerformed {Success = true});
  }
}
```