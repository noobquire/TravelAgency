using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using TravelAgency.DAL;
using TravelAgency.Models;

namespace TravelAgency
{
    public partial class SellTripWindow : Window
    {
        public Client Client { get; private set; }
        public Trip Trip { get; private set; }

        public SellTripWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using TravelAgencyDbContext context = new TravelAgencyDbContext();
            context.Clients.Load();
            context.Trips.Load();
            var clients = context.Clients.Local.ToObservableCollection();
            var trips = context.Trips.Local.Where(t => t.AmountOfTrips >= 1).ToArray();
            ClientsComboBox.ItemsSource = clients;
            TripsComboBox.ItemsSource = trips;
        }


        private void SellButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (TripsComboBox.SelectedItem == null || ClientsComboBox.SelectedItem == null)
            {
                MessageBox.Show("Choose client and trip to sell", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var client = ClientsComboBox.SelectedItem as Client;
            var trip = TripsComboBox.SelectedItem as Trip;
            if (client?.ClientTrips != null && client.ClientTrips.Any(ct => trip != null && ct.TripId == trip.Id))
            {
                MessageBox.Show("This client already bought this trip", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            Trip = trip;
            Client = client;
            DialogResult = true;
        }

        private void ClientsComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var client = e.AddedItems[0] as Client;
            if (client != null) DiscountLabel.Content = $"Discount: {client.Discount:P2}";
            var trip = TripsComboBox.SelectedItem as Trip;
            if (trip != null)
                TotalWithDiscountLabel.Content =
                    client == null
                        ? $"Total with discount: {trip.Price:C}"
                        : $"Total with discount: {trip.Price * (1 - client.Discount):C}";
        }

        private void TripsComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var trip = e.AddedItems[0] as Trip;
            if (trip != null) TotalLabel.Content = $"Total: {trip.Price:C}";
            var client = ClientsComboBox.SelectedItem as Client;
            if (trip != null)
                TotalWithDiscountLabel.Content =
                    client == null
                        ? $"Total with discount: {trip.Price:C}"
                        : $"Total with discount: {trip.Price * (1 - client.Discount):C}";
        }
    }
}