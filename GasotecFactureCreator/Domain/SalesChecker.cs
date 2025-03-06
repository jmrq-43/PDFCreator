﻿namespace GasotecFactureCreator.Domain;

public class SalesChecker
{
    public string Name{get;set;}
    public string Address{get;set;}
    public int PhoneNumber{get;set;}
    public String Email{get;set;}
    public string Nit{get;set;}

    public SalesChecker(string name, string address, int phoneNumber, string email, string nit)
    {
        Name = name;
        Address = address;
        PhoneNumber = phoneNumber;
        Email = email;
        Nit = nit;
    }
}