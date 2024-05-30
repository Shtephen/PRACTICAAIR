using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using PRACTICAAIR.Model;

namespace PRACTICAAIR
{
    /// <summary>
    /// Логика взаимодействия для NewFlights.xaml
    /// </summary>
    public partial class NewFlights : Window
    {

        private FlightsModel flightToUpdate;
        public NewFlights(FlightsModel selectedFlight = null)
        {
            
            InitializeComponent();

            DepartureTime.TextChanged += TextBox_TextChanged;
            ArrivalTime.TextChanged += TextBox_TextChanged;

            flightToUpdate = selectedFlight ?? new FlightsModel();
            if (selectedFlight != null)
            {
                PopulateFields();
            }
        }

        private void PopulateFields()
        {
            Carrier.Text = flightToUpdate.Carrier;
            NamePlane.Text = flightToUpdate.NamePlane;
            FlightStatus.Text = flightToUpdate.FlightStatus;
            DepartureDate.Text = flightToUpdate.DepartureDate.ToString();
            DepartureTime.Text = flightToUpdate.DepartureTime.ToString();
            DepartureCity.Text = flightToUpdate.DepartureCity;
            DepartureAirport.Text = flightToUpdate.DepartureAirport;
            IntermediateLanding.Text = flightToUpdate.IntermediateLanding;
            ArrivalTime.Text = flightToUpdate.ArrivalTime.ToString();
            ArrivalCity.Text = flightToUpdate.ArrivalCity;
            ArrivalAirport.Text = flightToUpdate.ArrivalAirport;
            EconomyClassSeats.Text = flightToUpdate.EconomyClassSeats.ToString();
            ComfortClassSeats.Text = flightToUpdate.ComfortClassSeats.ToString();
            BusinessClassSeats.Text = flightToUpdate.BusinessClassSeats.ToString(); 
            EconomyPrice.Text = flightToUpdate.EconomyPrice.ToString();
            ComfortPrice.Text = flightToUpdate.ComfortPrice.ToString();
            BusinessPrice.Text = flightToUpdate.BusinessPrice.ToString();

        }

        private void SaveFlightButton_Click (object sender, RoutedEventArgs e)
        {
            
            {
                if (string.IsNullOrWhiteSpace(Carrier.Text) || string.IsNullOrWhiteSpace(NamePlane.Text) ||
                string.IsNullOrWhiteSpace(FlightStatus.Text) || string.IsNullOrWhiteSpace(DepartureDate.Text) ||
                string.IsNullOrWhiteSpace(DepartureTime.Text) || string.IsNullOrWhiteSpace(DepartureAirport.Text) ||
                string.IsNullOrWhiteSpace(ArrivalTime.Text) || string.IsNullOrWhiteSpace (ArrivalCity.Text) || 
                string.IsNullOrWhiteSpace(ArrivalAirport.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните обязательные поля *");
                    return; 
                }

                using (var context = new DBContextAir())
                {
                    flightToUpdate.Carrier = Carrier.Text;
                    flightToUpdate.NamePlane = NamePlane.Text;
                    flightToUpdate.FlightStatus = ((ComboBoxItem)FlightStatus.SelectedItem).Content.ToString();
                    flightToUpdate.DepartureDate = DepartureDate.SelectedDate?.Date ?? DateTime.MinValue;
                    flightToUpdate.DepartureTime = TimeSpan.Parse(DepartureTime.Text);
                    flightToUpdate.DepartureCity = DepartureCity.Text;
                    flightToUpdate.DepartureAirport = DepartureAirport.Text;
                    flightToUpdate.IntermediateLanding = IntermediateLanding.Text;
                    flightToUpdate.ArrivalTime = TimeSpan.Parse(ArrivalTime.Text);
                    flightToUpdate.ArrivalCity = ArrivalCity.Text;
                    flightToUpdate.ArrivalAirport = ArrivalAirport.Text;
                    flightToUpdate.EconomyClassSeats = int.TryParse(EconomyClassSeats.Text, out int seatsE) ? (int?)seatsE : null;
                    flightToUpdate.ComfortClassSeats = int.TryParse(ComfortClassSeats.Text, out int seatsC) ? (int?)seatsC : null;
                    flightToUpdate.BusinessClassSeats = int.TryParse(BusinessClassSeats.Text, out int seatsB) ? (int?)seatsB : null;
                    flightToUpdate.EconomyPrice = decimal.TryParse(EconomyPrice.Text, out decimal priceE) ? (decimal?)priceE : null;
                    flightToUpdate.ComfortPrice = decimal.TryParse(ComfortPrice.Text, out decimal priceC) ? (decimal?)priceC : null;
                    flightToUpdate.BusinessPrice = decimal.TryParse(BusinessPrice.Text, out decimal priceB) ? (decimal?)priceB : null;

                    if (flightToUpdate.IdFlights == 0)
                    {
                        context.Flights.Add(flightToUpdate);
                    }
                    else
                    {
                        context.Entry(flightToUpdate).State = EntityState.Modified;

                    }

                    context.SaveChanges();
                }
                Close();
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            
            textBox.TextChanged -= TextBox_TextChanged;

            
            string text = textBox.Text.Replace(":", "");

           
            if (text.Length > 6)
            {
                text = text.Substring(0, 6);
            }

           
            if (text.Length > 2)
            {
                text = text.Insert(2, ":");
            }
            if (text.Length > 5)
            {
                text = text.Insert(5, ":");
            }

            textBox.Text = text;

            
            textBox.CaretIndex = text.Length;
            textBox.TextChanged += TextBox_TextChanged;
        }

        private void SaveDontButton_Click (object sender, EventArgs e)
        {
            Close();
        }
    }
}
