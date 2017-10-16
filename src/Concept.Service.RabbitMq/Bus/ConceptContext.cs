using System;

namespace Concept.Service.RabbitMq.Bus
{
  public class ConceptContext
  {
    public string Origin { get; set; }
    public Guid GlobalExecutionId { get; set; }
    public string MessageId { get; set; }
  }
}
