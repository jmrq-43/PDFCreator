using GasotecFactureCreator.Domain;

namespace GasotecFactureCreator.Controller;

public class ServiceController
{
    private static List<ServiceDomain> _services = new List<ServiceDomain>();

    public static void SetCurrentServices(List<ServiceDomain> services)
    {
        _services = services;
    }

    public static List<ServiceDomain> GetCurrentServices()
    {
        return _services;
    }
}