using ContactList.Server.Features;
using ContactList.Server.Model;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<Database>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddServerValidators();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseMiddleware<UnitOfWork>();

app.MapRazorPages();
app.MapFallbackToFile("index.html");

app.MapGet("/api/contacts", GetContactsQueryHandler.Handle);
app.MapPost("/api/contacts/add", AddContactCommandHandler.Handle);
app.MapGet("/api/contacts/edit", EditContactQueryHandler.Handle);
app.MapPost("/api/contacts/edit", EditContactCommandHandler.Handle);
app.MapPost("/api/contacts/delete", DeleteContactCommandHandler.Handle);

app.Run();
