using SimpleWPFWork.WPFUI.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Xceed.Wpf.Toolkit;

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

            //  ColorPicker Binding (Color → Hex String iki yönlü)
            CategoryColorPicker.SelectedColorChanged += (s, e) =>
            {
                if (CategoryColorPicker.SelectedColor.HasValue)
                {
                    var color = CategoryColorPicker.SelectedColor.Value;
                    _viewModel.CategoryColor = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
                }
            };

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

            //  ViewModel değişikliklerini dinle
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_viewModel.SelectedCategory))
                {
                    UpdateCategoryDeleteButtonVisibility();
                }

                //  ViewModel'deki CategoryColor değişince ColorPicker'ı güncelle
                if (e.PropertyName == nameof(_viewModel.CategoryColor))
                {
                    UpdateColorPickerFromViewModel();
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

        private async void DeleteCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.DeleteCategoryAsync();
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
        }

        // Delete butonunu göster/gizle
        private void UpdateCategoryDeleteButtonVisibility()
        {
            // Sadece gerçek kategori seçiliyse göster ("Create New" değilse)
            if (_viewModel.SelectedCategory != null && _viewModel.SelectedCategory.Id != Guid.Empty)
            {
                DeleteCategoryButton.Visibility = Visibility.Visible;
            }
            else
            {
                DeleteCategoryButton.Visibility = Visibility.Collapsed;
            }
        }

        //  ViewModel'deki hex string'i ColorPicker'a aktar
        private void UpdateColorPickerFromViewModel()
        {
            try
            {
                var hexColor = _viewModel.CategoryColor;
                if (!string.IsNullOrEmpty(hexColor))
                {
                    var color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(hexColor);
                    CategoryColorPicker.SelectedColor = color;
                }
            }
            catch
            {
                // Invalid color format, set default
                CategoryColorPicker.SelectedColor = System.Windows.Media.Colors.Blue;
            }
        }
    }

    // DİKKAT: Bu converter'lar MainWindow CLASS'ININ DIŞINDA olmalı!
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

    // Hex String → Brush Converter
    public class HexColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string hexColor && !string.IsNullOrEmpty(hexColor))
            {
                try
                {
                    var color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(hexColor);
                    return new System.Windows.Media.SolidColorBrush(color);
                }
                catch
                {
                    return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
                }
            }
            return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}