using Microsoft.EntityFrameworkCore;

namespace ExpenseAPI.Models
{
    public class ExpenseContext : DbContext
    {
        // public ExpenseContext(DbContextOptions<ExpenseContext> options)
        //     : base(options)
        // {
        // }

        // public DbSet<Expense> Expenses { get; set; }

        // protected override void OnConfiguring(DbContextOptionsBuilder options)
        //     => options.UseInMemoryDatabase("ExpenseDatabase");
        public ExpenseContext(DbContextOptions<ExpenseContext> options)
            : base(options)
        {
        }

        public DbSet<Expense> Expenses { get; set; }
    }
}
