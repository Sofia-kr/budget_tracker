using MySql.Data.MySqlClient;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace budget_tracker
{
    public partial class Expenses : Window
    {
        public Expenses()
        {
            InitializeComponent();
            DateExpenses.SelectedDate = DateTime.Now;
            LoadCategories(); // Завантажуємо категорії при ініціалізації
        }

        private void LoadCategories()
        {
            try
            {
                using (MySqlConnection connection = DBUtils.GetDBConnection())
                {
                    connection.Open();
                    string query = "SELECT IDcategory, CNameExpenses FROM expensescategory";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    ChoiceType.Items.Clear();
                    while (reader.Read())
                    {
                        ComboBoxItem item = new ComboBoxItem();
                        item.Content = reader["CNameExpenses"].ToString();
                        item.Tag = reader["IDcategory"].ToString();
                        ChoiceType.Items.Add(item);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження категорій: {ex.Message}");
            }
        }

        private void Category_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string category = button.Content.ToString();
                MessageBox.Show($"Selected category: {category}");
            }
        }

        private void AddCategoryExpenses_Click(object sender, RoutedEventArgs e)
        {
            FAddCategoryExpenses addCategoryWindow = new FAddCategoryExpenses();
            addCategoryWindow.ShowDialog();
            LoadCategories(); // Оновлюємо список категорій після додавання нової
        }

        private void SaveExpenses_Click(object sender, RoutedEventArgs e)
        {
            // Перевірка категорії
            if (ChoiceType.SelectedItem == null)
            {
                MessageBox.Show("Оберіть категорію!");
                return;
            }

            // Перевірка суми
            string amountText = AmountExpenses.Text.Replace(',', '.');
            if (string.IsNullOrWhiteSpace(amountText))
            {
                MessageBox.Show("Впишіть суму витрат!");
                return;
            }

            if (!Regex.IsMatch(amountText, @"^\d+(\.\d{1,2})?$"))
            {
                MessageBox.Show("Сума повинна містити лише додатні числа!");
                return;
            }

            double amount = double.Parse(amountText);
            if (amount <= 0)
            {
                MessageBox.Show("Сума повинна бути додатною!");
                return;
            }

            // Збереження в базу даних
            SaveExpenseToDatabase(amount);
        }

        private void SaveExpenseToDatabase(double amount)
        {
            try
            {
                using (MySqlConnection connection = DBUtils.GetDBConnection())
                {
                    connection.Open();

                    // Отримуємо ID категорії
                    ComboBoxItem selectedItem = (ComboBoxItem)ChoiceType.SelectedItem;
                    int categoryId = int.Parse(selectedItem.Tag.ToString());
                    string categoryName = selectedItem.Content.ToString();

                    // Отримуємо ID поточного користувача (потрібно реалізувати отримання поточного користувача)
                    int currentUserId = GetCurrentUserId(); // Цей метод потрібно реалізувати

                    // Отримуємо зображення категорії
                    string categoryImage = GetCategoryImage(categoryId);

                    string query = @"INSERT INTO expenses 
                           (IDuser, IDcategory, CategoryImageExpenses, CategoryNameExpenses, AmountExpenses, ExpenseDate) 
                           VALUES (@userid, @categoryid, @image, @categoryname, @amount, @date)";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@userid", currentUserId);
                    cmd.Parameters.AddWithValue("@categoryid", categoryId);
                    cmd.Parameters.AddWithValue("@image", categoryImage);
                    cmd.Parameters.AddWithValue("@categoryname", categoryName);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@date", DateExpenses.SelectedDate);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Витрати успішно збережено!");
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Помилка при збереженні витрат!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка збереження в базу даних: {ex.Message}");
            }
        }

        private int GetCurrentUserId()
        {
            // Тут потрібно реалізувати отримання ID поточного користувача
            // Наприклад, з сесії, з статичного класу або з файлу конфігурації
            // Приклад:
            // return UserSession.CurrentUserId;

            // Тимчасово повертаємо 1 (для тестування)
            return 1;
        }

        private string GetCategoryImage(int categoryId)
        {
            try
            {
                using (MySqlConnection connection = DBUtils.GetDBConnection())
                {
                    connection.Open();
                    string query = "SELECT ImageExpenses FROM expensescategory WHERE IDcategory = @categoryId";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@categoryId", categoryId);

                    object result = cmd.ExecuteScalar();
                    return result?.ToString() ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка отримання зображення категорії: {ex.Message}");
                return string.Empty;
            }
        }

        private void ClearForm()
        {
            AmountExpenses.Text = string.Empty;
            ChoiceType.SelectedIndex = -1;
            DateExpenses.SelectedDate = DateTime.Now;
        }

        private void CancelExpenses_Click(object sender, RoutedEventArgs e)
        {
            MainScreen main = new MainScreen();
            main.Show();
            this.Close();
        }

        private void ReturnExpenses_Click(object sender, RoutedEventArgs e)
        {
            MainScreen main = new MainScreen();
            main.Show();
            this.Close();
        }
    }
}
