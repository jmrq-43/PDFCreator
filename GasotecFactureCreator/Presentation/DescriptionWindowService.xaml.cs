using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using GasotecFactureCreator.Controller;
using GasotecFactureCreator.Controller.Service;
using GasotecFactureCreator.Domain;
using iTextSharp.text.pdf;

namespace GasotecFactureCreator.Presentation;

public partial class DescriptionWindowService : Window
{
    private List<RowService> _services = new List<RowService>();
    private string _pdfSavePath;

    public DescriptionWindowService(string pdfSavePath)
    {
        InitializeComponent();
        _pdfSavePath = pdfSavePath;
        AddRow();
        LoadSalesChecker();
        TextBoxTotal.IsReadOnly = true;
        TextBoxSaldo.IsReadOnly = true;
    }

    public DescriptionWindowService()
    {
        InitializeComponent();
        _pdfSavePath = string.Empty;
        AddRow();
        LoadSalesChecker();
        TextBoxTotal.IsReadOnly = true;
        TextBoxSaldo.IsReadOnly = true;
    }

    private void AddRow()
    {
        RowService servicioRows = new RowService();
        _services.Add(servicioRows);

        Grid gridRow = new Grid();
        gridRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        gridRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        gridRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        gridRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        Grid.SetColumn(servicioRows.TextBoxAmount, 0);
        Grid.SetColumn(servicioRows.MenuServicio, 1);
        Grid.SetColumn(servicioRows.TextBoxDescription, 2);
        Grid.SetColumn(servicioRows.TextBoxValue, 3);

        gridRow.Children.Add(servicioRows.TextBoxAmount);
        gridRow.Children.Add(servicioRows.MenuServicio);
        gridRow.Children.Add(servicioRows.TextBoxDescription);
        gridRow.Children.Add(servicioRows.TextBoxValue);
        servicioRows.TextBoxValue.TextChanged += TextBoxValue_TextChanged;

        MainStackPanel.Children.Add(gridRow);
        UpdateTotal();
    }

    private void SaveService(object sender, RoutedEventArgs e)
    {
        List<ServiceDomain> services = new List<ServiceDomain>();

        foreach (var rowService in _services)
        {
            ServiceDomain service = new ServiceDomain
            {
                serviceType = rowService.MenuServicio.Items[0] is MenuItem menuItem && menuItem.Items.Count > 0
                    ? menuItem.Header.ToString()
                    : null,
                serviceDescription = rowService.TextBoxDescription.Text,
                servicePrice = decimal.TryParse(rowService.TextBoxValue.Text, out decimal servicePrice)
                    ? servicePrice
                    : 0,
                serviceAmount = int.TryParse(rowService.TextBoxAmount.Text, out int serviceAmount)
                    ? serviceAmount
                    : 0,
            };

            services.Add(service);
        }

        ServiceController.SetCurrentServices(services);

        SalesChecker salesChecker = SalesCheckerController.GetCurrentSalesChecker();

        salesChecker.Total = decimal.TryParse(TextBoxTotal.Text, out decimal t) ? t : 0;
        salesChecker.Payment = decimal.TryParse(TextBoxAbono.Text, out decimal a) ? a : 0;
        salesChecker.Balance = decimal.TryParse(TextBoxSaldo.Text, out decimal s) ? s : 0;

        Dictionary<string, Tuple<float, float>> coordenadas = new Dictionary<string, Tuple<float, float>>
        {
            { "NAME1", new Tuple<float, float>(144, 845) },
            { "NIT", new Tuple<float, float>(160.92f, 818.54f) },
            { "LOCATION", new Tuple<float, float>(149.69f, 791.72f) },
            { "PHONE", new Tuple<float, float>(134.90f, 768.34f) },
            { "EMAIL", new Tuple<float, float>(124.67f, 746f) },
            { "SERVICEAMOUNT", new Tuple<float, float>(40.40f, 493.87f) },
            { "SERVICE", new Tuple<float, float>(93.12f, 493.87f) },
            { "SERVICEDESCRIPTION", new Tuple<float, float>(210.83f, 493.87f) },
            { "SERVICEPRICE", new Tuple<float, float>(498.60f, 493.87f) },
            { "PAYMENT", new Tuple<float, float>(75.69f, 188.97f) },
            { "BALANCE", new Tuple<float, float>(259.60f, 188.97f) },
            { "TOTAL1", new Tuple<float, float>(463.82f, 154.64f) },
            { "TOTAL2", new Tuple<float, float>(130.82f, 589.64f) },
            { "DATE", new Tuple<float, float>(185f, 870f) },
            { "NAME2", new Tuple<float, float>(390.3f, 69.13f) }
        };

        if (string.IsNullOrEmpty(_pdfSavePath))
        {
            MessageBox.Show("No se ha seleccionado una ruta de guardado.", "Advertencia", MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        string nombreArchivo = $"Factura_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
        string rutaCompleta = Path.Combine(_pdfSavePath, nombreArchivo);

        PdfCreatorWriter pdfWriter = new PdfCreatorWriter();
        pdfWriter.OverwritePdf(_pdfSavePath, rutaCompleta, salesChecker, services, coordenadas);
        MessageBox.Show($"Se ha creado un nuevo PDF en: {rutaCompleta}");
    }

    private void AddRow_Click(object sender, RoutedEventArgs e)
    {
        AddRow();
    }

    private void DeleteRow_CLick(object sender, RoutedEventArgs e)
    {
        if (_services.Count > 1)
        {
            _services.RemoveAt(_services.Count - 1);
            MainStackPanel.Children.RemoveAt(MainStackPanel.Children.Count - 1);
            UpdateTotal();
        }
    }

    private void Back_Button_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ClientDataCollectorWIndow clientDataCollectorWIndow
                = new ClientDataCollectorWIndow(_pdfSavePath);
            this.Close();
            clientDataCollectorWIndow.Show();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    private void LoadSalesChecker()
    {
        SalesChecker salesChecker = SalesCheckerController.GetCurrentSalesChecker();
        TextBlockRecivido.Text = $"Recibido por: {salesChecker.Name} ";
    }

    private void UpdateTotal()
    {
        decimal total = 0;
        foreach (var rowService in _services)
        {
            if (decimal.TryParse(rowService.TextBoxValue.Text, out decimal servicePrice))
            {
                total += servicePrice;
            }
        }

        TextBoxTotal.Text = total.ToString();
        UpdateBalance();
    }

    private void UpdateBalance()
    {
        if (decimal.TryParse(TextBoxTotal.Text, out decimal total) &&
            decimal.TryParse(TextBoxAbono.Text, out decimal abono))
        {
            TextBoxSaldo.Text = (total - abono).ToString();
        }
        else if (decimal.TryParse(TextBoxTotal.Text, out total))
        {
            TextBoxSaldo.Text = total.ToString();
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