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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private bool IsPasswordValid(string password)
        {
            // Проверка на длину пароля
            if (password.Length < 8)
                return false;

            // Проверка на наличие хотя бы одной цифры
            if (!password.Any(char.IsDigit))
                return false;

            // Проверка на наличие хотя бы одной буквы в верхнем регистре
            if (!password.Any(char.IsUpper))
                return false;

            // Проверка на наличие хотя бы одной буквы в нижнем регистре
            if (!password.Any(char.IsLower))
                return false;

            // Проверка на наличие хотя бы одного спецсимвола
            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                return false;

            return true;
        }

        private bool ArePasswordsMatching(string password, string confirmPassword)
        {
            return password.Equals(confirmPassword);
        }

        private void ButtonRegistration_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DBContextAir())
            {
                string login = TextBoxLogin.Text;
                string password = PasswordBoxPassword.Password;
                string confirmPassword = PasswordBoxPasswordCopy.Password;

                // Проверка на существование логина в базе данных
                if (context.Authorization.Any(r => r.Login == login))
                {
                    MessageBox.Show("Логин уже занят.");
                    return;
                }

                // Проверка на валидность пароля
                if (!IsPasswordValid(password))
                {
                    MessageBox.Show("Пароль должен содержать не менее 8 символов, хотя бы одну цифру, буквы в верхнем и нижнем регистре, а также хотя бы один спецсимвол.");
                    return;
                }

                // Проверка совпадения паролей
                if (!ArePasswordsMatching(password, confirmPassword))
                {
                    MessageBox.Show("Пароли не совпадают.");
                    return;
                }

                RegictrationModel regictrationModel = new RegictrationModel
                {
                    Login = login,
                    Password = password,
                    Role = ((ComboBoxItem)ComboBoxRole.SelectedItem).Content.ToString()
                };

                context.Authorization.Add(regictrationModel);
                context.SaveChanges();
                this.Close();
            }
        }
    }
}
