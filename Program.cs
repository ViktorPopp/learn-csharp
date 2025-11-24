using Microsoft.Data.Sqlite;

namespace TodoApp
{
    class Item
    {
        public Item(string content) => Content = content;

        public string Content { get; set; }
        public bool Checked { get; set; }
    }

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

                    case "list":
                        ListItems();
                        break;

                    case "check":
                        CheckItem();
                        break;

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
            var cmd = Connection.CreateCommand();
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
            var cmd = Connection.CreateCommand();
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
            var cmd = Connection.CreateCommand();
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
                cmd.CommandText = "UPDATE todos SET checked = 1 WHERE id = $id";
                cmd.Parameters.AddWithValue("$id", index);
                cmd.ExecuteNonQuery();
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