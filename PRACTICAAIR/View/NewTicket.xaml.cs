using PRACTICAAIR.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PRACTICAAIR
{
    /// <summary>
    /// Логика взаимодействия для NewTicket.xaml
    /// </summary>
    public partial class NewTicket : Window
    {
        private FlightsModel _selectedFlight;
        private TicketsModel ticketsModel;
        private PassengersModel passengersModel;
        public NewTicket(FlightsModel selectedFlight)
        {
            InitializeComponent();
            _selectedFlight = selectedFlight;
            ClassService.SelectionChanged += TicketStatus_SelectionChanged;
            ticketsModel = new TicketsModel();
            passengersModel = new PassengersModel();
            ticketsModel.IdFlights = selectedFlight.IdFlights;
        }
        private void DontBuiButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void BuiTicketButton_Click(object sender, RoutedEventArgs e)
        {
            if (SeatNumber.Text == "" || ClassService.SelectedItem == null || TicketStatus.SelectedItem == null || Surname.Text == "" || Name.Text == "" || DateOfBirth.Text == "" || Telephone.Text == "" || PassportDate.Text == "" || Email.Text == "")
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля *");
                return;
            }

            using (var context = new DBContextAir())
            {
                ticketsModel.SeatNumber = SeatNumber.Text;
                ticketsModel.ClassService = ((ComboBoxItem)ClassService.SelectedItem).Content.ToString();
                ticketsModel.TicketStatus = ((ComboBoxItem)TicketStatus.SelectedItem).Content.ToString();
                UpdateTicketPrice();
                ticketsModel.IdFlights = _selectedFlight.IdFlights;

                DecreaseAvailableSeats();

                context.Tickets.Add(ticketsModel);
                context.SaveChanges();

                int generatedId = ticketsModel.IdTickets;

                passengersModel.Gender = ((ComboBoxItem)Gender.SelectedItem).Content.ToString();
                passengersModel.Surname = Surname.Text;
                passengersModel.Name = Name.Text;
                passengersModel.Patronumic = Patronumic.Text;
                passengersModel.DateOfBirth = DateTime.Parse(DateOfBirth.Text);
                passengersModel.Phone = Telephone.Text;
                passengersModel.PassportData = PassportDate.Text;
                passengersModel.Email = Email.Text;
                passengersModel.IdTickets = generatedId;

                context.Passengers.Add(passengersModel);
                context.SaveChanges();
            }

            Close();
        }
        //МЕтод для цены
        private void UpdateTicketPrice()
        {
            if (ClassService.SelectedItem != null)
            {
                ticketsModel.ClassService = ((ComboBoxItem)ClassService.SelectedItem).Content.ToString();

                if (ticketsModel.ClassService == "Эконом")
                {
                    if (_selectedFlight != null)
                    {
                        ticketsModel.TicketPrice = _selectedFlight.EconomyPrice ?? 2;
                    }
                }
                else if (ticketsModel.ClassService == "Комфорт")
                {
                    if (_selectedFlight != null)
                    {
                        ticketsModel.TicketPrice = _selectedFlight.ComfortPrice ?? 2;
                    }
                }
                else if (ticketsModel.ClassService == "Бизнес")
                {
                    if (_selectedFlight != null)
                    {
                        ticketsModel.TicketPrice = _selectedFlight.BusinessPrice ?? 2;
                    }
                }

                PriceTicket.Text = ticketsModel.TicketPrice.ToString();
            }
        }
        // метод для обновления
        private void TicketStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTicketPrice(); // вызов метода при изменении статуса билета
        }
        private void DecreaseAvailableSeats()
        {
            if (ClassService.SelectedItem != null)
            {
                using (var context = new DBContextAir())
                {
                    var selectedFlight = context.Flights.FirstOrDefault(f => f.IdFlights == _selectedFlight.IdFlights);

                    if (selectedFlight != null)
                    {
                        if (ticketsModel.ClassService == "Эконом" && selectedFlight.EconomyClassSeats > 0)
                        {
                            selectedFlight.EconomyClassSeats--;
                        }
                        else if (ticketsModel.ClassService == "Комфорт" && selectedFlight.ComfortClassSeats > 0)
                        {
                            selectedFlight.ComfortClassSeats--;
                        }
                        else if (ticketsModel.ClassService == "Бизнес" && selectedFlight.BusinessClassSeats > 0)
                        {
                            selectedFlight.BusinessClassSeats--;
                        }

                        context.SaveChanges(); // Сохранение изменений в базе данных
                    }
                }
            }
        }


        private void Telephone_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string phoneNumber = textBox.Text;

            // Проверяем, что номер телефона начинается с "+7" и содержит только цифры
            if (!phoneNumber.StartsWith("+7") || !Regex.IsMatch(phoneNumber.Substring(1), @"^\d+$"))
            {
                if (!phoneNumber.StartsWith("+7"))
                {
                    // Если номер не начинается с "+7", добавляем его автоматически
                    phoneNumber = "+7" + phoneNumber;
                }
                else
                {
                    // Если номер начинается с "+7", но содержит нецифровые символы, очищаем его
                    phoneNumber = "+7";
                }

                // Обновляем текст в поле ввода с новым номером
                textBox.Text = phoneNumber;
                // Устанавливаем курсор в конец текста, чтобы пользователь мог продолжить вводить номер
                textBox.SelectionStart = phoneNumber.Length;
            }
        }
    }
}


