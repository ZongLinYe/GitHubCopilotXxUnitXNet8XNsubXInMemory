using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ExpenseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExpenseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ExpenseContext _context;
        private readonly ILogger<ExpenseController> _logger;
        public ExpenseController(ExpenseContext context, ILogger<ExpenseController> logger)
        {
            _context = context;
            _logger = logger;

        }


        [HttpGet]
        public IActionResult Index()
        {
            var result = _context.Expenses;

            return Ok(result);
        }
        // PostExpense
        // q: 提供 curl 指令 category = 郊遊   
        // curl -X POST -H "Content-Type: application/json" -d '{"date":"2021-01-01","description":"買早餐","amount":50,"category":"飲食"}' http://localhost:5287/expense
        //curl -X POST -H "Content-Type: application/json" -d "{\"date\":\"2021-01-01\",\"description\":\"劍湖山\",\"amount\":5000,\"category\":\"郊遊\"}" http://localhost:5287/api/Expense
        [HttpPost]
        public async Task<ActionResult<Exception>> PostExpense(Expense expense)
        {
            try
            {
                if (expense == null)
                {
                    return BadRequest("資料錯誤");
                }
                if (expense.Amount < 0)
                {
                    return BadRequest("金額錯誤");
                }
                if (string.IsNullOrWhiteSpace(expense.Category))
                {
                    return BadRequest("類別錯誤");
                }
                if (expense.Date == null)
                {
                    return BadRequest("日期錯誤");
                }
                if (string.IsNullOrWhiteSpace(expense.Description))
                {
                    return BadRequest("說明錯誤");
                }
                _context.Expenses.Add(expense);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred creating the DB.");
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostExpenses(List<Expense> expenses)
        {
            try
            {
                if (expenses == null)
                {
                    return BadRequest("資料錯誤");
                }
                foreach (var expense in expenses)
                {
                    if (expense.Amount < 0)
                    {
                        return BadRequest("金額錯誤");
                    }
                    if (string.IsNullOrWhiteSpace(expense.Category))
                    {
                        return BadRequest("類別錯誤");
                    }
                    if (expense.Date == null)
                    {
                        return BadRequest("日期錯誤");
                    }
                    if (string.IsNullOrWhiteSpace(expense.Description))
                    {
                        return BadRequest("說明錯誤");
                    }
                    _context.Expenses.Add(expense);
                }
                await _context.SaveChangesAsync();
                return Ok(new { succuss = true, message = "新增成功", data = expenses });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred creating the DB.");
                return BadRequest(ex);
            }
        }

        [HttpPut]
        public async Task<ActionResult<Exception>> PutExpense(Expense expense)
        {
            try
            {
                var result = await _context.Expenses.FindAsync(expense.Id);
                if (result == null)
                {
                    return BadRequest("找不到資料");
                }
                result.Date = expense.Date;
                result.Description = expense.Description;
                result.Amount = expense.Amount;
                result.Category = expense.Category;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred creating the DB.");
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<Exception>> DeleteExpense(int id)
        {
            try
            {
                var result = await _context.Expenses.FindAsync(id);
                if (result == null)
                {
                    return BadRequest("找不到資料");
                }
                _context.Expenses.Remove(result);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred creating the DB.");
                return BadRequest(ex);
            }
        }

        [HttpGet("GetExpenseByCategory")]
        public IActionResult GetExpenseByCategory(string category)
        {
            var result = _context.Expenses.Where(x => x.Category == category).ToList();

            return Ok(result);
        }

        [HttpGet("GetExpenseByCategoryAndAmount")]
        public IActionResult GetExpenseByCategoryAndAmount(string category, decimal amount)
        {
            var result = _context.Expenses.Where(x => x.Category == category && x.Amount > amount).ToList();

            return Ok(result);
        }
        [HttpGet("GetExpenseById")]
        public IActionResult GetExpenseById(int id)
        {
            var result = _context.Expenses.FirstOrDefault(x => x.Id == id);

            return Ok(result);
        }



    }
}