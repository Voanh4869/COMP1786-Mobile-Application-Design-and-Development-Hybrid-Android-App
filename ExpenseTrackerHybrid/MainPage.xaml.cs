using ExpenseTrackerHybrid.Models;
using ExpenseTrackerHybrid.Services;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTrackerHybrid
{
    public partial class MainPage : ContentPage
    {
        private List<Project> _allProjects = new List<Project>();
        private Project _selectedProject;
        private readonly DatabaseService _dataService;

        public MainPage()
        {
            InitializeComponent();
            _dataService = new DatabaseService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadProjectsAsync();
        }

        private async Task LoadProjectsAsync()
        {
            _allProjects = await _dataService.GetProjectsAsync();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ProjectsCollectionView.ItemsSource = _allProjects;
            });
        }

        private void OnSearchButtonPressed(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            PerformSearch();
        }

        private void OnSearchDateChanged(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void OnClearDateClicked(object sender, EventArgs e)
        {
            UseDateFilterCheckBox.IsChecked = false;
        }

        private void PerformSearch()
        {
            var query = ProjectSearchBar.Text?.ToLower() ?? string.Empty;
            var useDate = UseDateFilterCheckBox.IsChecked;
            var searchDate = ((DateTime)ProjectSearchDatePicker.Date).Date;

            var filtered = _allProjects.Where(p => 
                (string.IsNullOrWhiteSpace(query) || (p.Name != null && p.Name.ToLower().Contains(query))) &&
                (!useDate || (DateTime.TryParseExact(p.Date, new[] { "dd/MM/yyyy", "yyyy-MM-dd" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var projectDate) && projectDate.Date == searchDate))
            ).ToList();

            ProjectsCollectionView.ItemsSource = filtered;
        }

        private async void OnProjectSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Project selected)
            {
                _selectedProject = selected;
                SelectedProjectLabel.Text = $"Selected Project: {_selectedProject.Name}";
                await LoadExpensesAsync();
            }
            else
            {
                _selectedProject = null;
                SelectedProjectLabel.Text = "Selected Project: None";
                ExpensesCollectionView.ItemsSource = null;
            }
        }

        private async Task LoadExpensesAsync()
        {
            if (_selectedProject != null)
            {
                var expenses = await _dataService.GetExpensesAsync(_selectedProject.Id);
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ExpensesCollectionView.ItemsSource = null;
                    ExpensesCollectionView.ItemsSource = expenses;
                });
            }
        }

        private async void OnAddExpenseClicked(object sender, EventArgs e)
        {
            if (_selectedProject == null)
            {
                await DisplayAlert("Error", "Please select a project first", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(AmountEntry.Text) || TypePicker.SelectedItem == null || PaymentMethodPicker.SelectedItem == null || ClaimantEntry.Text == null || PaymentStatusPicker.SelectedItem == null || CurrencyPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "All required fields must be filled", "OK");
                return;
            }

            if (!decimal.TryParse(AmountEntry.Text, out decimal amount))
            {
                await DisplayAlert("Error", "Invalid amount entered", "OK");
                return;
            }

            var expense = new Expense
            {
                ProjectId = _selectedProject.Id,
                DateOfExpense = ((DateTime)ExpenseDatePicker.Date).ToString("yyyy-MM-dd"),
                Amount = amount,
                Currency = CurrencyPicker.SelectedItem.ToString(),
                TypeOfExpense = TypePicker.SelectedItem.ToString(),
                PaymentMethod = PaymentMethodPicker.SelectedItem.ToString(),
                Claimant = ClaimantEntry.Text,
                PaymentStatus = PaymentStatusPicker.SelectedItem.ToString(),
                Description = DescriptionEntry.Text ?? string.Empty,
                Location = LocationEntry.Text ?? string.Empty
            };

            // Save directly to Firebase Database
            bool success = await _dataService.SaveExpenseAsync(expense) == 1;

            await DisplayAlert(success ? "Success" : "Error", success ? "Expense saved successfully." : "Failed to save expense.", "OK");

            if (success)
            {
                // Clear fields
                AmountEntry.Text = string.Empty;
                ClaimantEntry.Text = string.Empty;
                DescriptionEntry.Text = string.Empty;
                LocationEntry.Text = string.Empty;
                TypePicker.SelectedItem = null;
                PaymentMethodPicker.SelectedItem = null;
                PaymentStatusPicker.SelectedItem = null;
                CurrencyPicker.SelectedItem = null;

                await LoadExpensesAsync();
            }
        }

        private async void OnDeleteExpenseClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Expense expense)
            {
                bool confirm = await DisplayAlert("Confirm", "Delete this expense?", "Yes", "No");
                if (confirm)
                {
                    await _dataService.DeleteExpenseAsync(expense);
                    await LoadExpensesAsync();
                }
            }
        }
    }
}

