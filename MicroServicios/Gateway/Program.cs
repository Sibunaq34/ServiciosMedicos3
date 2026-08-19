var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactDev", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "http://127.0.0.1:5173",
                "https://focused-easley.138-59-135-33.plesk.page"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddReverseProxy().LoadFromConfig(
    builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("ReactDev");
app.UseAuthorization();
app.MapReverseProxy();

app.Run();