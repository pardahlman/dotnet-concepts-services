using System;

namespace Concept.Service
{
  public class ServiceMetadata
  {
    public Type Type { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Version { get; set; }
    public string Commit { get; set; }
  }
}