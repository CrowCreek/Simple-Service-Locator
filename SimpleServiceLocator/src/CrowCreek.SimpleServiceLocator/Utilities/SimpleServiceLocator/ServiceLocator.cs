using System;
using System.Collections.Generic;

namespace CrowCreek.Utilities.SimpleServiceLocator
{
  public static class ServiceLocator
  {
    public static TService LocateService<TService>() where TService : class
    {
      var requestedType = typeof(TService);
      object rawService;
      TService service;
      if (_serviceOverrides != null && _serviceOverrides.TryGetValue(requestedType, out rawService))
      {
        service = rawService as TService;
        if (service == null)
        {
          throw new Exception("service was the wrong type");
        }
        return service;
      }
      if (Services.TryGetValue(requestedType, out rawService))
      {
        service = rawService as TService;
        if (service == null)
        {
          throw new Exception("service was the wrong type");
        }
        return service;
      }
      return null;
    }
    public static bool TryLocateService<TService>(out TService service) where TService : class
    {
      var requestedType = typeof(TService);
      object rawService;
      if (_serviceOverrides != null && _serviceOverrides.TryGetValue(requestedType, out rawService))
      {
        service = rawService as TService;
        return service != null;
      }
      if (Services.TryGetValue(requestedType, out rawService))
      {
        service = rawService as TService;
        return service != null;
      }
      service = null;
      return false;
    }

    private static readonly object ConfigurationLock = new object();

    private static readonly IDictionary<Type, object> Services = new Dictionary<Type, object>();

    private static IDictionary<Type, object> _serviceOverrides;

    public static void BuildFromConfiguration(IEnumerable<ServiceTypeConfiguration> config)
    {
      lock (ConfigurationLock)
      {
        foreach (var typeConfiguration in config)
        {
          Services.Add(typeConfiguration.GetServiceMap());
        }
      }
    }

    public static void AddService<TService>(TService concreteService)
    {
      if (concreteService == null) throw new ArgumentNullException(nameof(concreteService));
      lock (ConfigurationLock)
      {
        Services.Add(typeof(TService), concreteService);
      }
    }

    public static void AddServiceOverride<TService>(TService concreteService)
    {
      if (concreteService == null) throw new ArgumentNullException(nameof(concreteService));
      lock (ConfigurationLock)
      {
        if (_serviceOverrides == null)
        {
          _serviceOverrides = new Dictionary<Type, object>();
        }
        _serviceOverrides.Add(typeof(TService), concreteService);
      }
    }

    public static void ClearServiceOverrides()
    {
      lock (ConfigurationLock)
      {
        _serviceOverrides?.Clear();
        _serviceOverrides = null;
      }
    }
  }
}