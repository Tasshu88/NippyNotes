using System;
using System.Data.SQLite;

namespace Nippy_Notes
{
    public static class Logger
    {
   //     private static readonly string connectionString = "Data Source=NippyDB.db;Version=3;";

        static Logger()
        {
            DatabaseHelper.EnsureActivityLogTable();
        }

        public static void LogActivity(string actionType, string details)
        {
            DatabaseHelper.LogActivity(actionType, details);
        }
    }
}
