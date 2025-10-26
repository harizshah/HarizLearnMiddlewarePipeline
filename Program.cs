// Create a WebApplication builder — sets up the app configuration and hosting
var builder = WebApplication.CreateBuilder(args);

// Build the app — prepares it to handle HTTP requests
var app = builder.Build();


// ========== MIDDLEWARE #1 ==========
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    // This code runs *before* passing control to the next middleware
    await context.Response.WriteAsync("Middleware #1: Before calling next\r\n");

    // Pass control to the next middleware in the pipeline
    await next(context);

    // This code runs *after* the next middleware finishes executing
    await context.Response.WriteAsync("Middleware #1: After calling next\r\n");
});


// ========== MIDDLEWARE #2 ==========
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    // Executes before the next middleware (if any)
    await context.Response.WriteAsync("Middleware #2: Before calling next\r\n");

    // The 'next' call is commented out, so the pipeline stops here
    // await next(context);

    // Because next() is skipped, middleware #3 will NEVER be called
    await context.Response.WriteAsync("Middleware #2: After calling next\r\n");
});


// ========== MIDDLEWARE #3 ==========
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    // This line would only run if Middleware #2 called next()
    await context.Response.WriteAsync("Middleware #3: Before calling next\r\n");

    // Pass control further down (to any next middleware or endpoint)
    await next(context);

    // This line would run after lower middleware finishes
    await context.Response.WriteAsync("Middleware #3: After calling next\r\n");
});


// The terminal middleware — ends the request pipeline
app.Run();