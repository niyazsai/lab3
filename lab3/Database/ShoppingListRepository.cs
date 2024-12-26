using Microsoft.EntityFrameworkCore;

namespace lab3
{
    public class ShoppingListRepository
    {
        private readonly ShoppingListDbContext _context;

        public ShoppingListRepository(ShoppingListDbContext context)
        {
            _context = context;
        }

        public async Task<List<ShoppingList>> GetAllShoppingListsAsync()
        {
            return await _context.ShoppingLists.Include(s => s.Products).ToListAsync();
        }

        public async Task<ShoppingList> GetShoppingListByIdAsync(int id)
        {
            return await _context.ShoppingLists.Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.ShoppingListId == id);
        }

        public async Task AddShoppingListAsync(ShoppingList shoppingList)
        {
            await _context.ShoppingLists.AddAsync(shoppingList);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateShoppingListAsync(ShoppingList shoppingList)
        {
            _context.ShoppingLists.Update(shoppingList);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteShoppingListAsync(ShoppingList shoppingList)
        {
            _context.ShoppingLists.Remove(shoppingList);
            await _context.SaveChangesAsync();
        }
    }
}