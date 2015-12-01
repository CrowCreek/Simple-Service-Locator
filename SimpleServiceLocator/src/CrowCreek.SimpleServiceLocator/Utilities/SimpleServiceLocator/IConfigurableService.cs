namespace CrowCreek.Utilities.SimpleServiceLocator
{
  public interface IConfigurableService
  {
    void Configure(ConcreteServiceConfiguration configuration);
  }
}
