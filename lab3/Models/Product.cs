using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace lab3
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Category { get; set; }
        public bool IsPurchased { get; set; }
        public DateTime PurchaseDate { get; set; }
        
        [JsonIgnore]
        public int ShoppingListId { get; set; }
        
        //[JsonIgnore]
        public ShoppingList ShoppingList { get; set; }
        
        public Product() { }

        public Product(string name, string category)
        {
            Name = name;
            Category = category;
            IsPurchased = false;
        }
    }
}