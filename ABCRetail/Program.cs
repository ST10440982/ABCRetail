using ABCRetail;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Register AzureStorageService
builder.Services.AddSingleton(new AzureStorageService("DefaultEndpointsProtocol=https;AccountName=st10440982;AccountKey=voure985JTD/tW9VhjuS44e+3FlbqagW+3hWzbikgBNdnQr2HfjKc2SNjC3EVOyjneXKQYmuakYD+ASthz9HzQ==;EndpointSuffix=core.windows.net"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();