using System;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using TravelAgency.DAL;

namespace TravelAgency
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private MainWindow _mainWindow;
        public LoginWindow()
        {

        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginButton.IsEnabled = false;
            var username = UsernameBox.Text;
            var password = PasswordBox.Password;
            await using var context = new TravelAgencyDbContext();
            try
            {
                var employees = context.Employees;
                var employee = await employees.SingleAsync(emp => emp.Username == username);
                if (employee.CheckPassword(password))
                {
                    _mainWindow = new MainWindow(employee);
                    _mainWindow.Show();
                    this.Close();
                }
                else
                {
                    LoginButton.IsEnabled = true;
                    MessageBox.Show("Invalid password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (InvalidOperationException)
            {
                LoginButton.IsEnabled = true;
                MessageBox.Show("Invalid username", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var regWindow = new RegisterWindow();
            regWindow.ShowDialog();
        }
    }
}
