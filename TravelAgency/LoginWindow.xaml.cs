using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using TravelAgency.DAL;

namespace TravelAgency
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
           
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameBox.Text;
            var password = PasswordBox.Password;
            await using var context = new TravelAgencyDbContext();
            try
            {
                var employees = context.Employees;
                var employee = await employees.SingleAsync(emp => emp.Username == username);
                if (employee.CheckPassword(password))
                {
                    var mainWindow = new MainWindow(employee);
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (InvalidOperationException)
            {
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
