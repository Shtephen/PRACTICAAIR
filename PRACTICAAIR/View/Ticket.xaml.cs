using Microsoft.Data.SqlClient;
using PRACTICAAIR.Model;
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
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;

namespace PRACTICAAIR
{
    /// <summary>
    /// Логика взаимодействия для Ticket.xaml
    /// </summary>
    public partial class Ticket : Window
    {
        public Ticket(int flightId)
        {
            InitializeComponent();
            List<TicketsModel> tickets = GetTicketsFromSqlServer(flightId);

            List<PassengersModel> passengersModels = new List<PassengersModel>();
            foreach (var ticket in tickets)
            {
                int ticketId = ticket.IdTickets;
                List<PassengersModel> passengers = GetPassengersFromSqlServer(ticketId);
                passengersModels.AddRange(passengers);
            }

            ListViewTicket.ItemsSource = tickets;
            ListViewPassengers.ItemsSource = passengersModels;

        }

        public List<TicketsModel> GetTicketsFromSqlServer(int flightId)
        {
            List<TicketsModel> ticketsModels = new List<TicketsModel>();
            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-ANRB8JC;Initial Catalog=PRACTICAAIR;Integrated Security=True;Trust Server Certificate=True"))
            {
                connection.Open();
                string query = "SELECT * FROM Tickets WHERE IdFlights = @FlightId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FlightId", flightId);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TicketsModel ticketsModel = new TicketsModel();
                    ticketsModel.IdTickets = Convert.ToInt32(reader["IdTickets"]);
                    ticketsModel.SeatNumber = reader["SeatNumber"].ToString();
                    ticketsModel.ClassService = reader["ClassService"].ToString();
                    ticketsModel.TicketStatus = reader["TicketStatus"].ToString();
                    ticketsModel.TicketPrice = Math.Round(Convert.ToDecimal(reader["TicketPrice"]), 2);
                    ticketsModel.IdFlights = Convert.ToInt32(reader["IdFlights"]);
                    ticketsModels.Add(ticketsModel);
                }
            }
            return ticketsModels;
        }

        public List<PassengersModel> GetPassengersFromSqlServer(int ticketId)
        {
            List<PassengersModel> passengersModels = new List<PassengersModel>();
            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-ANRB8JC;Initial Catalog=PRACTICAAIR;Integrated Security=True;Trust Server Certificate=True"))
            {
                connection.Open();
                string query = "SELECT * FROM Passengers WHERE IdTickets = @TicketId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TicketId", ticketId);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PassengersModel passengersModel = new PassengersModel();
                    passengersModel.IdPassengers = Convert.ToInt32(reader["IdPassengers"]);
                    passengersModel.Gender = reader["Gender"].ToString();
                    passengersModel.Surname = reader["Surname"].ToString();
                    passengersModel.Name = reader["Name"].ToString();
                    passengersModel.Patronumic = reader["Patronumic"].ToString();
                    passengersModel.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                    passengersModel.PassportData = reader["PassportData"].ToString();
                    passengersModel.Phone = reader["Phone"].ToString();
                    passengersModel.Email = reader["Email"].ToString();
                    passengersModel.IdTickets = Convert.ToInt32(reader["IdTickets"]);
                    passengersModels.Add(passengersModel);
                }
            }
            return passengersModels;
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            ListFlights listFlights = new ListFlights();
            listFlights.ShowDialog();
        }
        private void VedomostButton_Click(Object sender, EventArgs e)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; 

            FileInfo file = new FileInfo(@"C:\Users\Stephen\Desktop\Практика\PRACTICAAIR\PRACTICAAIR\Statements\Statements.xlsx");
            using (ExcelPackage package = new ExcelPackage(file))
            {

                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Ведомость на рейс");
                worksheet.Cells["A1"].Value = "Ведомость на рейс";
                worksheet.Cells["A1:O1"].Merge = true; 
                worksheet.Cells["A1:M2"].Style.Font.Bold = true; 
                worksheet.Cells["A1:O1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells.AutoFitColumns();
                worksheet.Cells["A2"].Value = "Номер места";
                worksheet.Cells["B2"].Value = "Класс обуслуживания";
                worksheet.Cells["C2"].Value = "Статус билета";
                worksheet.Cells["D2"].Value = "Цена";
                worksheet.Cells["F2"].Value = "Пол";
                worksheet.Cells["G2"].Value = "Фамилия";
                worksheet.Cells["H2"].Value = "Имя";
                worksheet.Cells["I2"].Value = "Отчество";
                worksheet.Cells["J2"].Value = "Дата рождения";
                worksheet.Cells["K2"].Value = "Паспортные данные";
                worksheet.Cells["L2"].Value = "Номер телефона";
                worksheet.Cells["M2"].Value = "Email";
                

                int col = 1;
                int row = 3;
                foreach (var item in ListViewTicket.Items)
                {
                    col = 1;
                    foreach (var property in item.GetType().GetProperties())
                    {
                        if (property.Name != "IdTickets" && property.Name != "IdFlights") 
                        {
                            worksheet.Cells[row, col].Value = property.GetValue(item).ToString();
                            col++;
                        }
                    }
                    row++;
                }


                
                row = 3;
                foreach (var item in ListViewPassengers.Items)
                {
                    col = 6;
                    foreach (var property in item.GetType().GetProperties())
                    {
                        if (property.Name != "IdPassengers" && property.Name != "IdTickets") 
                        {
                            worksheet.Cells[row, col].Value = property.GetValue(item).ToString();
                            col++;
                        }
                    }
                    row++;
                }

                package.Save();
                MessageBox.Show("Ведомость успешно создана!");
            }
        }
    }
}
