using System.Windows;
using GasotecFactureCreator.Controller;
using GasotecFactureCreator.Domain;

namespace GasotecFactureCreator.Presentation;

public partial class ClientDataCollectorWIndow : Window
{
    private string _pdfSavePath;

    public ClientDataCollectorWIndow(string pdfSavePath)
    {
        InitializeComponent();
        DataContext = SalesCheckerController.GetCurrentSalesChecker();
        _pdfSavePath = pdfSavePath;
    }

    public ClientDataCollectorWIndow()
    {
        InitializeComponent();
        DataContext = SalesCheckerController.GetCurrentSalesChecker();
        _pdfSavePath = string.Empty;
    }

    private void ClientDataCollectorWIndow_Loaded(object sender, RoutedEventArgs e)
    {
        DataContext = SalesCheckerController.GetCurrentSalesChecker();
    }

    private void Back_Button_Click(object sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        this.Close();
        mainWindow.Show();
    }

    private void Sig_Button_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is SalesChecker currentChecker)
        {
            SalesCheckerController.CreateSalesChecker(
                currentChecker.Name,
                currentChecker.Address,
                currentChecker.PhoneNumber,
                currentChecker.Email,
                currentChecker.Nit,
                currentChecker.Total,
                currentChecker.Payment,
                currentChecker.Balance);
        }
        string name = TextBoxName.Text;
        string address = TextBoxAddres.Text;
        long phone = long.Parse(TextBoxPhone.Text);
        string mail = TextBoxMail.Text;
        string nit = TextBoxNit.Text;

        SalesCheckerController.CreateSalesChecker(name, address, phone, mail, nit, 0, 0, 0);

        DescriptionWindowService service = new DescriptionWindowService(_pdfSavePath);
        this.Close();
        service.Show();
    }

    private void GenerarPdfButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_pdfSavePath))
        {
            MessageBox.Show("No se ha seleccionado una ruta de guardado.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        Domain.SalesChecker salesCheckerData = Controller.SalesCheckerController.GetCurrentSalesChecker();
        System.Collections.Generic.List<Domain.ServiceDomain> serviceData = new System.Collections.Generic.List<Domain.ServiceDomain>();
        System.Collections.Generic.Dictionary<string, Tuple<float, float>> coordinatesData = new System.Collections.Generic.Dictionary<string, Tuple<float, float>>();

        string outputFileName = $"Factura_{salesCheckerData?.Name?.Replace(" ", "_") ?? "SinNombre"}_{System.DateTime.Now:yyyyMMdd_HHmmss}.pdf";

        Domain.PdfCreatorWriter pdfWriter = new Domain.PdfCreatorWriter();
        pdfWriter.OverwritePdf(_pdfSavePath, outputFileName, salesCheckerData, serviceData, coordinatesData);

        MessageBox.Show($"El PDF se ha guardado en: {System.IO.Path.Combine(_pdfSavePath, outputFileName)}", "PDF Generado", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}