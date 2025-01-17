using Microsoft.AspNetCore.Mvc;

namespace lab3
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingListController : ControllerBase
    {
        private readonly ShoppingListRepository _repository;

        public ShoppingListController(ShoppingListRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingList>>> GetAllShoppingLists()
        {
            var shoppingLists = await _repository.GetAllShoppingListsAsync();
            return Ok(shoppingLists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingList>> GetShoppingList(int id)
        {
            var shoppingList = await _repository.GetShoppingListByIdAsync(id);
            if (shoppingList == null)
            {
                return NotFound();
            }

            return Ok(shoppingList);
        }

        [HttpPost]
        public async Task<ActionResult> CreateShoppingList([FromBody] ShoppingList shoppingList)
        {
            foreach (var product in shoppingList.Products)
            {
                product.ShoppingList = shoppingList;
            }

            await _repository.AddShoppingListAsync(shoppingList);
            return CreatedAtAction(nameof(GetShoppingList), new { id = shoppingList.ShoppingListId }, shoppingList);
        }

        //https://www.ozon.ru/product/nabor-konteynerov-dlya-produktov-s-vintovoy-kryshkoy-twist-5-sht-1712078037/?avtc=1&avte=4&avts=1735282166

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateShoppingList([FromRoute]int id, [FromBody] ShoppingList updatedList)
        {
            var existingList = await _repository.GetShoppingListByIdAsync(id);
            if (existingList == null)
            {
                return NotFound();
            }

            existingList.Name = updatedList.Name;
            existingList.Products = updatedList.Products;

            await _repository.UpdateShoppingListAsync(existingList);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteShoppingList(int id)
        {
            var shoppingList = await _repository.GetShoppingListByIdAsync(id);
            if (shoppingList == null)
            {
                return NotFound();
            }

            await _repository.DeleteShoppingListAsync(shoppingList);
            return NoContent();
        }
    }
}