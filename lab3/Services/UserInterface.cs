namespace lab3
{
    public class UserInterface
    {
        private readonly CommandHandler _commandHandler;

        public UserInterface(CommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public async Task MainMenuAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Меню ===");
                Console.WriteLine("1. Создать новый список");
                Console.WriteLine("2. Просмотреть текущие списки");
                Console.WriteLine("3. Выйти");

                int choice = InputValidator.GetValidatedInt("Выберите действие: ", 1, 3);

                switch (choice)
                {
                    case 1:
                        await _commandHandler.CreateNewListAsync();
                        break;
                    case 2:
                        await _commandHandler.ViewListsAsync();
                        break;
                    case 3:
                        return;
                }
            }
        }
    }
}