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
    public partial class StatsWindow
    {
        public string SpentForDiscounts { get; set; }
        public Trip MostExpensiveTrip { get; set; }
        public Trip MostPopularTrip { get; set; }

        public StatsWindow()
        {
            var pricesSums = new Dictionary<Client, decimal>();
            using (var context = new TravelAgencyDbContext())
            {
                var clients = context.Clients
                    .Include(c => c.ClientTrips)
                    .ThenInclude(ct => ct.Trip);

                foreach (var client in clients)
                {
                    if (client.ClientTrips != null && client.ClientTrips.Any())
                    {
                        pricesSums[client] = client.ClientTrips.Sum(ct => ct.Trip.Price);
                    }
                }

                var tripsSales = context.Sales.ToDictionary(s => s.Trip, s => s.SalesCount);
                SpentForDiscounts = $"{pricesSums.Sum(kvp => kvp.Key.Discount * kvp.Value):C}";

                MostExpensiveTrip =
                    context.Trips.ToList().Aggregate((trip1, trip2) => trip1.Price > trip2.Price ? trip1 : trip2);

                var mostPopularTripPair = tripsSales.Aggregate((kvp1, kvp2) => kvp1.Value > kvp2.Value ? kvp1 : kvp2);
                MostPopularTrip = mostPopularTripPair.Key;
            }

            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}