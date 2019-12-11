using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using TravelAgency.DAL;
using TravelAgency.Models;

namespace TravelAgency
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Employee _employee;

        public MainWindow(Employee employee)
        {
            _employee = employee;
            InitializeComponent();
            LoadData();
            LoginLabel.Content = $"Logged in as {employee.FirstName} {employee.MiddleName} {employee.LastName}";
            EmployeesTab.IsEnabled = employee.IsAdmin;
            AddTripMenuItem.IsEnabled = employee.IsAdmin;
        }

        private void LoadData()
        {
            using var context = new TravelAgencyDbContext();
            context.Trips.Load();
            context.Sales.Load();
            context.Clients.Load();
            context.Employees.Load();
            TripsGrid.ItemsSource = context.Trips.Local.ToObservableCollection();
            SalesGrid.ItemsSource = context.Sales.Local.ToObservableCollection();
            ClientsGrid.ItemsSource = context.Clients.Include(c => c.ClientTrips).ToList();
            EmployeesGrid.ItemsSource = context.Employees.Local.ToObservableCollection();
            CityFilterComboBox.ItemsSource = context.Trips.Local.Select(t => t.City).Distinct();
        }

        private void Grid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyDescriptor is PropertyDescriptor pd)
            {
                e.Column.Header = pd.DisplayName;
            }

            if (e.PropertyType == typeof(DateTime))
            {
                ((DataGridTextColumn)e.Column).Binding.StringFormat = "dd.MM.yyyy";
            }

            if (e.PropertyType == typeof(string[]) || e.PropertyType == typeof(ICollection<ClientTrip>))
            {
                ((DataGridTextColumn)e.Column).Visibility = Visibility.Hidden;
            }

            if (e.PropertyName == "Discount")
            {
                ((DataGridTextColumn)e.Column).Binding.StringFormat = "P";
            }

        }

        private void ClientsFilterCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            var cv = CollectionViewSource.GetDefaultView(ClientsGrid.ItemsSource);
            cv.Filter = c =>
            {
                var client = (Client)c;
                return client.ClientTrips != null && client.ClientTrips.Any() &&
                       client.ClientTrips.Any(ct => ct.Trip.City == (string)CityFilterComboBox.SelectedItem);
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
            using var context = new TravelAgencyDbContext();
            context.Clients.Add(client);
            context.SaveChanges();
        }

        private void AddTripMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var addTrip = new AddTripWindow();
            addTrip.ShowDialog();
            if (addTrip.DialogResult == null || !addTrip.DialogResult.Value) return;

            var trip = addTrip.Trip;
            using var context = new TravelAgencyDbContext();
            context.Trips.Add(trip);
            context.SaveChanges();
        }

        private async void SellTripMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var sellTrip = new SellTripWindow();
            sellTrip.ShowDialog();
            if (sellTrip.DialogResult == null || !sellTrip.DialogResult.Value) return;

            await using (var context = new TravelAgencyDbContext())
            {
                var trip = sellTrip.Trip;
                var client = sellTrip.Client;

                context.Trips.Attach(trip);
                context.Clients.Attach(client);

                var salesCount = context.Sales.Any(s => s.Trip.Id == trip.Id)
                    ? context.Sales.Single(s => s.Trip.Id == trip.Id).SalesCount + 1
                    : 1;

                Sale sale;

                if (context.Sales.Any(s => s.Trip.Id == trip.Id))
                {
                    sale = context.Sales.Single(s => s.Trip.Id == trip.Id);
                    sale.SalesCount = salesCount;
                    sale.TripsLeft = trip.AmountOfTrips - 1;
                    sale.LastSale = DateTime.Now;
                }
                else
                {
                    sale = new Sale
                    {
                        EmployeeFirstName = _employee.FirstName,
                        EmployeeMiddleName = _employee.MiddleName,
                        EmployeeLastName = _employee.LastName,
                        Trip = trip,
                        SalesCount = salesCount,
                        TripsLeft = trip.AmountOfTrips - 1,
                        LastSale = DateTime.Now
                    };
                    context.Sales.Add(sale);
                }


                trip.AmountOfTrips--;

                if (client.ClientTrips == null) client.ClientTrips = new List<ClientTrip>();
                client.ClientTrips.Add(new ClientTrip
                {
                    Client = client,
                    Trip = trip,
                });
                await context.SaveChangesAsync();
            }
            LoadData();
        }

        private async void Window_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                await using var context = new TravelAgencyDbContext();
                if ((TabItem)CollectionsTabs.SelectedItem == ClientsTab && ClientsGrid.SelectedItem != null)
                {
                    context.Remove(context.Clients.Single(c => c.Id == ((Client)ClientsGrid.SelectedItem).Id));
                }
                else if ((TabItem)CollectionsTabs.SelectedItem == TripsTab && TripsGrid.SelectedItem != null && _employee.IsAdmin)
                {
                    context.Remove(context.Trips.Single(t => t.Id == ((Trip) TripsGrid.SelectedItem).Id));
                }
                else  if ((TabItem)CollectionsTabs.SelectedItem == EmployeesTab && EmployeesGrid.SelectedItem != null)
                {
                    if (!_employee.IsAdmin)
                    {
                        MessageBox.Show("Current user is not an admin, cannot delete another employee.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (_employee.Id == ((Employee)EmployeesGrid.SelectedItem).Id)
                    {
                        MessageBox.Show("You can not delete yourself.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (((Employee) EmployeesGrid.SelectedItem).IsAdmin)
                    {
                        MessageBox.Show("This user is an admin too, you can not delete them.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    context.Remove(context.Employees.Single(s => s.Id == ((Employee) EmployeesGrid.SelectedItem).Id));
                }
                await context.SaveChangesAsync();
                LoadData();
            }
        }
    }
}