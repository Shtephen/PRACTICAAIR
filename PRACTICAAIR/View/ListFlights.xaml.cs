using PRACTICAAIR.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Globalization;
using System.Windows.Threading;
using System.Windows.Media.Media3D;

namespace PRACTICAAIR
{
    /// <summary>
    /// Логика взаимодействия для ListFlights.xaml
    /// </summary>
    public partial class ListFlights : Window
    {
        public ListFlights()
        {
            InitializeComponent();

            List<FlightsModel> Flights = GetFlightsFromSqlServer();

            ListViewFlights.ItemsSource = Flights;


            StartClock();

            CheckUserRole();
        }



        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewFlights.SelectedItem != null)
            {
                FlightsModel selectedFlight = ListViewFlights.SelectedItem as FlightsModel;
                NewFlights newFlights = new NewFlights(selectedFlight);
                newFlights.ShowDialog();

                using (var context = new DBContextAir())
                {
                    context.Entry(selectedFlight).State = EntityState.Modified;
                    context.SaveChanges();
                }

                // Обновляете список рейсов на текущем окне
                List<FlightsModel> updatedFlights = GetFlightsFromSqlServer();
                ListViewFlights.ItemsSource = updatedFlights;
            }
            else
            {
                MessageBox.Show("Выберите рейс для редактирования");
            }
        }

        private void DelitButton_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewFlights.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверенны что хотите удалить рейс?", "Предупреждение об удалении", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {

                    FlightsModel selectedItem = ListViewFlights.SelectedItem as FlightsModel;

                    using (var context = new DBContextAir())
                    {
                        if (selectedItem != null)
                        {
                            var itemToRemove = context.Flights.FirstOrDefault(f => f.IdFlights == selectedItem.IdFlights);
                            if (itemToRemove != null)
                            {
                                context.Flights.Remove(itemToRemove);
                                context.SaveChanges();

                                List<FlightsModel> updatedFlights = GetFlightsFromSqlServer();
                                ListViewFlights.ItemsSource = updatedFlights;
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите рейс для удаления");
            }
        }

        public List<FlightsModel> GetFlightsFromSqlServer()
        {
            List<FlightsModel> Flights = new List<FlightsModel>();
            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-ANRB8JC;Initial Catalog=PRACTICAAIR;Integrated Security=True;"))
            {
                connection.Open();
                string query = "SELECT * FROM Flights";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    FlightsModel flights = new FlightsModel();
                    flights.IdFlights = Convert.ToInt32(reader["IdFlights"]);
                    flights.Carrier = reader["Carrier"].ToString();
                    flights.NamePlane = reader["NamePlane"].ToString();
                    flights.FlightStatus = reader["FlightStatus"].ToString();
                    flights.DepartureDate = Convert.ToDateTime(reader["DepartureDate"]);
                    flights.DepartureTime = TimeSpan.Parse(reader["DepartureTime"].ToString());
                    flights.DepartureCity = reader["DepartureCity"].ToString();
                    flights.DepartureAirport = reader["DepartureAirport"].ToString();
                    flights.IntermediateLanding = reader["IntermediateLanding"].ToString();
                    flights.ArrivalTime = TimeSpan.Parse(reader["ArrivalTime"].ToString());
                    flights.ArrivalCity = reader["ArrivalCity"].ToString();
                    flights.ArrivalAirport = reader["ArrivalAirport"].ToString();
                    flights.EconomyClassSeats = Convert.ToInt32(reader["EconomyClassSeats"]);
                    flights.ComfortClassSeats = Convert.ToInt32(reader["ComfortClassSeats"]);
                    flights.BusinessClassSeats = Convert.ToInt32(reader["BusinessClassSeats"]);
                    flights.EconomyPrice = Math.Round(Convert.ToDecimal(reader["EconomyPrice"]), 2);
                    flights.ComfortPrice = Math.Round(Convert.ToDecimal(reader["ComfortPrice"]), 2);
                    flights.BusinessPrice = Math.Round(Convert.ToDecimal(reader["BusinessPrice"]), 2);
                    Flights.Add(flights);
                }

            }
            return Flights;
        }

        private void ButtonNewList_Click(object sender, EventArgs e)
        {
            NewFlights newFlights = new NewFlights();
            newFlights.Closed += NewFlights_Close;
            newFlights.ShowDialog();
        }

        private void NewFlights_Close(object sender, EventArgs e)
        {
            List<FlightsModel> Flights = GetFlightsFromSqlServer();
            ListViewFlights.ItemsSource = Flights;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            string departureCity = textBoxDepartureCity.Text;
            string arrivalCity = textBoxArrivalCity.Text;
            DateTime? selectedDate = textBoxDepartureDate.SelectedDate;

            List<FlightsModel> flightsModels;

            if (string.IsNullOrEmpty(departureCity) && string.IsNullOrEmpty(arrivalCity) && selectedDate == null)
            {
                flightsModels = GetFlightsFromSqlServer();
            }
            else
            {
                DateTime departureDate = selectedDate ?? DateTime.Now;

                flightsModels = GetFlightsFromSqlServer()
                    .Where(f =>
                        (string.IsNullOrEmpty(departureCity) || f.DepartureCity.ToLower().Contains(departureCity.ToLower())) &&
                        (string.IsNullOrEmpty(arrivalCity) || f.ArrivalCity.ToLower().Contains(arrivalCity.ToLower())) &&
                        (selectedDate == null || f.DepartureDate.Date == departureDate.Date))
                    .ToList();
            }

            ListViewFlights.ItemsSource = flightsModels;
        }
        private DispatcherTimer timer;
        private void StartClock()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            clockText.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void CheckUserRole()
        {

            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            ComboBox comboBox = (ComboBox)mainWindow.FindName("ComboBoxRole");

            if (comboBox != null && comboBox.Text == "Администратор")
            {
                BuiTicketButton.Visibility = Visibility.Hidden;
            }
            else if (comboBox != null && comboBox.Text == "Клиент")
            {
                
                CreateFlights.Visibility = Visibility.Hidden;
                EditFlights.Visibility = Visibility.Hidden;
                DelFlights.Visibility = Visibility.Hidden;
                PriceTicket.Visibility = Visibility.Hidden;

            }
        }

        private void PriceTicketButton_Click(object sender, EventArgs e)
        {
            if (ListViewFlights.SelectedItems.Count > 0)
            {
                var selectedFlight = (FlightsModel)ListViewFlights.SelectedItems[0]; // Получаем выбранный рейс
                int flightId = selectedFlight.IdFlights; // Получаем Id выбранного рейса

                Ticket ticket = new Ticket(flightId); // Передаем Id выбранного рейса в конструктор Ticket
                ticket.ShowDialog();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите рейс из списка.");
            }
        }

        private void BuiTicketButton_Click(object sender, EventArgs e)
        {
            if (ListViewFlights.SelectedItems.Count > 0)
            {
                FlightsModel selectedFlights = ListViewFlights.SelectedItems[0] as FlightsModel;
                NewTicket ticket = new NewTicket(selectedFlights);
                ticket.Closed += (s, args) =>
                {
                    
                    List<FlightsModel> updatedFlights = GetFlightsFromSqlServer();
                    ListViewFlights.ItemsSource = updatedFlights;
                };
                ticket.Show();
            }
            else
            {
                MessageBox.Show("Выберите рейс!");
            }
        }
    }
}
