using System.Windows;

namespace GasotecFactureCreator.Presentation;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_Go_Data_Collector(object sender, RoutedEventArgs e)
    {
        ClientDataCollectorWIndow wind = new ClientDataCollectorWIndow();
        this.Close();
        wind.Show();
    }
}