using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace GasotecFactureCreator.Presentation;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private const string SavePathFile = "pdf_save_path.txt";
    private string _pdfSavePath = string.Empty;

    public MainWindow()
    {
        InitializeComponent();
        LoadSavePath();
    }

    private void LoadSavePath()
    {
        if (File.Exists(SavePathFile))
        {
            try
            {
                _pdfSavePath = File.ReadAllText(SavePathFile).Trim();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al leer la ruta guardada: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _pdfSavePath = string.Empty;
            }
        }
        else
        {
            _pdfSavePath = string.Empty;
        }
    }

    private void SaveSavePath()
    {
        try
        {
            File.WriteAllText(SavePathFile, _pdfSavePath);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al guardar la ruta: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Button_Go_Data_Collector(object sender, RoutedEventArgs e)
    {
        ClientDataCollectorWIndow wind = new ClientDataCollectorWIndow(_pdfSavePath);
        this.Close();
        wind.Show();
    }

    private void Select_Paht_Button(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Title = "Seleccionar Carpeta de Guardado";
        openFileDialog.FileName = "Seleccionar Carpeta";
        openFileDialog.Filter = "Carpetas|*.";
        openFileDialog.CheckFileExists = false;
        openFileDialog.CheckPathExists = true;
        openFileDialog.DereferenceLinks = true;

        if (openFileDialog.ShowDialog() == true)
        {
            _pdfSavePath = Path.GetDirectoryName(openFileDialog.FileName);
            SaveSavePath();
        }
    }
}