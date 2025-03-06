using GasotecFactureCreator.Domain;

namespace GasotecFactureCreator.Controller;

public static class SalesCheckerController
{
    public static SalesChecker CurrentSalesChecker { get; private set; }

    public static void CreateSalesChecker(string name, string address, int phoneNumber, string email, string Nit)
    {
        CurrentSalesChecker = new SalesChecker(name, address, phoneNumber, email, Nit);
    }

    public static SalesChecker GetCurrentSalesChecker()
    {
        return CurrentSalesChecker;
    }
}