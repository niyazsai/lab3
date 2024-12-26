namespace lab3
{
    public class CommandHandler
    {
        private readonly ShoppingListRepository _repository;

        public CommandHandler(ShoppingListRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateNewListAsync()
        {
            //Console.Clear();
            string name = InputValidator.GetNonEmptyString("Введите название нового списка: ");
            var shoppingList = new ShoppingList(name);
            Console.WriteLine("Добавьте товары в список. Введите 'готово' для завершения.");
            while (true)
            {
                string productName = InputValidator.GetNonEmptyString("Товар: ");
                if (productName.ToLower() == "готово")
                {
                    break;
                }

                Console.Write("Категория товара: ");
                string category = Console.ReadLine();

                var product = new Product(productName, category)
                {
                    ShoppingListId = shoppingList.ShoppingListId
                };

                shoppingList.AddProduct(product);
            }
            await _repository.AddShoppingListAsync(shoppingList);
            Console.WriteLine("Новый список создан. Нажмите любую клавишу для продолжения.");
            Console.ReadLine();
        }

        public async Task ViewListsAsync()
        {
            //Console.Clear();
            var shoppingLists = await _repository.GetAllShoppingListsAsync();
            if (shoppingLists.Count == 0)
            {
                Console.WriteLine("Нет доступных списков.");
                Console.WriteLine("Нажмите любую клавишу для продолжения.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Доступные списки:");
            for (int i = 0; i < shoppingLists.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {shoppingLists[i].Name}");
            }

            int choice = InputValidator.GetValidatedInt("Выберите список для управления или 0 для возврата: ", 0, shoppingLists.Count);
            if (choice == 0)
            {
                return;
            }

            var selectedList = shoppingLists[choice - 1];
            await ManageListAsync(selectedList);
        }

        private async Task ManageListAsync(ShoppingList shoppingList)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"== {shoppingList.Name} ==");
                Console.WriteLine("1. Просмотреть товары");
                Console.WriteLine("2. Отметить покупку");
                Console.WriteLine("3. Добавить товары");
                Console.WriteLine("4. Удалить товар");
                Console.WriteLine("5. Посмотреть историю");
                Console.WriteLine("6. Назад");

                int choice = InputValidator.GetValidatedInt("Выберите действие: ", 1, 6);

                switch (choice)
                {
                    case 1:
                        ViewProducts(shoppingList);
                        break;
                    case 2:
                        await MarkPurchaseAsync(shoppingList);
                        return;
                    case 3:
                        await EditListAsync(shoppingList);
                        break;
                    case 4:
                        await RemoveProductAsync(shoppingList);
                        break;
                    case 5:
                        ViewHistory(shoppingList);
                        break;
                    case 6:
                        return;
                }
            }
        }

        private async Task EditListAsync(ShoppingList shoppingList)
        {
            //Console.Clear();
            Console.WriteLine("Добавьте товары в список. Введите 'готово' для завершения.");
            while (true)
            {
                string productName = InputValidator.GetNonEmptyString("Товар: ");
                if (productName.ToLower() == "готово")
                {
                    break;
                }

                Console.Write("Категория товара: ");
                string category = Console.ReadLine();

                var product = new Product(productName, category)
                {
                    ShoppingListId = shoppingList.ShoppingListId
                };

                shoppingList.AddProduct(product);
            }
            await _repository.UpdateShoppingListAsync(shoppingList);
            Console.WriteLine("Список успешно обновлен. Нажмите любую клавишу для продолжения.");
            Console.ReadLine();
        }

        private void ViewProducts(ShoppingList shoppingList)
        {
            //Console.Clear();
            if (shoppingList.Products.Count == 0)
            {
                Console.WriteLine("Список товаров пуст.");
            }
            else
            {
                Console.WriteLine("Товары в списке:");
                foreach (var product in shoppingList.Products)
                {
                    string status = product.IsPurchased ? "(Куплено)" : "(Не куплено)";
                    Console.WriteLine($"- {product.Name} [{product.Category}] {status}");
                }
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения.");
            Console.ReadLine();
        }
        
        private async Task MarkPurchaseAsync(ShoppingList shoppingList)
        {
            //Console.Clear();
            Console.WriteLine("Товары в списке:");
            for (int i = 0; i < shoppingList.Products.Count; i++)
            {
                var product = shoppingList.Products[i];
                string status = product.IsPurchased ? "(Куплено)" : "(Не куплено)";
                Console.WriteLine($"{i + 1}. {product.Name} [{product.Category}] {status}");
            }

            int choice = InputValidator.GetValidatedInt("Выберите номер товара для отметки: ", 1, shoppingList.Products.Count);
            shoppingList.MarkAsPurchased(choice - 1);
            await _repository.UpdateShoppingListAsync(shoppingList);
            Console.WriteLine("Товар успешно отмечен как купленный. Нажмите любую клавишу для продолжения.");
            Console.ReadLine();
        }

        private async Task RemoveProductAsync(ShoppingList shoppingList)
        {
            //Console.Clear();
            if (shoppingList.Products.Count == 0)
            {
                Console.WriteLine("Список товаров пуст.");
                Console.WriteLine("Нажмите любую клавишу для продолжения.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Товары в списке:");
            var products = shoppingList.Products.ToList();
            for (int i = 0; i < products.Count; i++)
            {
                var product = products[i];

                Console.WriteLine($"{i + 1}. {product.Name} [{product.Category}]");
            }

            int choice = InputValidator.GetValidatedInt("Выберите номер товара для удаления: ", 1, products.Count);
            shoppingList.RemoveProduct(choice - 1);
            await _repository.UpdateShoppingListAsync(shoppingList);

            Console.WriteLine("Товар успешно удален. Нажмите любую клавишу для продолжения.");
            Console.ReadLine();
        }
        
        private void ViewHistory(ShoppingList shoppingList)
        {
            //Console.Clear();
            if (shoppingList.History.Entries.Count == 0)
            {
                Console.WriteLine("История изменений пуста.");
            }
            else
            {
                Console.WriteLine("История изменений:");
                foreach (var entry in shoppingList.History.Entries)
                {
                    Console.WriteLine($"{entry.Timestamp}: {entry.Description}");
                }
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения.");
            Console.ReadLine();
        }
    }
}
