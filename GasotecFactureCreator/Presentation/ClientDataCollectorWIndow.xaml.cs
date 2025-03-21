﻿using System.Windows;
using GasotecFactureCreator.Controller;

namespace GasotecFactureCreator.Presentation;

public partial class ClientDataCollectorWIndow : Window
{
    public ClientDataCollectorWIndow()
    {
        InitializeComponent();
    }

    private void Back_Button_Click(Object sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        this.Close();
        mainWindow.Show();
    }

    private void Sig_Button_Click(Object sender, RoutedEventArgs e)
    {
        string name = TextBoxName.Text;
        string address = TextBoxAddres.Text;
        long phone = long.Parse(TextBoxPhone.Text);
        string mail = TextBoxMail.Text;
        string nit = TextBoxNit.Text;

        SalesCheckerController.CreateSalesChecker(name, address, phone, mail, nit, 0,0 ,0);

        DescriptionWindowService service = new DescriptionWindowService();
        this.Close();
        service.Show();
    }
}