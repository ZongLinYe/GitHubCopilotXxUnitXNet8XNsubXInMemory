// 使用 ExpenseContext 來產生預設資料


using ExpenseAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseAPI
{
    public static class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ExpenseContext(
                serviceProvider.GetRequiredService<DbContextOptions<ExpenseContext>>()))
            {
                context.Database.EnsureCreated();

                // Look for any expenses.
                if (context.Expenses.Any())
                {
                    return;   // Data was already seeded
                }
          
                var expenses = new List<Expense>
                {
                    new Expense
                    {
                        Date = DateTime.Parse("2021-01-01"),
                        Description = "買早餐",
                        Amount = 50,
                        Category = "飲食"
                    },
                    new Expense
                    {
                        Date = DateTime.Parse("2021-01-02"),
                        Description = "買午餐",
                        Amount = 100,
                        Category = "飲食"
                    },
                    new Expense
                    {
                        Date = DateTime.Parse("2021-01-03"),
                        Description = "買晚餐",
                        Amount = 150,
                        Category = "飲食"
                    },
                };

                context.Expenses.AddRange(expenses);
                context.SaveChanges();
            }
        }


    }

}