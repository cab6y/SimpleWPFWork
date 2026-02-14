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
                _selectedCategory = value;
                OnPropertyChanged();
                OnCategorySelected();
            }
        }

        private string _categoryName;
        public string CategoryName
        {
            get => _categoryName;
            set { _categoryName = value; OnPropertyChanged(); }
        }

        private string _categoryColor;
        public string CategoryColor
        {
            get => _categoryColor;
            set { _categoryColor = value; OnPropertyChanged(); }
        }

        // Todos
        public ObservableCollection<TodoDto> TodosList { get; set; }
        private TodoDto _selectedTodo;
        public TodoDto SelectedTodo
        {
            get => _selectedTodo;
            set
            {
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
            set { _todoTitle = value; OnPropertyChanged(); }
        }

        private string _todoDescription;
        public string TodoDescription
        {
            get => _todoDescription;
            set { _todoDescription = value; OnPropertyChanged(); }
        }

        private bool _todoIsCompleted;
        public bool TodoIsCompleted
        {
            get => _todoIsCompleted;
            set { _todoIsCompleted = value; OnPropertyChanged(); }
        }

        private string _todoPriority;
        public string TodoPriority
        {
            get => _todoPriority;
            set { _todoPriority = value; OnPropertyChanged(); }
        }

        private DateTimeOffset _todoDueDate;
        public DateTimeOffset TodoDueDate
        {
            get => _todoDueDate;
            set { _todoDueDate = value; OnPropertyChanged(); }
        }

        private string _todoUsername;
        public string TodoUsername
        {
            get => _todoUsername;
            set { _todoUsername = value; OnPropertyChanged(); }
        }

        // UI States
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        private bool _isTodoFormEnabled;
        public bool IsTodoFormEnabled
        {
            get => _isTodoFormEnabled;
            set { _isTodoFormEnabled = value; OnPropertyChanged(); }
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

                // API: CategoryAllAsync(string name, string color, int? limit, int? page)
                var categories = await _client.CategoryAllAsync(null, null, 100, 0);
                Categories.Clear();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }

                StatusMessage = $"Loaded {Categories.Count} categories";
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
            if (SelectedCategory != null)
            {
                // Kategori seçildi - bilgilerini forma aktar
                CategoryName = SelectedCategory.Name;
                CategoryColor = SelectedCategory.Color;

                // Todo listesini yükle
                await LoadTodosByCategoryAsync(SelectedCategory.Id);
                IsTodoFormEnabled = true;
            }
            else
            {
                // Kategori seçimi kaldırıldı
                TodosList.Clear();
                IsTodoFormEnabled = false;
                ClearTodoForm();
            }
        }

        private async Task LoadTodosByCategoryAsync(Guid categoryId)
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Loading todos...";

                // API: TodoAllAsync(string title, string description, bool? isCompleted, 
                //                   string priority, DateTimeOffset? dueDate, Guid? categoryId, 
                //                   string username, int? limit, int? page)
                var todos = await _client.TodoAllAsync(
                    title: null,
                    description: null,
                    isCompleted: null,
                    priority: null,
                    dueDate: null,
                    categoryId: categoryId,  // ✅ GUID olarak gönderiliyor
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
                // Todo seçildi - bilgilerini forma aktar
                TodoTitle = SelectedTodo.Title;
                TodoDescription = SelectedTodo.Description;
                TodoIsCompleted = SelectedTodo.IsCompleted;
                TodoPriority = SelectedTodo.Priority;
                TodoDueDate = SelectedTodo.DueDate;
                TodoUsername = SelectedTodo.Username;
            }
            else
            {
                // Todo seçimi kaldırıldı - formu temizle
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

                if (SelectedCategory == null)
                {
                    // YENİ KATEGORİ
                    StatusMessage = "Creating new category...";
                    var command = new CreateCategoryCommand
                    {
                        Name = CategoryName,
                        Color = CategoryColor ?? "#3498DB"
                    };

                    var newCategory = await _client.CategoryPOSTAsync(command);
                    Categories.Add(newCategory);
                    SelectedCategory = newCategory;

                    StatusMessage = "Category created successfully";
                }
                else
                {
                    // GÜNCELLEME
                    StatusMessage = "Updating category...";
                    var command = new UpdateCategoryCommand
                    {
                        Id = SelectedCategory.Id,  // ✅ GUID
                        Name = CategoryName,
                        Color = CategoryColor ?? "#3498DB"
                    };

                    var updatedCategory = await _client.CategoryPUTAsync(command);

                    // Listedeki kategoriyi güncelle
                    var index = Categories.IndexOf(SelectedCategory);
                    if (index >= 0)
                    {
                        Categories[index] = updatedCategory;
                        SelectedCategory = updatedCategory;
                    }

                    StatusMessage = "Category updated successfully";
                }
            }
            catch (ApiException ex)
            {
                StatusMessage = "Failed to save category";
                MessageBox.Show($"API Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task SaveTodoAsync()
        {
            if (SelectedCategory == null)
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
                        CategoryId = SelectedCategory.Id,  // ✅ GUID
                        Username = TodoUsername ?? Environment.UserName
                    };

                    var newTodo = await _client.TodoPOSTAsync(command);
                    TodosList.Insert(0, newTodo);

                    ClearTodoForm();
                    StatusMessage = "Todo created successfully";
                }
                else
                {
                    // GÜNCELLEME
                    StatusMessage = "Updating todo...";
                    var command = new UpdateTodoCommand
                    {
                        Id = SelectedTodo.Id,  // ✅ GUID
                        Title = TodoTitle,
                        Description = TodoDescription,
                        IsCompleted = TodoIsCompleted,
                        Priority = TodoPriority ?? "Normal",
                        DueDate = TodoDueDate,
                        CategoryId = SelectedCategory.Id,  // ✅ GUID
                        Username = TodoUsername ?? Environment.UserName
                    };

                    var updatedTodo = await _client.TodoPUTAsync(command);

                    // Listedeki todo'yu güncelle
                    var index = TodosList.IndexOf(SelectedTodo);
                    if (index >= 0)
                    {
                        TodosList[index] = updatedTodo;
                    }

                    StatusMessage = "Todo updated successfully";
                }
            }
            catch (ApiException ex)
            {
                StatusMessage = "Failed to save todo";
                MessageBox.Show($"API Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                StatusMessage = $"Deleting '{SelectedTodo.Title}'...";

                await _client.TodoDELETEAsync(SelectedTodo.Id);  // ✅ GUID
                TodosList.Remove(SelectedTodo);

                ClearTodoForm();
                StatusMessage = "Todo deleted successfully";
            }
            catch (ApiException ex)
            {
                StatusMessage = "Failed to delete todo";
                MessageBox.Show($"API Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch
            {

            }
        }
    }
}