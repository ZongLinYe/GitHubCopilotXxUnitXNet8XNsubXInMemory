using ExpenseAPI;
using ExpenseAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ExpenseContext>(options =>
    options.UseInMemoryDatabase("ExpenseDatabase")
);

var app = builder.Build();
// Generate default data
// 由於使用 InMemoryDatabase，所以每次啟動都會產生新的資料庫
// 這裡使用 DataGenerator.Initialize(app.Services) 來產生預設資料
// 這樣就不用每次都使用 curl 來新增資料
// 如何用 curl 來新增資料請參考 ExpenseController.cs
// curl -X POST -H "Content-Type: application/json" -d '{"date":"2021-01-01","description":"買早餐","amount":50,"category":"飲食"}' http://localhost:5287/expense
// Scope 是一個生命週期，當 Scope 結束時，Scope 內的物件會被釋放
// 生命週期有三種：Singleton、Scoped、Transient
// Singleton：整個應用程式的生命週期，只會有一個實例
// Scoped：每一個 HTTP Request 的生命週期，每一個 HTTP Request 會有一個實例
// Transient：每一次呼叫時的生命週期，每一次呼叫時會有一個實例
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ExpenseContext>();
        DataGenerator.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// app.MapControllers(); 是 app.UseRouting(); + app.UseEndpoints(); 的语法糖
// 主要是為了讓 app.MapGet() 及 app.MapPost() 可以使用
// app.UseHttpsRedirection();
app.MapControllers();



app.Run();
