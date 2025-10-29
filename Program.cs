var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Middleware #1
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("Middleware #1: Before calling next\r\n");

    await next(context);

    await context.Response.WriteAsync("Middleware #1: After calling next\r\n");

});

app.MapWhen((context) => {
        return context.Request.Path.StartsWithSegments("/employees") && 
               context.Request.Query.ContainsKey("id");
    },
    (appBuilder) =>
    {
        appBuilder.Use(async (HttpContext context, RequestDelegate next) =>
        {
            await context.Response.WriteAsync("Middleware #5: Before calling next\r\n");

            await next(context);

            await context.Response.WriteAsync("Middleware #5: After calling next\r\n");

        });

        appBuilder.Use(async (HttpContext context, RequestDelegate next) =>
        {
            await context.Response.WriteAsync("Middleware #6: Before calling next\r\n");

            await next(context);

            await context.Response.WriteAsync("Middleware #6: After calling next\r\n");

        });
    });

// Middleware #2
app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("Middleware #2: Before calling next\r\n");

    await next(context);

    await context.Response.WriteAsync("Middleware #2: After calling next\r\n");
});

// Middleware #3
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("Middleware #3: Before calling next\r\n");

    await next(context);

    await context.Response.WriteAsync("Middleware #3: After calling next\r\n");

});

app.Run();