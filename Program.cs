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
        private static List<Item> Todos = new List<Item>();

        public static int Main()
        {
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
                        return 0;

                    default:
                        Console.WriteLine("Valid commands are 'add', 'list', 'check', 'remove' and 'exit'");
                        break;
                }
            }
        }

        public static void ListItems()
        {
            for (int i = 0; i < Todos.Count; i++)
            {
                var item = Todos[i];
                Console.WriteLine($"{i} [{(item.Checked ? 'X' : ' ')}] {item.Content}");
            }
        }

        public static void AddItem()
        {
            var content = InputString("Enter content: ");
            Todos.Add(new Item(content));
        }

        public static void CheckItem()
        {
            var input = InputString("Enter index: ");
            if (!int.TryParse(input, out int index))
            {
                Console.WriteLine("Error: Invalid index");
            }
            else if (index < 0 || index >= Todos.Count)
            {
                Console.WriteLine("Error: Index out of range");
            }
            else
            {
                Todos[index].Checked = true;
            }
        }

        public static void RemoveItem()
        {
            var input = InputString("Enter index: ");
            if (!int.TryParse(input, out int index))
            {
                Console.WriteLine("Error: Invalid index");
            }
            else if (index < 0 || index >= Todos.Count)
            {
                Console.WriteLine("Error: Index out of range");
            }
            else
            {
                Todos.RemoveAt(index);
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