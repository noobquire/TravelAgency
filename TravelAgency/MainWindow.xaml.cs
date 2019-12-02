using System;
using System.CodeDom;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;
using System.Windows.Data;
using TravelAgency.DAL;
using TravelAgency.Models;

namespace TravelAgency
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly TravelAgencyDbContext _context;
        private readonly Employee _employee;

        public MainWindow(Employee employee)
        {
            _context = new TravelAgencyDbContext();
            _employee = employee;
            InitializeComponent();
            LoadData();
            LoginLabel.Content =  $"Logged in as {employee.FirstName} {employee.MiddleName} {employee.LastName}";
        }

        private void LoadData()
        {
            _context.Trips.Load();
            _context.Sales.Load();
            _context.Clients.Load();
            TripsGrid.ItemsSource = _context.Trips.Local.ToObservableCollection();
            SalesGrid.ItemsSource = _context.Sales.Local.ToObservableCollection();
            ClientsGrid.ItemsSource = _context.Clients.Local.ToObservableCollection();
            CityFilterComboBox.ItemsSource = _context.Trips.Local.Select(t => t.City).Distinct();
        }

        private void Grid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyDescriptor is PropertyDescriptor pd)
            {
                e.Column.Header = pd.DisplayName;
            }

            if (e.PropertyType == typeof(DateTime))
            {
                ((DataGridTextColumn) e.Column).Binding.StringFormat = "dd.MM.yyyy";
            }

            if (e.PropertyType == typeof(string[]))
            {
                ((DataGridTextColumn) e.Column).Visibility = Visibility.Hidden;
            }

            if (e.PropertyName == "Discount")
            {
                ((DataGridTextColumn) e.Column).Binding.StringFormat = "P";
            }
        }

        private void ClientsFilterCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            var cv = CollectionViewSource.GetDefaultView(ClientsGrid.ItemsSource);
            cv.Filter = c =>
            {
                var client = (Client) c;
                return client.Trips != null && client.Trips.Any() && client.Trips.Any(t => t.City == (string)CityFilterComboBox.SelectedItem);
            };
        }

        private void ClientsFilterCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            var cv = CollectionViewSource.GetDefaultView(ClientsGrid.ItemsSource);
            cv.Filter = null;
        }

        private void TripsFilterCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            var cv = CollectionViewSource.GetDefaultView(TripsGrid.ItemsSource);
            cv.Filter += TripsDateFilter;
        }

        private void TripsFilterCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            var cv = CollectionViewSource.GetDefaultView(TripsGrid.ItemsSource);
            cv.Filter -= TripsDateFilter;
        }

        private void LastMinuteCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            var cv = CollectionViewSource.GetDefaultView(TripsGrid.ItemsSource);
            cv.Filter += TripsLastMinuteFilter;
        }

        private void LastMinuteCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            var cv = CollectionViewSource.GetDefaultView(TripsGrid.ItemsSource);
            cv.Filter -= TripsLastMinuteFilter;
            
        }

        private bool TripsDateFilter(object t)
        {
            var trip = (Trip)t;
            return TripDatePicker.SelectedDate.HasValue &&
                   trip.Start.Date == TripDatePicker.SelectedDate.Value.Date;
        }

        private bool TripsLastMinuteFilter(object t)
        {
            var trip = (Trip)t;
            var timeToTrip = trip.Start.Date - DateTime.Today;

            return timeToTrip.Days <= 5 && trip.Start.Date > DateTime.Today;
        }

        private void StatsMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var stats = new StatsWindow();
            stats.ShowDialog();
        }

        private void AddClientMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var addClient = new AddClientWindow();
            addClient.ShowDialog();
            if (addClient.DialogResult == null || !addClient.DialogResult.Value) return;
            
            var client = addClient.Client;
            _context.Clients.Add(client);
            _context.SaveChanges();
        }

        private void AddTripMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var addTrip = new AddTripWindow();
            addTrip.ShowDialog();
            if (addTrip.DialogResult == null || !addTrip.DialogResult.Value) return;

            var trip = addTrip.Trip;
            _context.Trips.Add(trip);
            _context.SaveChanges();
        }
        
    }
}