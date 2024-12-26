using System.ComponentModel.DataAnnotations;

namespace lab3;

public class History
{
    [Key]
    public int HistoryId { get; set; }

    [Required]
    public int ShoppingListId { get; set; } 
    
    public List<HistoryEntry> Entries { get; set; } = new List<HistoryEntry>();
    
    public void AddEntry(string description)
    {
        Entries.Add(new HistoryEntry
        {
            Timestamp = DateTime.Now,
            Description = description
        });
    }
}