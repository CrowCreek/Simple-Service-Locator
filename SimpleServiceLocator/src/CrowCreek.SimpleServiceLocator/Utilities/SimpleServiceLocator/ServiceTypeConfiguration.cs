using System;
using System.Collections.Generic;
using System.Reflection;

namespace CrowCreek.Utilities.SimpleServiceLocator
{
  public class ServiceTypeConfiguration
  {
    public string ServiceTypeAssembly { get; set; }

    public string ServiceTypeName { get; set; }

    public ConcreteServiceConfiguration ConcreteServiceConfiguration { get; set; }

    public KeyValuePair<Type, object> GetServiceMap()
    {
      var serviceAssembly = Assembly.Load(new AssemblyName("ServiceTypeAssembly"));
      if (serviceAssembly == null)
      {
        throw new Exception();
      }
      var serviceType = serviceAssembly.GetType("ServiceTypeName", true, true);
      if (serviceType == null)
      {
        throw new Exception();
      }
      if (!serviceType.GetTypeInfo().IsInterface)
      {
        throw new Exception();
      }
      var service = ConcreteServiceConfiguration.BuildService(serviceType);
      return new KeyValuePair<Type, object>(serviceType, service);
    }
  }
}
