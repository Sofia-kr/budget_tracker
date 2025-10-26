using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using MySql.Data.MySqlClient;

namespace budget_tracker
{
    /// <summary>
    /// Interaction logic for UserRegistration.xaml
    /// </summary>
    public partial class UserRegistration : Window
    {
        private string connectionString = "server=sql7.freesqldatabase.com;port=3306;user=sql7803706;password=DrUIbcmB1f;database=sql7803706;Charset=utf8mb4;";
        public UserRegistration()
        {
            InitializeComponent();
        }

        private bool IsEmailRegistered(string email)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM  userdata WHERE Gmail=@Email";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string name = NameTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string question = SpecialQuestionTextBox.Text.Trim();
            string answer = SpecialAnswerTextBox.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(question) ||
                string.IsNullOrEmpty(answer))
            {
                MessageBox.Show("Заповніть всі поля!");
                return;
            }

            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$"))
            {
                MessageBox.Show("Неправильний формат Gmail!");
                return;
            }

            // Перевірка Name
            if (name.Length > 20)
            {
                MessageBox.Show("Ім'я повинне містити не більше 20 символів");
                return;
            }

            // Перевірка Password
            if (password.Length < 5)
            {
                MessageBox.Show("Пароль повинен містити більше 5 символів");
                return;
            }

            if (IsEmailRegistered(email))
            {
                MessageBox.Show("Ви вже зареєстровані!");
                return;
            }

            MessageBox.Show("Питання буде використане для відновлення паролю! Переконайтеся, що знатимете відповідь!");

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string insertQuery = @"INSERT INTO userdata (Name, Gmail, Password, SpecialQuestion, SpecialAnswer)
                                       VALUES (@Name, @Gmail, @Password, @SpecialQuestion, @SpecialAnswer)";
                using (var cmd = new MySqlCommand(insertQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Gmail", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@SpecialQuestion", question);
                    cmd.Parameters.AddWithValue("@SpecialAnswer", answer);

                    cmd.ExecuteNonQuery();
                }
            }
            // Повідомлення про успіх
            MessageBox.Show("Успішна реєстрація!");

            //MainScreen main = new MainScreen();
            //main.Show();
            //this.Close();

        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            //UserSignIn signIn = new UserSignIn();
            //signIn.Show();
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //Login_Register login = new Login_Register();
            //login.Show();
            this.Close();
        }
    }
}
