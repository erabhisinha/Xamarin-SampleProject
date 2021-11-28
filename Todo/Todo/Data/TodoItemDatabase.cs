using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using SQLitePCL;
using Todo.Models;
using Xamarin.Forms;

namespace Todo.Data
{
    public class TodoItemDatabase
    {
        static SQLiteConnection Database;

        public static readonly AsyncLazy<TodoItemDatabase> Instance = new AsyncLazy<TodoItemDatabase>(async () =>
        {
            return await Task.Run(() =>
            {
                var instance = new TodoItemDatabase();
                /*
                // check if the db is ready
                while (true)
                {
                    var status = Database.ExecuteScalar<string>("pragma sync_status");
                    if (status.Contains("\"db_is_ready\": true")) break;
                    System.Threading.Thread.Sleep(250);
                }
                */
                return instance;
            });
        });

        public TodoItemDatabase()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                // use the OctoDB static native library
                SQLitePCL.raw.SetProvider(new SQLite3Provider_static());
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                // load the LiteSync dynamic native library
                SQLitePCL.raw.SetProvider(new SQLite3Provider_LiteSync());
            }

            Database = new SQLiteConnection(Constants.DatabasePath, Constants.Flags);
        }

        public bool IsReady()
        {
            return Database.IsReady();
        }

        public void OnReady(System.Action Callback)
        {
            Database.OnReady(Callback);
        }

        public void OnSync(System.Action Callback)
        {
            Database.OnSync(Callback);
        }

        public void Execute(string command)
        {
            Database.ExecuteScalar<string>(command);
        }

        public List<TodoItem> GetItems()
        {
            return Database.Table<TodoItem>().ToList();
        }

        public List<TodoItem> GetItemsNotDone()
        {
            return Database.Query<TodoItem>("SELECT * FROM tasks WHERE done = 0");
        }

        public TodoItem GetItem(int id)
        {
            return Database.Table<TodoItem>().Where(i => i.ID == id).FirstOrDefault();  // FirstOrDefaultAsync();
        }

        public int SaveItem(TodoItem item)
        {
            if (item.ID != 0)
            {
                return Database.Update(item);
            }
            else
            {
                var allData = Database.Table<TodoItem>().ToList();
                if(allData?.Count>0)
                {
                    item.ID = allData.Max(x=>x.ID)+1;
                }
                else
                {
                    item.ID = 1;
                }
                return Database.Insert(item);
            }
        }

        public int DeleteItem(TodoItem item)
        {
            return Database.Delete(item);
        }

        public void CreateTable()
        {
            Database.CreateTable<TodoItem>();
        }
    }
}
