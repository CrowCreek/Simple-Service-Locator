using System;
using System.Reflection;
using System.Linq;

namespace CrowCreek.Utilities.SimpleServiceLocator
{
  public class ConcreteServiceConfiguration
  {
    public string ConcreteServiceAssembly { get; set; }

    public string ConcreteServiceName { get; set; }

    public double TimeOutMilliseconds { get; set; }

    public int NextService { get; set; }

    public int Priority { get; set; }

    public object BuildService(Type serviceType)
    {
      var concreteAssembly = Assembly.Load(new AssemblyName(ConcreteServiceAssembly));
      if (concreteAssembly == null)
      {
        throw new Exception();
      }
      var concreteServiceType = concreteAssembly.GetType("ConcreteServiceName", true, true);
      if (concreteServiceType.GetTypeInfo().ImplementedInterfaces.All(implementedInterface => implementedInterface != serviceType))
      {
        throw new Exception();
      }
      var concreteServiceTypeCostructor = concreteServiceType.GetConstructor(new Type[] { });
      if (concreteServiceTypeCostructor == null)
      {
        throw new Exception();
      }
      var concreteService = concreteServiceTypeCostructor.Invoke(new object[] { });
      var configurableService = concreteService as IConfigurableService;
      if (configurableService == null)
      {
        throw new Exception();
      }
      configurableService.Configure(this);
      return concreteService;
    }
  }
}
