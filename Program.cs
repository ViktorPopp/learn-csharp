using Microsoft.Data.Sqlite;

// This code is not very good, so please, spare yourself and do not read it.

namespace TodoApp
{
    class Program
    {
        private static SqliteConnection Connection = new SqliteConnection("Data Source=todo.db");

        public static int Main()
        {
            Initialize();
            while (true)
            {
                var input = InputString("Enter command: ");
                switch (input)
                {
                    case "add":
                        AddItem();
                        break;

                    case "ls":
                    case "list":
                        ListItems();
                        break;

                    case "check":
                        CheckItem();
                        break;

                    case "rm":
                    case "remove":
                        RemoveItem();
                        break;

                    case "exit":
                        Exit(0);
                        break;

                    default:
                        Console.WriteLine("Valid commands are 'add', 'list', 'check', 'remove' and 'exit'");
                        break;
                }
            }
        }

        private static void Initialize()
        {
            Connection.Open();
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = """
            CREATE TABLE IF NOT EXISTS todos (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                content TEXT NOT NULL,
                checked BOOLEAN
            );
            """;
            cmd.ExecuteNonQuery();
        }

        private static void Exit(int error)
        {
            Connection.Dispose();
            Environment.Exit(error);
        }

        public static void ListItems()
        {
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = """
            SELECT * from todos
            """;
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var content = reader.GetString(1);
                var checkedFlag = reader.GetBoolean(2);

                Console.WriteLine($"{id} [{(checkedFlag ? 'X' : ' ')}] {content}");
            }

        }

        public static void AddItem()
        {
            var content = InputString("Enter content: ");
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = """
            INSERT INTO todos (content, checked)
            VALUES ($content, FALSE);
            """;
            cmd.Parameters.AddWithValue("$content", content);
            cmd.ExecuteNonQuery();
        }

        public static void CheckItem()
        {
            var input = InputString("Enter index: ");
            if (!int.TryParse(input, out int index))
            {
                Console.WriteLine("Error: Invalid index");
            }
            else
            {
                using var cmd = Connection.CreateCommand();
                cmd.CommandText = "UPDATE todos SET checked = TRUE WHERE id = $id";
                cmd.Parameters.AddWithValue("$id", index);
                int affected = cmd.ExecuteNonQuery();
                if (affected == 0)
                {
                    Console.WriteLine("Error: Could not check todo");
                }
            }

        }

        public static void RemoveItem()
        {
            var input = InputString("Enter index: ");
            if (!int.TryParse(input, out int index))
            {
                Console.WriteLine("Error: Invalid index");
            }
            using var cmd = Connection.CreateCommand();
            cmd.CommandText = "DELETE FROM todos WHERE id = $id";
            cmd.Parameters.AddWithValue("$id", index);

            int affected = cmd.ExecuteNonQuery();
            if (affected == 0)
            {
                Console.WriteLine("Error: Could not delete todo");
            }
        }

        private static string InputString(string message)
        {
            Console.Write(message);
            var input = Console.ReadLine();
            return string.IsNullOrEmpty(input) ? "" : input;
        }
    }
}