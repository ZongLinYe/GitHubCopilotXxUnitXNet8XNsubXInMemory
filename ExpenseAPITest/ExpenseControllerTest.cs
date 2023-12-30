using Microsoft.EntityFrameworkCore; // Add this using directive
using ExpenseAPI.Models;
using Microsoft.Extensions.Logging;
using ExpenseAPI.Controllers; // Add this using directive
using System.Threading.Tasks;
using NSubstitute;


// Copilot Chat Response:
// dotnet add ExpenseControllerTest.csproj reference ../OtherProject/OtherProject.csproj
// dotnet add ExpenseControllerTest.csproj package Newtonsoft.Json
// dotnet add ExpenseAPITest.csproj reference ../ExpenseAPI/ExpenseAPI.csproj


namespace ExpenseAPITest;

// 這是用來測試 ExpenseController 的類別
// 只需要測試 POST 方法
// 使用 AAA 模式來撰寫測試
// 至少要測試三種情境，一個正向，一個反向，一個邊界
public class ExpenseControllerTest
{
    // 準備 EF Core In Memory Database
    // 準備 ExpenseContext
    private readonly ExpenseContext _context;
    private readonly ExpenseController _controller;
    public ExpenseControllerTest()
    {
        var options = new DbContextOptionsBuilder<ExpenseContext>()
        .UseInMemoryDatabase(databaseName: "ExpenseDatabase").Options;
        _context = new ExpenseContext(options);
        _controller = new ExpenseController(_context, Substitute.For<ILogger<ExpenseController>>());
    }

    // 測試正向情境
    [Fact]
    public async Task PostExpense_ValidExpense_ReturnsCreatedAtAction()
    {
        // Arrange
        var newExpense = new Expense
        {
            Date = new DateTime(2021, 1, 1),
            Description = "買早餐",
            Amount = 50,
            Category = "飲食"
        };
        // Act
        var result = await _controller.PostExpense(newExpense);
        // Assert
        var actionResult = Assert.IsType<ActionResult<Exception>>(result);
        var okResult = Assert.IsType<OkResult>(actionResult.Result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task PostExpense_NullDescription_ReturnsBadRequest()
    {
        // Arrange
        var newExpense = new Expense
        {
            Date = new DateTime(2021, 1, 1),
            Description = null,
            Amount = 50,
            Category = "飲食"
        };
        // Act
        var result = await _controller.PostExpense(newExpense);
        // Assert
        // return 類別 ActionResult<Exception>
        var actionResult = Assert.IsType<ActionResult<Exception>>(result);
        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        Assert.Equal(400, (actionResult.Result as BadRequestObjectResult).StatusCode);
        Assert.Equal("說明錯誤", (actionResult.Result as BadRequestObjectResult).Value);
    }

    [Fact]
    public async Task PostExpense_ShouldReturnOk_WhenInputValidExpense()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ExpenseContext>()
            .UseInMemoryDatabase(databaseName: "ExpenseDatabase")
            .Options;
        var context = new ExpenseContext(options);
        var logger = Substitute.For<ILogger<ExpenseController>>();
        var controller = new ExpenseController(context, logger);
        var expense = new Expense
        {
            Date = new DateTime(2021, 1, 1),
            Description = "買早餐",
            Amount = 50,
            Category = "飲食"
        };

        // Act
        var result = await controller.PostExpense(expense);

        // Assert
        Assert.IsType<ActionResult<Exception>>(result);
    }

    [Fact]
    public async Task PostExpenses_ValidExpenses_ReturnsOkResult()
    {
        // Arrange
        var newExpenses = new List<Expense>
        {
            new Expense
            {
                Date = new DateTime(2021, 1, 1),
                Description = "買早餐",
                Amount = 50,
                Category = "飲食"
            },
            new Expense
            {
                Date = new DateTime(2021, 1, 2),
                Description = "買午餐",
                Amount = 60,
                Category = "飲食"
            }
        };

        // Act
        var result = await _controller.PostExpenses(newExpenses);

        // Assert
        // return 類別 IActionResult
        var okResult = Assert.IsType<OkObjectResult>(result);
        // 回傳匿名物件需要額外處理
        // https://www.malema.net/csharp-unit-test/xunit/anonymous-type.html
        dynamic returnValue = okResult.Value.ToExpandoObject();
        Assert.Equal(200, okResult.StatusCode);
        Assert.True((bool)returnValue.succuss);
        Assert.Equal("新增成功", (string)returnValue.message);
        Assert.Equal(newExpenses, (List<Expense>)returnValue.data);
    }
}