using System.IO;
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

        Grid.SetColumn(servicioRows.MenuServicio, 0);
        Grid.SetColumn(servicioRows.TextBoxDescription, 1);
        Grid.SetColumn(servicioRows.TextBoxValue, 2);

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
            serviceType = rowService.MenuServicio.Items[0] is MenuItem menuItem && menuItem.Items.Count > 0 ? menuItem.Header.ToString() : null,
            serviceDescription = rowService.TextBoxDescription.Text,
            servicePrice = decimal.TryParse(rowService.TextBoxValue.Text, out decimal servicePrice)
                ? servicePrice
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
        { "NOMBRE", new Tuple<float, float>(80, 498) },
        { "LOCATION", new Tuple<float, float>(80, 469) },
        { "PHONE", new Tuple<float, float>(80, 455) },
        { "EMAIL", new Tuple<float, float>(65, 440) },
        { "NIT", new Tuple<float, float>(82, 483) },
        { "SERVICE", new Tuple<float, float>(10, 289) },
        { "SERVICEDESCRIPTION", new Tuple<float, float>(100, 289) },
        { "SERVICEPRICE", new Tuple<float, float>(300, 289) },
        { "TOTAL", new Tuple<float, float>(290, 90) },
        { "PAYMENT", new Tuple<float, float>(50, 110) },
        { "BALANCE", new Tuple<float, float>(150, 110) }
    };

    string carpetaSalida = "PDFs";

    if (!Directory.Exists(carpetaSalida))
    {
        Directory.CreateDirectory(carpetaSalida);
    }

    string nombreArchivo = $"Factura_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

    string rutaCompleta = Path.Combine(carpetaSalida, nombreArchivo);

    PdfCreatorWriter pdfWriter = new PdfCreatorWriter();
    pdfWriter.OverwritePdf(rutaCompleta, salesChecker, services, coordenadas);

    MessageBox.Show("Se ha creado un nuevo PDF");
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