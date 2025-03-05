using System.Windows;

namespace GasotecFactureCreator;

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
}