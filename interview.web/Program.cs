using interview.web.App.Interfaces;
using interview.web.App.Middlewares;
using interview.web.App.Services;
using interview.web.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("AppConfig"));
builder.Services.AddScoped(typeof(IGetServices<>), typeof(GetService<>));
builder.Services.AddScoped(typeof(IPostServices<>), typeof(PostService<>));
builder.Services.AddScoped(typeof(IPutServices<>), typeof(PutServices<>));
builder.Services.AddScoped(typeof(IDeleteServices<>), typeof(DeleteServices<>));
builder.Services.AddMemoryCache();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
