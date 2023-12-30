// 這個是用來代表一筆支出的 Entity
// 這個 Entity 會對應到資料庫中的一個 Table
// 這個 Entity 會被 Entity Framework Core 用來產生資料庫的 Table
// 這個 Entity 有幾個欄位，分別是 Id, Date, Description, Amount
// Id 是一個整數，是這筆支出的唯一識別碼
// Date 是一個日期，是這筆支出的日期
// Description 是一個字串，是這筆支出的說明
// Amount 是一個浮點數，是這筆支出的金額
// Category 是一個字串，是這筆支出的類別

namespace ExpenseAPI.Models
{
    // public record Expense(int Id, DateOnly Date, string Description, decimal Amount);
    public class Expense
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
    }
}