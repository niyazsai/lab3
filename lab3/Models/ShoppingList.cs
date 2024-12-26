using System.ComponentModel.DataAnnotations;

namespace lab3
{ 
    public class ShoppingList
    {
        [Key]
        public int ShoppingListId { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
        
        public History History { get; set; } = new History();

        public ShoppingList() { }

        public ShoppingList(string name)
        {
            Name = name;
            History = new History { ShoppingListId = this.ShoppingListId };
        }

        public void AddProduct(Product product)
        {
            product.ShoppingList = this;
            Products.Add(product);
            History.AddEntry($"Добавлен товар '{product.Name}'");
        }

        public void RemoveProduct(int index)
        {
            if (index >= 0 && index < Products.Count)
            {
                var product = Products.ElementAt(index);
                Products.Remove(product);
                History.AddEntry($"Удален товар '{product.Name}'");
            }
        }

        public void MarkAsPurchased(int index)
        {
            if (index >= 0 && index < Products.Count)
            {
                var product = Products[index];
                product.IsPurchased = true;
                product.PurchaseDate = DateTime.Now;
                History.AddEntry($"Товар '{product.Name}' отмечен как купленный");
            }
        }
    }
}