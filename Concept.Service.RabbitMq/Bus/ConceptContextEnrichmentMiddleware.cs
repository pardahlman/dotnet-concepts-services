using System.Threading;
using System.Threading.Tasks;
using RawRabbit.Operations.Publish;
using RawRabbit.Pipe;
using RawRabbit.Pipe.Middleware;
using Serilog;

namespace Concept.Service.RabbitMq.Bus
{
  public class ConceptContextEnrichmentMiddleware : StagedMiddleware
  {
    private readonly ILogger _logger = Log.ForContext<ConceptContextEnrichmentMiddleware>();

    public override Task InvokeAsync(IPipeContext context, CancellationToken token = new CancellationToken())
    {
      if (!(context.GetMessageContext() is ConceptContext messageContext))
      {
        _logger.Information("Unable to extract message context from the pipe context.");
      }
      else
      {
        var basicProps = context.GetBasicProperties();
        messageContext.MessageId = basicProps.MessageId;
      }
      return Next.InvokeAsync(context, token);
    }

    public override string StageMarker => PublishStage.PreMessagePublish.ToString();
  }
}
