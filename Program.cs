// Create a WebApplication builder — prepares configuration, logging, and web server setup
var builder = WebApplication.CreateBuilder(args);

// Build the WebApplication object
var app = builder.Build();


// ========== MIDDLEWARE #1 ==========
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    // Executes first when a request arrives
    await context.Response.WriteAsync("Middleware #1: Before calling next\r\n");

    // Pass control to the next middleware in the pipeline
    await next(context);

    // Executes after the next middleware has completed
    await context.Response.WriteAsync("Middleware #1: After calling next\r\n");
});


// ========== MIDDLEWARE #2 (Terminal Middleware) ==========
app.Run(async (context) =>
{
    // This is a *terminal middleware* because app.Run() ends the pipeline
    await context.Response.WriteAsync("Middleware #2: Processed.\r\n");
});


// ========== MIDDLEWARE #3 (Unreachable) ==========
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    // This middleware is *never executed* because Middleware #2 (app.Run) stops the pipeline
    await context.Response.WriteAsync("Middleware #3: Before calling next\r\n");

    await next(context);

    await context.Response.WriteAsync("Middleware #3: After calling next\r\n");
});


// Starts the web app and begins handling requests
app.Run();