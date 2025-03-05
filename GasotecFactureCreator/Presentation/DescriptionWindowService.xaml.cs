using System.Windows;
using System.Windows.Controls;

namespace GasotecFactureCreator;

public partial class DescriptionWindowService : Window
{
    public DescriptionWindowService()
    {
        InitializeComponent();
    }

    private void MenuItemSubmenu_Click(object sender, RoutedEventArgs e)
    {
        MenuItem menuItem = (MenuItem)sender;
        MenuItemServicio.Header = menuItem.Header;
    }

    private void AddRow_Click(object sender, RoutedEventArgs e)
    {
        Grid newRow = new Grid();
        newRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        newRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        newRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        Menu newMenu = new Menu();
        MenuItem newMenuItemServicio = new MenuItem { Header = "Servicio" };

        string menuItemServicioName = "MenuItemServicio_" + MainStackPanel.Children.Count;
        newMenuItemServicio.Name = menuItemServicioName;

        MenuItem mantenimiento = new MenuItem { Header = "Mantenimiento" };
        mantenimiento.Click +=
            (s, args) => MenuItemSubmenu_Click(s, args, newMenuItemServicio);
        MenuItem reparacion = new MenuItem { Header = "Reparacion" };
        reparacion.Click += (s, args) => MenuItemSubmenu_Click(s, args, newMenuItemServicio);
        MenuItem instalacion = new MenuItem { Header = "Instalacion" };
        instalacion.Click += (s, args) => MenuItemSubmenu_Click(s, args, newMenuItemServicio);
        MenuItem ventaRepuesto = new MenuItem { Header = "Venta repuesto" };
        ventaRepuesto.Click += (s, args) => MenuItemSubmenu_Click(s, args, newMenuItemServicio);
        MenuItem ventaEquipo = new MenuItem { Header = "Venta Equipo" };
        ventaEquipo.Click += (s, args) => MenuItemSubmenu_Click(s, args, newMenuItemServicio);

        newMenuItemServicio.Items.Add(mantenimiento);
        newMenuItemServicio.Items.Add(reparacion);
        newMenuItemServicio.Items.Add(instalacion);
        newMenuItemServicio.Items.Add(ventaRepuesto);
        newMenuItemServicio.Items.Add(ventaEquipo);
        newMenu.Items.Add(newMenuItemServicio);

        TextBox newTextBoxDescription = new TextBox();
        TextBox newTextBoxValue = new TextBox();

        Grid.SetColumn(newMenu, 0);
        Grid.SetColumn(newTextBoxDescription, 1);
        Grid.SetColumn(newTextBoxValue, 2);

        newRow.Children.Add(newMenu);
        newRow.Children.Add(newTextBoxDescription);
        newRow.Children.Add(newTextBoxValue);

        MainStackPanel.Children.Add(newRow);
    }

    private void MenuItemSubmenu_Click(object sender, RoutedEventArgs e, MenuItem menuItemServicio)
    {
        MenuItem menuItem = (MenuItem)sender;
        menuItemServicio.Header = menuItem.Header;
    }
}