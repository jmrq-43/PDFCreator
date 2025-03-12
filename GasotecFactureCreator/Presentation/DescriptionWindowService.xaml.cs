using System.Windows;
using System.Windows.Controls;
using GasotecFactureCreator.Controller;
using GasotecFactureCreator.Controller.Service;
using GasotecFactureCreator.Domain;

namespace GasotecFactureCreator.Presentation;

public partial class DescriptionWindowService : Window
{
    private List<RowService> _services = new List<RowService>();

    public DescriptionWindowService()
    {
        InitializeComponent();
        AddRow();
        LoadSalesChecker();
    }

    private void AddRow()
    {
        RowService servicioRows = new RowService();
        _services.Add(servicioRows);

        Grid gridRow = new Grid();
        gridRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        gridRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        gridRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        Grid.SetColumn(servicioRows.MenuServicio, 0);
        Grid.SetColumn(servicioRows.TextBoxDescription, 1);
        Grid.SetColumn(servicioRows.TextBoxValue, 2);

        gridRow.Children.Add(servicioRows.MenuServicio);
        gridRow.Children.Add(servicioRows.TextBoxDescription);
        gridRow.Children.Add(servicioRows.TextBoxValue);

        MainStackPanel.Children.Add(gridRow);
        UpdateTotal();
    }

    private void SaveService(object sender, RoutedEventArgs e)
    {
        List<Service> services = new List<Service>();

        foreach (var rowService in _services)
        {
            Service servicio = new Service
            {
                serviceType = rowService.MenuServicio.Items[0].ToString(),
                serviceDescription = rowService.TextBoxDescription.Text,
                servicePrice = decimal.TryParse(rowService.TextBoxValue.Text, out decimal servicePrice)
                    ? servicePrice
                    : 0,
            };

            services.Add(servicio);
        }

        MessageBox.Show("Se ha creado un nuevo PDF");
    }

    private void AddRow_Click(object sender, RoutedEventArgs e)
    {
        AddRow();
    }

    private void DeleteRow_CLick(Object sender, RoutedEventArgs e)
    {
        if (_services.Count > 1)
        {
            _services.RemoveAt(_services.Count - 1);

            MainStackPanel.Children.RemoveAt(MainStackPanel.Children.Count - 1);
            UpdateTotal();
        }
    }

    private void LoadSalesChecker()
    {
        SalesChecker salesChecker = SalesCheckerController.GetCurrentSalesChecker();

        TextBlockRecivido.Text = $"Recivido por:   {salesChecker.Name} ";
    }
}

    private void TextBoxValue_TextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateTotal();
    }

    private void TextBoxAbono_TextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateBalance();
    }
}