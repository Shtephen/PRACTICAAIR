using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PRACTICAAIR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {   
        private string connectionSQL = "Data Source=DESKTOP-ANRB8JC;Initial Catalog=PRACTICAAIR;Integrated Security=True;";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = TextBoxLogin.Text;
            string password = PasswordBoxPassword.Password;
            string role = ComboBoxRole.Text;

            if (CheckLogin(login, password,role)) 
            {
                ListFlights listFlights = new ListFlights();
                listFlights.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неправильный логин, пароль или роль.");
            }
        }
        private bool CheckLogin (string login, string password,string role)
        {
            using (SqlConnection connection = new SqlConnection(connectionSQL))
            {
            connection.Open();
                string query = $"SELECT COUNT(*) FROM [Authorization] WHERE Login = '{login}' AND Password = '{password}' AND Role = '{role}'";
                SqlCommand command = new SqlCommand(query, connection);
                int count = (int) command.ExecuteScalar();
                return count == 1;
            }
        }

        private void ButtonReg_Click (object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.ShowDialog();
        }

        
    }
}