using GasotecFactureCreator.Domain;

namespace GasotecFactureCreator.Controller;

public static class SalesCheckerController
{
    private static SalesChecker _CurrentSalesChecker;

    public static void CreateSalesChecker(string name, string address, long phoneNumber, string email,
        string Nit, decimal total, decimal payment, decimal balance)
    {
        _CurrentSalesChecker = new SalesChecker(name, address, phoneNumber, email, Nit)
        {
            Total = total,
            Payment = payment,
            Balance = balance
        };
    }

    public static SalesChecker GetCurrentSalesChecker()
    {
        return _CurrentSalesChecker;
    }
}