using System.Windows;
using System.Windows.Controls;

namespace GasotecFactureCreator.Controller.Service;

public class RowService
{
    public Menu MenuServicio { get; set; }
    public TextBox TextBoxDescription { get; set; }
    public TextBox TextBoxValue { get; set; }
    public MenuItem MenuItemServicio { get; set; }

    public RowService()
    {
        MenuServicio = new Menu();
        MenuItemServicio = new MenuItem { Header = "Servicio", FontSize = 20 };
        MenuServicio.Items.Add(MenuItemServicio);

        TextBoxDescription = new TextBox { Style = (Style)Application.Current.Resources["RoundedTextBoxStyle"] };
        TextBoxValue = new TextBox { Style = (Style)Application.Current.Resources["RoundedTextBoxStyle"] };

        MenuItem mantenimiento = new MenuItem { Header = "Mantenimiento" };
        mantenimiento.Click += (s, args) => MenuItemSubmenu_Click(s, args, MenuItemServicio);
        MenuItem reparacion = new MenuItem { Header = "Reparacion" };
        reparacion.Click += (s, args) => MenuItemSubmenu_Click(s, args, MenuItemServicio);
        MenuItem instalacion = new MenuItem { Header = "Instalacion" };
        instalacion.Click += (s, args) => MenuItemSubmenu_Click(s, args, MenuItemServicio);
        MenuItem ventaRepuesto = new MenuItem { Header = "Venta repuesto" };
        ventaRepuesto.Click += (s, args) => MenuItemSubmenu_Click(s, args, MenuItemServicio);
        MenuItem ventaEquipo = new MenuItem { Header = "Venta Equipo" };
        ventaEquipo.Click += (s, args) => MenuItemSubmenu_Click(s, args, MenuItemServicio);

        MenuItemServicio.Items.Add(mantenimiento);
        MenuItemServicio.Items.Add(reparacion);
        MenuItemServicio.Items.Add(instalacion);
        MenuItemServicio.Items.Add(ventaRepuesto);
        MenuItemServicio.Items.Add(ventaEquipo);
    }

    private void MenuItemSubmenu_Click(object sender, RoutedEventArgs e, MenuItem menuItemServicio)
    {
        MenuItem menuItem = (MenuItem)sender;
        menuItemServicio.Header = menuItem.Header;
    }
}