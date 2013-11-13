using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace GetStartedWithData
{    
    public class TodoItem
    {
        public int Id { get; set; }

        //// TODO: Add the following serialization attribute.
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        //// TODO: Add the following serialization attribute.
        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }

    }

    public sealed partial class MainPage : Page
    {
		// TODO: Comment out the following line that defined the in-memory collection.
        //private ObservableCollection<TodoItem> items = new ObservableCollection<TodoItem>();

        private MobileServiceCollection<TodoItem, TodoItem> items;
        private IMobileServiceTable<TodoItem> todoTable = App.MobileService.GetTable<TodoItem>();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void InsertTodoItem(TodoItem todoItem)
        {
            // TODO: Delete or comment the following statement; Mobile Services auto-generates the ID.
            todoItem.Id = items.Count == 0 ? 0 : items.Max(i => i.Id) + 1;

            await todoTable.InsertAsync(todoItem);
             
            //items.Add(todoItem);
        }

        private async void RefreshTodoItems()
        {
            items = await todoTable.ToCollectionAsync();

            //// TODO #2: More advanced query that filters out completed items. 
            //items = await todoTable
            //   .Where(todoItem => todoItem.Complete == false)
            //   .ToCollectionAsync();
           
            ListItems.ItemsSource = items;
        }

        private async void UpdateCheckedTodoItem(TodoItem item)
        {
            await todoTable.UpdateAsync(item);     
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshTodoItems();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var todoItem = new TodoItem { Text = TextInput.Text };
            InsertTodoItem(todoItem);
        }

        private void CheckBoxComplete_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            TodoItem item = cb.DataContext as TodoItem;
            UpdateCheckedTodoItem(item);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            RefreshTodoItems();
        }
    }
}
