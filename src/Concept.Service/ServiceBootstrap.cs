namespace Concept.Service
{
  public abstract class ServiceBootstrap<TService> where TService : Service
  {
    public abstract ServiceMetadata CreateMetadata();

    public virtual void PreConfigureLogger() { }
    public abstract void ConfigureLogger();
    public virtual void PostConfigureLogger() { }

    public virtual void PreRegisterDependencies() { }
    public abstract void RegisterDependencies();
    public virtual void PostRegisterDependencies() { }

    public abstract TService CreateService();
  }
}
