using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SavingsApp
{
    public partial class Saving : UserControl
    {
        private decimal generalBalance = 0;
        private decimal totalSavings = 0;

        public Saving()
        {
            InitializeComponent();
            UpdateGeneralBalance();
        }

        private void AmountSavingTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Allow only numbers, dot, and comma
            Regex regex = new Regex(@"^[0-9.,]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void SaveSavingButton_Click(object sender, RoutedEventArgs e)
        {
            string amountText = AmountSavingTextBox.Text.Trim();

            // Check if amount is empty
            if (string.IsNullOrEmpty(amountText))
            {
                ShowError("Впишіть суму витрат");
                return;
            }

            // Replace comma with dot for decimal parsing
            amountText = amountText.Replace(',', '.');

            // Try to parse the amount
            if (decimal.TryParse(amountText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount))
            {
                // Check if amount is positive
                if (amount <= 0)
                {
                    ShowError("Сума повинна містити лише додатні числа!");
                    return;
                }

                // Add to savings and general balance
                totalSavings += amount;
                generalBalance += amount;

                // Update display
                UpdateGeneralBalance();
                ShowSuccess($"Успішно додано: ${amount:F2}");

                // Clear the input field
                AmountSavingTextBox.Text = "";

                // Simulate saving to ViewSavings
                SaveToViewSavings(amount);
            }
            else
            {
                ShowError("Сума повинна містити лише додатні числа!");
            }
        }

        private void SaveToViewSavings(decimal amount)
        {
            // Тут буде логіка збереження до ViewSavings
            // Наразі просто показуємо в консолі
            Console.WriteLine($"Saved to ViewSavings: ${amount}. Total savings: ${totalSavings}");

            // Можна викликати подію для головного вікна
            // OnSaveSaving?.Invoke(amount);
        }

        private void CancelSavingButton_Click(object sender, RoutedEventArgs e)
        {
            ReturnToMainScreen();
        }

        private void ReturnsSavingButton_Click(object sender, RoutedEventArgs e)
        {
            ReturnToMainScreen();
        }

        private void ReturnToMainScreen()
        {
            // Clear any errors and input
            SavingErrorText.Visibility = Visibility.Collapsed;
            AmountSavingTextBox.Text = "";

            // Можна викликати подію для повернення на головний екран
            // OnReturnToMain?.Invoke();
        }

        private void UpdateGeneralBalance()
        {
            GeneralBalanceText.Text = $"General Balance: ${generalBalance:F2}";
        }

        private void ShowError(string message)
        {
            SavingErrorText.Text = message;
            SavingErrorText.Foreground = System.Windows.Media.Brushes.Red;
            SavingErrorText.Visibility = Visibility.Visible;
        }

        private void ShowSuccess(string message)
        {
            SavingErrorText.Text = message;
            SavingErrorText.Foreground = System.Windows.Media.Brushes.Green;
            SavingErrorText.Visibility = Visibility.Visible;
        }

        // Події для інтеграції з головним вікном
        public event Action<decimal> OnSaveSaving;
        public event Action OnReturnToMain;
    }
}