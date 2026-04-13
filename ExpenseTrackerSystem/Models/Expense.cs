using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerSystem.Models;

public class Expense
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    [Required]
    public string Category { get; set; } = string.Empty;

    [Required]
    public DateTime Date { get; set; } = DateTime.Now;

    public string? Notes { get; set; }
}

public class ExpenseListViewModel
{
    public List<Expense> Expenses { get; set; } = new List<Expense>();
    public List<string> Categories { get; set; } = new List<string>();
    public string? SelectedCategory { get; set; }
    public decimal TotalAmount { get; set; }
}