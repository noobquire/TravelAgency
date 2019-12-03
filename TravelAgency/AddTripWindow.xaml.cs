using System.Windows;
using System.Linq;
using TravelAgency.Models;

namespace TravelAgency
{
    public partial class AddTripWindow : Window
    {
        public Trip Trip { get; private set; }

        public AddTripWindow()
        {
            InitializeComponent();
        }

        private void CreateButton_OnClick(object sender, RoutedEventArgs e)
        {
            var name = NameBox.Text;
            var city = CityBox.Text;
            var start = StartDatePicker.SelectedDate ?? default;
            var end = EndDatePicker.SelectedDate ?? default;
            var services = ServicesListBox.Items?.Cast<string>().ToArray();
            var price = PriceUpDown.Value ?? 0;
            var amount = AmountUpDown.Value ?? 0;
            
            if (name == "" || city == "" || start == default || end == default || !services.Any() || price == 0 || amount == 0)
            {
                MessageBox.Show("Fill in all the fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Trip = new Trip {Name = name, City = city, Start = start, End = end, Services = services, Price = price, AmountOfTrips = amount};
            this.DialogResult = true;
        }

        private void AddServiceButton_OnClick(object sender, RoutedEventArgs e)
        {
            var service = AddServiceBox.Text;

            if (service == "")
            {
                MessageBox.Show("Fill in service name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ServicesListBox.Items.Add(service);
            AddServiceBox.Text = "";
        }
    }
}