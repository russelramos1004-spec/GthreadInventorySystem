using Inventory.Core.Repositories;
using Inventory.Core.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();


var connString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddScoped(s => new SqlConnection(connString));


builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<SupplierRepository>();
builder.Services.AddScoped<CustomerRepository>();

builder.Services.AddScoped<StockInRepository>(sp =>
{
    var productRepo = sp.GetRequiredService<ProductRepository>();
    var supplierRepo = sp.GetRequiredService<SupplierRepository>();
    return new StockInRepository(connString, productRepo, supplierRepo);
});


builder.Services.AddScoped<StockOutRepository>(sp =>
{
    var conn = sp.GetRequiredService<SqlConnection>();
    return new StockOutRepository(conn);
});


builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<SupplierService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<StockInService>();
builder.Services.AddScoped<StockOutService>();


builder.Services.AddScoped<AuthenticationStateProvider, Inventory.Web.Auth.SessionAuthProvider>();
builder.Services.AddAuthorizationCore();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");


using (var testConn = new SqlConnection(connString))
{
    try
    {
        testConn.Open();
        Console.WriteLine("✅ Connected to SQL Server successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ SQL Connection Failed: {ex.Message}");
    }
}

app.Run();
