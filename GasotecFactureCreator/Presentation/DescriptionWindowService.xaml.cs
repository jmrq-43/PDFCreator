using System.Windows;
using System.Windows.Controls;
using GasotecFactureCreator.Controller.Service;

namespace GasotecFactureCreator.Presentation;

public partial class DescriptionWindowService : Window
{
    private List<RowService> _services = new List<RowService>();

    public DescriptionWindowService()
    {
        InitializeComponent();
    }

    private void MenuItemSubmenu_Click(object sender, RoutedEventArgs e)
    {
        MenuItem menuItem = (MenuItem)sender;
        MenuItemServicio.Header = menuItem.Header;
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
    }

    private void AddRow_Click(object sender, RoutedEventArgs e)
    {
        AddRow();
    }

    private void MenuItemSubmenu_Click(object sender, RoutedEventArgs e, MenuItem menuItemServicio)
    {
        MenuItem menuItem = (MenuItem)sender;
        menuItemServicio.Header = menuItem.Header;
    }
}