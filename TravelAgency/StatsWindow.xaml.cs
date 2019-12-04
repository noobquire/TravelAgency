using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using TravelAgency.DAL;
using TravelAgency.Models;

namespace TravelAgency
{
    /// <summary>
    /// Interaction logic for StatsWindow.xaml
    /// </summary>
    public partial class StatsWindow : Window
    {
        public string SpentForDiscounts { get; set; }
        public Trip MostExpensiveTrip { get; set; }

        public StatsWindow()
        {
            using (var context = new TravelAgencyDbContext())
            {
                var clients = context.Clients
                    .Include(c => c.ClientTrips)
                    .ThenInclude(ct => ct.Trip);
                var pricesSums = new Dictionary<Client, decimal>();
                foreach (var client in clients)
                {
                    if (client.ClientTrips != null && client.ClientTrips.Any())
                    {
                        pricesSums[client] = client.ClientTrips.Sum(ct => ct.Trip.Price);
                    }
                }
                SpentForDiscounts = $"{pricesSums.Sum(kvp => kvp.Key.Discount * kvp.Value):C}";

                MostExpensiveTrip =
                    context.Trips.ToList().Aggregate((trip1, trip2) => trip1.Price > trip2.Price ? trip1 : trip2);
            }

            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}