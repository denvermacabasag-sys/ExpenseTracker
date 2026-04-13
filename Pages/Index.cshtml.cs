using Microsoft.AspNetCore.Mvc.RazorPages;
using ExpenseTracker.Models;
using System.Text.Json;

namespace ExpenseTracker.Pages
{
    public class IndexModel : PageModel
    {
        // This list stores all expenses (like a notebook)
        public List<Expense> Expenses { get; set; } = new();

        // Temporary fields for the form
        public string TempExpenseName { get; set; } = "";
        public decimal TempAmount { get; set; }

        // Total amount (calculated automatically)
        public decimal Total => Expenses.Sum(e => e.Amount);

        // This runs when page first loads
        public void OnGet()
        {
            LoadExpenses(); // Get expenses from "storage"
        }

        // This runs when user clicks "Add Expense" button
        public IActionResult OnPost()
        {
            // Step 1: Get data from form (like reading what user typed)
            string name = Request.Form["ExpenseName"];
            decimal amount = decimal.Parse(Request.Form["Amount"]);

            // Step 2: Create new expense (like writing in notebook)
            var newExpense = new Expense
            {
                Id = Expenses.Count + 1, // Simple ID counter
                Name = name,
                Amount = amount
            };

            // Step 3: Add to our list
            Expenses.Add(newExpense);

            // Step 4: Save to storage (so it doesn't disappear)
            SaveExpenses();

            return RedirectToPage(); // Refresh page to show new expense
        }

        // This runs when user clicks "Delete" button
        public IActionResult OnPostDelete()
        {
            // Step 1: Find which expense to delete
            int deleteId = int.Parse(Request.Form["DeleteId"]);

            // Step 2: Remove it from list (like erasing from notebook)
            var expenseToDelete = Expenses.FirstOrDefault(e => e.Id == deleteId);
            if (expenseToDelete != null)
            {
                Expenses.Remove(expenseToDelete);
            }

            // Step 3: Save updated list
            SaveExpenses();

            return RedirectToPage(); // Refresh page
        }

        // ★ TEACHER DEFENSE: Simple Storage System ★
        private void SaveExpenses()
        {
            // Convert list to text and save in a file (like saving to USB)
            string json = JsonSerializer.Serialize(Expenses, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText("expenses.json", json);
        }

        private void LoadExpenses()
        {
            // Read from file and convert back to list
            if (System.IO.File.Exists("expenses.json"))
            {
                string json = System.IO.File.ReadAllText("expenses.json");
                Expenses = JsonSerializer.Deserialize<List<Expense>>(json) ?? new List<Expense>();
            }
        }
    }
}