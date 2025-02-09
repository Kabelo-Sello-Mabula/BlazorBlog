using BlazingBlogNet8.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddTransient<UserService>()
                .AddTransient<CategoryService>()
                .AddTransient<BlogPostService>();

builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<BlogAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(serviceProvider =>
    serviceProvider.GetRequiredService<BlogAuthenticationStateProvider>());

var blogConnectionString = builder.Configuration.GetConnectionString("Blog");

builder.Services.AddDbContext<BlogContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")), ServiceLifetime.Transient);


var app = builder.Build();

/*app.Use(async (context, next) =>
{
    var host = context.Request.Host.Host;

    if (host.Contains(".example.com", StringComparison.OrdinalIgnoreCase) && context.Request.Path == "/")
    {
        // Redirect to the subpath
        context.Response.Redirect("/subpath");
        return;
    }

    await next();
});*/


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
