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
        public decimal? SpentForDiscounts { get; set; }
        public Trip MostExpensiveTrip { get; set; }
        public StatsWindow()
        {
            using (var context = new TravelAgencyDbContext())
            {

                var totalSpentPerClient = context.Clients?.ToDictionary(c => c, c => c.Trips?.Sum(t => t.Price) ?? 0);
                SpentForDiscounts = totalSpentPerClient?.Sum(kvp => kvp.Key.Discount * kvp.Value);

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
