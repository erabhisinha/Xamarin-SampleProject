using System;
using System.IO;

namespace Todo
{
    public static class Constants
    {
        public const string DatabaseFilename = "Todo.db";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create;

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var filePath = Path.Combine(basePath, DatabaseFilename);
                //return "file:" + filePath + "?node=secondary&connect=tcp://192.168.1.45:1234";
                //return "file:" + filePath + "?node=secondary&connect=tcp://192.168.1.45:1234";
                return "file:" + filePath + "?node=primary&connect=tcp://192.168.1.16:1234";
            }
        }
    }
}
