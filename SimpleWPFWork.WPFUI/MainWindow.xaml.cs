using SimpleWPFWork.WPFUI.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SimpleWPFWork.WPFUI
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            SetupBindings();
            Loaded += MainWindow_Loaded;
        }

        private void SetupBindings()
        {
            // Category Bindings
            CategoriesComboBox.SetBinding(ItemsControl.ItemsSourceProperty,
                new Binding("Categories") { Source = _viewModel });
            CategoriesComboBox.SetBinding(System.Windows.Controls.Primitives.Selector.SelectedItemProperty,
                new Binding("SelectedCategory") { Source = _viewModel });

            CategoryNameTextBox.SetBinding(TextBox.TextProperty,
                new Binding("CategoryName")
                {
                    Source = _viewModel,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

            CategoryColorTextBox.SetBinding(TextBox.TextProperty,
                new Binding("CategoryColor")
                {
                    Source = _viewModel,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

            // Todo List Binding
            TodosListBox.SetBinding(ItemsControl.ItemsSourceProperty,
                new Binding("TodosList") { Source = _viewModel });
            TodosListBox.SetBinding(System.Windows.Controls.Primitives.Selector.SelectedItemProperty,
                new Binding("SelectedTodo") { Source = _viewModel });

            // Todo Form Bindings
            TitleTextBox.SetBinding(TextBox.TextProperty,
                new Binding("TodoTitle")
                {
                    Source = _viewModel,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

            DescriptionTextBox.SetBinding(TextBox.TextProperty,
                new Binding("TodoDescription")
                {
                    Source = _viewModel,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

            IsCompletedCheckBox.SetBinding(System.Windows.Controls.Primitives.ToggleButton.IsCheckedProperty,
                new Binding("TodoIsCompleted") { Source = _viewModel });

            UsernameTextBox.SetBinding(TextBox.TextProperty,
                new Binding("TodoUsername")
                {
                    Source = _viewModel,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

            DueDatePicker.SetBinding(DatePicker.SelectedDateProperty,
                new Binding("TodoDueDate")
                {
                    Source = _viewModel,
                    Converter = new DateTimeOffsetToDateTimeConverter()
                });

            // Priority - Manuel olarak seçili olanı sync edeceğiz
            PriorityComboBox.SelectionChanged += (s, e) =>
            {
                if (PriorityComboBox.SelectedItem is ComboBoxItem item)
                {
                    _viewModel.TodoPriority = item.Content.ToString();
                }
            };
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadCategoriesAsync();
        }

        private async void SaveCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.SaveCategoryAsync();
        }

        private async void SaveTodoButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.SaveTodoAsync();
        }

        private async void DeleteTodoButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.DeleteTodoAsync();
        }

        private void TodosListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ViewModel zaten SelectedTodo binding'i üzerinden todo seçimini yakalıyor
            // Burada ekstra işlem yapmaya gerek yok
        }
    }

    // DateTimeOffset <-> DateTime Converter
    public class DateTimeOffsetToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTimeOffset dto)
            {
                return dto.DateTime;
            }
            return DateTime.Now;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTime dt)
            {
                return new DateTimeOffset(dt);
            }
            return DateTimeOffset.Now;
        }
    }
}