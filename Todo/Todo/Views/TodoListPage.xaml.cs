using System;
using Todo.Data;
using Todo.Models;
using Xamarin.Forms;

namespace Todo.Views
{
    public partial class TodoListPage : ContentPage
    {
        public TodoListPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            TodoItemDatabase database = await TodoItemDatabase.Instance;
            // check if the database is ready
            if (database.IsReady())
            {
                database.CreateTable();
                // the user is already logged in. show the main screen
                listView.ItemsSource = database.GetItems();
            }
            else
            {
                database.CreateTable();
                // wait until the db is ready (and do not access the database)
                database.OnReady(() => {

                    // The database is ready for access
                    listView.ItemsSource = database.GetItems();
                });
            }

            database.OnSync(() => {
                // the db received an update. update the screen with new data
                listView.ItemsSource = database.GetItems();
            });
        }

        async void OnItemAdded(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TodoItemPage
            {
                BindingContext = new TodoItem()
            });
        }

        async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new TodoItemPage
                {
                    BindingContext = e.SelectedItem as TodoItem
                });
            }
        }
    }
}
