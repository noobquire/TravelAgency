using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TravelAgency.Models;

namespace TravelAgency
{
    /// <summary>
    /// Interaction logic for AddClientWindow.xaml
    /// </summary>
    public partial class AddClientWindow : Window
    {
        public Client Client { get; private set; }
        public AddClientWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var firstName = FirstNameBox.Text;
            var middleName = MiddleNameBox.Text;
            var lastName = LastNameBox.Text;

            var phone = PhoneBox.Value as string;
            var passport = PassportBox.Value as string;
            var discount = DiscountBox.Value ?? 0;

            if (firstName == "" || middleName == "" || lastName == "" || phone == null || passport == null || phone == "" || passport == "")
            {
                MessageBox.Show("Fill in all the fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            Client = new Client()
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                PhoneNumber = phone,
                PassportNumber = passport,
                Discount = discount
            };

            this.DialogResult = true;
        }

        private void IdCardRadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            PassportBox.Text = "";
            PassportBox.Mask = "000000000";
        }

        private void ConventionalPassRadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            PassportBox.Text = "";
            PassportBox.Mask = "LL 000000";
        }
    }
}
