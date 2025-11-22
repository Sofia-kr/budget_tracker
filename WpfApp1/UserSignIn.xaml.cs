using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using WpfApp1;

using MySql.Data.MySqlClient;
private string connectionString = "server=sql7.freesqldatabase.com;port=3306;user=sql7803706;password=DrUIbcmB1f;database=sql7803706;Charset=utf8mb4;";

namespace UserSignInApp
{
    public partial class UserSignIn : UserControl
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=UserDB;Integrated Security=True";

        public UserSignIn()
        {
            InitializeComponent();
        }

        private bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            string pattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
            return Regex.IsMatch(email, pattern);
        }

        private bool ValidatePassword(string password)
        {
            return !string.IsNullOrEmpty(password) && password.Length >= 8;
        }

        private void GmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(GmailTextBox.Text))
            {
                GmailErrorText.Visibility = Visibility.Collapsed;
                return;
            }

            if (!ValidateEmail(GmailTextBox.Text))
            {
                GmailErrorText.Text = "Некоректний формат Gmail";
                GmailErrorText.Visibility = Visibility.Visible;
            }
            else
            {
                GmailErrorText.Visibility = Visibility.Collapsed;
            }
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            GeneralErrorText.Visibility = Visibility.Collapsed;
            PasswordErrorText.Visibility = Visibility.Collapsed;

            string email = GmailTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ShowError("Будь ласка, заповніть всі поля");
                return;
            }

            if (!ValidateEmail(email))
            {
                ShowError("Некоректний формат Gmail");
                return;
            }

            if (!ValidatePassword(password))
            {
                PasswordErrorText.Text = "Пароль повинен містити至少 8 символів";
                PasswordErrorText.Visibility = Visibility.Visible;
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string checkUserQuery = "SELECT COUNT(*) FROM UserData WHERE Gmail = @Gmail";
                    SqlCommand checkUserCmd = new SqlCommand(checkUserQuery, connection);
                    checkUserCmd.Parameters.AddWithValue("@Gmail", email);

                    int userCount = (int)checkUserCmd.ExecuteScalar();

                    if (userCount == 0)
                    {
                        ShowError("Ви не зареєстровані!");
                        return;
                    }

                    string checkPasswordQuery = "SELECT Password FROM UserData WHERE Gmail = @Gmail";
                    SqlCommand checkPasswordCmd = new SqlCommand(checkPasswordQuery, connection);
                    checkPasswordCmd.Parameters.AddWithValue("@Gmail", email);

                    string storedPassword = checkPasswordCmd.ExecuteScalar()?.ToString();

                    if (storedPassword != password)
                    {
                        ShowError("Пароль або електронна пошта введені не правильно");
                        return;
                    }

                    MessageBox.Show("Вхід успішний!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigateToMainScreen();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Помилка бази даних: {ex.Message}");
            }
        }

        private void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                var passwordRecovery = new PasswordRecovery();
                mainWindow?.MainFrame.Navigate(passwordRecovery);
            }
            catch
            {
                MessageBox.Show("Форма відновлення пароля тимчасово недоступна", "Інформація",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                var userRegistration = new UserRegistration();
                mainWindow?.MainFrame.Navigate(userRegistration);
            }
            catch
            {
                MessageBox.Show("Форма реєстрації тимчасово недоступна", "Інформація",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ShowError(string message)
        {
            GeneralErrorText.Text = message;
            GeneralErrorText.Visibility = Visibility.Visible;
        }

        private void NavigateToMainScreen()
        {
            try
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                var mainScreen = new MainScreen();
                mainWindow?.MainFrame.Navigate(mainScreen);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка переходу: {ex.Message}", "Помилка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

