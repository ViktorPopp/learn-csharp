namespace TodoApp
{
    class Item
    {
        public Item(string content) { Content = content; }

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
                try
                {
                    Console.Write("Enter command: ");
                    string? command = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(command))
                    {
                        continue;
                    }
                    switch (command)
                    {
                        case "add":
                            Console.Write("Enter todo: ");
                            string? todo = Console.ReadLine();
                            Todos.Add(new Item(todo));
                            break;

                        case "list":
                            for (int i = 0; i < Todos.Count; i++)
                            {
                                Console.WriteLine($"{i} [{(Todos[i].Checked ? 'X' : ' ')}] {Todos[i].Content}");
                            }
                            break;

                        case "check":
                            {
                                Console.Write("Enter index: ");
                                string? index = Console.ReadLine();
                                if (!int.TryParse(index, out int idx))
                                {
                                    Console.WriteLine("Invalid index.");
                                }
                                else if (idx < 0 || idx >= Todos.Count)
                                {
                                    Console.WriteLine("Index out of range.");
                                }
                                else
                                {
                                    Todos[idx].Checked = true;
                                }
                            }
                            break;
                        case "remove":
                            {
                                Console.Write("Enter index: ");
                                string? index = Console.ReadLine();
                                if (!int.TryParse(index, out int idx))
                                {
                                    Console.WriteLine("Invalid index.");
                                }
                                else if (idx < 0 || idx >= Todos.Count)
                                {
                                    Console.WriteLine("Index out of range.");
                                }
                                else
                                {
                                    Todos.RemoveAt(idx);
                                }
                            }
                            break;

                        case "exit":
                            return 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex}");
                }
            }
        }
    }
}