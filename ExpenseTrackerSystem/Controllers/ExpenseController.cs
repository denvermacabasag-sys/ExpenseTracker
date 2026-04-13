using Microsoft.AspNetCore.Mvc;
using ExpenseTrackerSystem.Models;

namespace ExpenseTrackerSystem.Controllers;

public class ExpenseController : Controller
{
    private static List<Expense> _expenses = new List<Expense>();
    private static int _nextId = 1;

    public IActionResult Index(string? categoryFilter)
    {
        var expenses = _expenses.AsQueryable();

        if (!string.IsNullOrEmpty(categoryFilter))
        {
            expenses = expenses.Where(e => e.Category == categoryFilter);
        }

        var viewModel = new ExpenseListViewModel
        {
            Expenses = expenses.OrderByDescending(e => e.Date).ToList(),
            Categories = _expenses.Select(e => e.Category).Distinct().ToList(),
            SelectedCategory = categoryFilter,
            TotalAmount = _expenses.Sum(e => e.Amount)
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Expense expense)
    {
        if (ModelState.IsValid)
        {
            expense.Id = _nextId++;
            _expenses.Add(expense);
            return RedirectToAction(nameof(Index));
        }

        var viewModel = new ExpenseListViewModel
        {
            Expenses = _expenses.OrderByDescending(e => e.Date).ToList(),
            Categories = _expenses.Select(e => e.Category).Distinct().ToList(),
            TotalAmount = _expenses.Sum(e => e.Amount)
        };
        return View("Index", viewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var expense = _expenses.FirstOrDefault(e => e.Id == id);
        if (expense == null) return NotFound();

        return View(expense);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Expense expense)
    {
        if (ModelState.IsValid)
        {
            var existing = _expenses.FirstOrDefault(e => e.Id == expense.Id);
            if (existing != null)
            {
                existing.Description = expense.Description;
                existing.Amount = expense.Amount;
                existing.Category = expense.Category;
                existing.Date = expense.Date;
                existing.Notes = expense.Notes;
            }
            return RedirectToAction(nameof(Index));
        }

        return View(expense);
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var expense = _expenses.FirstOrDefault(e => e.Id == id);
        if (expense == null) return NotFound();

        return View(expense);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var expense = _expenses.FirstOrDefault(e => e.Id == id);
        if (expense != null)
        {
            _expenses.Remove(expense);
        }
        return RedirectToAction(nameof(Index));
    }
}