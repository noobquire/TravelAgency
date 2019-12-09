using System.Windows;
using Microsoft.EntityFrameworkCore;
using TravelAgency.DAL;
using TravelAgency.Models;
using TravelAgency.Models.Utils;

namespace TravelAgency
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameBox.Text;
            var password = PasswordBox.Password;
            var passwordConfirmation = ConfirmPasswordBox.Password;
            if (password != passwordConfirmation)
            {
                MessageBox.Show("Passwords do not match", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var firstName = FirstNameBox.Text;
            var middleName = MiddleNameBox.Text;
            var lastName = LastNameBox.Text;

            if (firstName == "" || middleName == "" || lastName == "")
            {
                MessageBox.Show("Specify first, middle and last name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            var passwordHash = Hasher.CalculateHash(password, username);
            var employee = new Employee(username, passwordHash)
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
            };

            await using var context = new TravelAgencyDbContext();
            if (await context.Employees.AnyAsync(emp => emp.Username == username))
            {
                MessageBox.Show("User with such username already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            context.Employees.Add(employee);
            await context.SaveChangesAsync();
            
            DialogResult = true;
        }
    }
}
