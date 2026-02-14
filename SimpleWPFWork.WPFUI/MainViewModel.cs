using SimpleWPFWork.WPFUI.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleWPFWork.WPFUI
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IClient _client;

        // Categories
        public ObservableCollection<CategoryDto> Categories { get; set; }
        private CategoryDto _selectedCategory;
        public CategoryDto SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory == value) return; // ✅ SONSUZ DÖNGÜYÜ ÖNLE!
                _selectedCategory = value;
                OnPropertyChanged();
                OnCategorySelected();
            }
        }

        private string _categoryName;
        public string CategoryName
        {
            get => _categoryName;
            set
            {
                if (_categoryName == value) return; // ✅
                _categoryName = value;
                OnPropertyChanged();
            }
        }

        private string _categoryColor;
        public string CategoryColor
        {
            get => _categoryColor;
            set
            {
                if (_categoryColor == value) return; // ✅
                _categoryColor = value;
                OnPropertyChanged();
            }
        }

        // Todos
        public ObservableCollection<TodoDto> TodosList { get; set; }
        private TodoDto _selectedTodo;
        public TodoDto SelectedTodo
        {
            get => _selectedTodo;
            set
            {
                if (_selectedTodo == value) return; // ✅
                _selectedTodo = value;
                OnPropertyChanged();
                OnTodoSelected();
            }
        }

        // Todo Form Fields
        private string _todoTitle;
        public string TodoTitle
        {
            get => _todoTitle;
            set
            {
                if (_todoTitle == value) return; // ✅
                _todoTitle = value;
                OnPropertyChanged();
            }
        }

        private string _todoDescription;
        public string TodoDescription
        {
            get => _todoDescription;
            set
            {
                if (_todoDescription == value) return; // ✅
                _todoDescription = value;
                OnPropertyChanged();
            }
        }

        private bool _todoIsCompleted;
        public bool TodoIsCompleted
        {
            get => _todoIsCompleted;
            set
            {
                if (_todoIsCompleted == value) return; // ✅
                _todoIsCompleted = value;
                OnPropertyChanged();
            }
        }

        private string _todoPriority;
        public string TodoPriority
        {
            get => _todoPriority;
            set
            {
                if (_todoPriority == value) return; // ✅
                _todoPriority = value;
                OnPropertyChanged();
            }
        }

        private DateTimeOffset _todoDueDate;
        public DateTimeOffset TodoDueDate
        {
            get => _todoDueDate;
            set
            {
                if (_todoDueDate == value) return; // ✅
                _todoDueDate = value;
                OnPropertyChanged();
            }
        }

        private string _todoUsername;
        public string TodoUsername
        {
            get => _todoUsername;
            set
            {
                if (_todoUsername == value) return; // ✅
                _todoUsername = value;
                OnPropertyChanged();
            }
        }

        // UI States
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading == value) return; // ✅
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage == value) return; // ✅
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        private bool _isTodoFormEnabled;
        public bool IsTodoFormEnabled
        {
            get => _isTodoFormEnabled;
            set
            {
                if (_isTodoFormEnabled == value) return; // ✅
                _isTodoFormEnabled = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel(IClient client)
        {
            _client = client;
            Categories = new ObservableCollection<CategoryDto>();
            TodosList = new ObservableCollection<TodoDto>();
            CategoryColor = "#3498DB";
            TodoPriority = "Normal";
            TodoDueDate = DateTimeOffset.Now.AddDays(7);
            TodoUsername = Environment.UserName;
            IsTodoFormEnabled = false;
        }

        public async Task LoadCategoriesAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Loading categories...";

                var categories = await _client.CategoryAllAsync(null, null, 100, 0);

                Categories.Clear();

                // İlk sıraya placeholder ekle
                Categories.Add(new CategoryDto
                {
                    Id = Guid.Empty,
                    Name = "-- Create New Category --",
                    Color = "#CCCCCC"
                });

                foreach (var category in categories)
                {
                    Categories.Add(category);
                }

                StatusMessage = $"Loaded {categories.Count} categories";
            }
            catch (ApiException ex)
            {
                StatusMessage = $"API Error: {ex.Message}";
                MessageBox.Show($"API Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void OnCategorySelected()
        {
            if (SelectedCategory == null || SelectedCategory.Id == Guid.Empty)
            {
                CategoryName = string.Empty;
                CategoryColor = "#3498DB";
                TodosList.Clear();
                IsTodoFormEnabled = false;
                ClearTodoForm();
                StatusMessage = "Ready to create new category";
                return;
            }

            CategoryName = SelectedCategory.Name;
            CategoryColor = SelectedCategory.Color;
            await LoadTodosByCategoryAsync(SelectedCategory.Id);
            IsTodoFormEnabled = true;
        }

        private async Task LoadTodosByCategoryAsync(Guid categoryId)
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Loading todos...";

                var todos = await _client.TodoAllAsync(
                    title: null,
                    description: null,
                    isCompleted: null,
                    priority: null,
                    dueDate: null,
                    categoryId: categoryId,
                    username: null,
                    limit: 100,
                    page: 0
                );

                TodosList.Clear();
                foreach (var todo in todos)
                {
                    TodosList.Add(todo);
                }

                StatusMessage = $"Loaded {TodosList.Count} todos";
            }
            catch (ApiException ex)
            {
                StatusMessage = $"API Error: {ex.Message}";
                MessageBox.Show($"API Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void OnTodoSelected()
        {
            if (SelectedTodo != null)
            {
                TodoTitle = SelectedTodo.Title;
                TodoDescription = SelectedTodo.Description;
                TodoIsCompleted = SelectedTodo.IsCompleted;
                TodoPriority = SelectedTodo.Priority;
                TodoDueDate = SelectedTodo.DueDate;
                TodoUsername = SelectedTodo.Username;
            }
            else
            {
                ClearTodoForm();
            }
        }

        public async Task SaveCategoryAsync()
        {
            if (string.IsNullOrWhiteSpace(CategoryName))
            {
                MessageBox.Show("Please enter category name", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                IsLoading = true;
                CategoryDto savedCategory = null;

                if (SelectedCategory == null || SelectedCategory.Id == Guid.Empty)
                {
                    // YENİ KATEGORİ
                    StatusMessage = "Creating new category...";
                    var command = new CreateCategoryCommand
                    {
                        Name = CategoryName,
                        Color = CategoryColor ?? "#3498DB"
                    };

                    savedCategory = await _client.CategoryPOSTAsync(command);
                    Categories.Add(savedCategory);
                    SelectedCategory = savedCategory;

                    // ✅ Success Popup
                    ModernMessageBox.Show(
                        $"Category '{savedCategory.Name}' created successfully!",
                        "Success",
                        ModernMessageBox.MessageType.Success
                    );

                    StatusMessage = "Category created successfully";
                }
                else
                {
                    // GÜNCELLEME
                    StatusMessage = "Updating category...";
                    var command = new UpdateCategoryCommand
                    {
                        Id = SelectedCategory.Id,
                        Name = CategoryName,
                        Color = CategoryColor ?? "#3498DB"
                    };

                    savedCategory = await _client.CategoryPUTAsync(command);

                    var index = Categories.IndexOf(SelectedCategory);
                    if (index >= 0)
                    {
                        Categories[index] = savedCategory;
                        SelectedCategory = savedCategory;
                    }

                    // ✅ Success Popup
                    ModernMessageBox.Show(
                        $"Category '{savedCategory.Name}' updated successfully!",
                        "Success",
                        ModernMessageBox.MessageType.Success
                    );

                    StatusMessage = "Category updated successfully";
                }
            }
            catch (ApiException ex)
            {
                StatusMessage = "Failed to save category";

                // ✅ Error Popup
                ModernMessageBox.Show(
                    $"Failed to save category: {ex.Message}",
                    "Error",
                    ModernMessageBox.MessageType.Error
                );
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task SaveTodoAsync()
        {
            if (SelectedCategory == null || SelectedCategory.Id == Guid.Empty)
            {
                MessageBox.Show("Please select a category first", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(TodoTitle))
            {
                MessageBox.Show("Please enter todo title", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                IsLoading = true;
                TodoDto savedTodo = null;

                if (SelectedTodo == null)
                {
                    // YENİ TODO
                    StatusMessage = "Creating new todo...";
                    var command = new CreateTodoCommand
                    {
                        Title = TodoTitle,
                        Description = TodoDescription,
                        IsCompleted = TodoIsCompleted,
                        Priority = TodoPriority ?? "Normal",
                        DueDate = TodoDueDate,
                        CategoryId = SelectedCategory.Id,
                        Username = TodoUsername ?? Environment.UserName
                    };

                    savedTodo = await _client.TodoPOSTAsync(command);
                    TodosList.Insert(0, savedTodo);

                    // ✅ Success Popup
                    ModernMessageBox.Show(
                        $"Todo '{savedTodo.Title}' created successfully!",
                        "Success",
                        ModernMessageBox.MessageType.Success
                    );

                    ClearTodoForm();
                    StatusMessage = "Todo created successfully";
                }
                else
                {
                    // GÜNCELLEME
                    StatusMessage = "Updating todo...";
                    var command = new UpdateTodoCommand
                    {
                        Id = SelectedTodo.Id,
                        Title = TodoTitle,
                        Description = TodoDescription,
                        IsCompleted = TodoIsCompleted,
                        Priority = TodoPriority ?? "Normal",
                        DueDate = TodoDueDate,
                        CategoryId = SelectedCategory.Id,
                        Username = TodoUsername ?? Environment.UserName
                    };

                    savedTodo = await _client.TodoPUTAsync(command);

                    var index = TodosList.IndexOf(SelectedTodo);
                    if (index >= 0)
                    {
                        TodosList[index] = savedTodo;
                    }

                    // ✅ Success Popup
                    ModernMessageBox.Show(
                        $"Todo '{savedTodo.Title}' updated successfully!",
                        "Success",
                        ModernMessageBox.MessageType.Success
                    );

                    StatusMessage = "Todo updated successfully";
                }

                // ✅ Kaydedilen todo'yu tekrar seç
                SelectedTodo = savedTodo;
            }
            catch (ApiException ex)
            {
                StatusMessage = "Failed to save todo";

                // ✅ Error Popup
                ModernMessageBox.Show(
                    $"Failed to save todo: {ex.Message}",
                    "Error",
                    ModernMessageBox.MessageType.Error
                );
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task DeleteTodoAsync()
        {
            if (SelectedTodo == null)
            {
                MessageBox.Show("Please select a todo to delete", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete '{SelectedTodo.Title}'?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            try
            {
                IsLoading = true;
                var deletedTitle = SelectedTodo.Title;
                StatusMessage = $"Deleting '{deletedTitle}'...";

                await _client.TodoDELETEAsync(SelectedTodo.Id);
                TodosList.Remove(SelectedTodo);

                ClearTodoForm();

                // ✅ Success Popup
                ModernMessageBox.Show(
                    $"Todo '{deletedTitle}' deleted successfully!",
                    "Success",
                    ModernMessageBox.MessageType.Success
                );

                StatusMessage = "Todo deleted successfully";
            }
            catch (ApiException ex)
            {
                StatusMessage = "Failed to delete todo";

                // ✅ Error Popup
                ModernMessageBox.Show(
                    $"Failed to delete todo: {ex.Message}",
                    "Error",
                    ModernMessageBox.MessageType.Error
                );
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void ClearCategoryForm()
        {
            SelectedCategory = null;
            CategoryName = string.Empty;
            CategoryColor = "#3498DB";
        }

        public void ClearTodoForm()
        {
            SelectedTodo = null;
            TodoTitle = string.Empty;
            TodoDescription = string.Empty;
            TodoIsCompleted = false;
            TodoPriority = "Normal";
            TodoDueDate = DateTimeOffset.Now.AddDays(7);
            TodoUsername = Environment.UserName;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public async Task DeleteCategoryAsync()
        {
            if (SelectedCategory == null || SelectedCategory.Id == Guid.Empty)
            {
                MessageBox.Show("Please select a category to delete", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete category '{SelectedCategory.Name}'?\n\nAll todos in this category will also be deleted!",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            try
            {
                IsLoading = true;
                var deletedName = SelectedCategory.Name;
                StatusMessage = $"Deleting category '{deletedName}'...";

                await _client.CategoryDELETEAsync(SelectedCategory.Id);

                Categories.Remove(SelectedCategory);
                SelectedCategory = Categories.FirstOrDefault();

                // ✅ Success Popup
                ModernMessageBox.Show(
                    $"Category '{deletedName}' deleted successfully!",
                    "Success",
                    ModernMessageBox.MessageType.Success
                );

                StatusMessage = "Category deleted successfully";
            }
            catch (ApiException ex)
            {
                StatusMessage = "Failed to delete category";

                // ✅ Error Popup
                ModernMessageBox.Show(
                    $"Failed to delete category: {ex.Message}",
                    "Error",
                    ModernMessageBox.MessageType.Error
                );
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}