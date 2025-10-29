// 1️⃣ Create a WebApplication builder
// This sets up the basic ASP.NET Core environment (config, logging, web server, etc.)
var builder = WebApplication.CreateBuilder(args);

// 2️⃣ Build the app pipeline object from the builder
var app = builder.Build();


// ========== GLOBAL MIDDLEWARES (apply to all routes) ==========

// 🧱 Middleware #1 — Executes first for all requests
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("Middleware #1: Before calling next\r\n");

    // Pass control to the next middleware in the global pipeline
    await next(context);

    await context.Response.WriteAsync("Middleware #1: After calling next\r\n");
});


// ========== BRANCH MIDDLEWARE (only for "/employees" path) ==========

// `app.Map("/employees", ...)` creates a *branch* in the middleware pipeline.
// Only requests whose path starts with `/employees` will execute this branch.
app.Map("/employees", (appBuilder) =>
{
    // 🧩 Middleware #5 — First in /employees branch
    appBuilder.Use(async (HttpContext context, RequestDelegate next) =>
    {
        await context.Response.WriteAsync("Middleware #5: Before calling next\r\n");

        await next(context);

        await context.Response.WriteAsync("Middleware #5: After calling next\r\n");
    });

    // 🧩 Middleware #6 — Second in /employees branch
    appBuilder.Use(async (HttpContext context, RequestDelegate next) =>
    {
        await context.Response.WriteAsync("Middleware #6: Before calling next\r\n");

        await next(context);

        await context.Response.WriteAsync("Middleware #6: After calling next\r\n");
    });

    // ⚠️ No `Run()` inside this branch, so this branch passes control back
    // to the global pipeline once all branch middleware have completed.
});


// ========== MORE GLOBAL MIDDLEWARES (after the branch definition) ==========

// 🧱 Middleware #2 — Runs after #1 for all routes (except when a branch is taken)
app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("Middleware #2: Before calling next\r\n");

    await next(context);

    await context.Response.WriteAsync("Middleware #2: After calling next\r\n");
});

// 🧱 Middleware #3 — Runs after #2, before the final endpoint
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("Middleware #3: Before calling next\r\n");

    await next(context);

    await context.Response.WriteAsync("Middleware #3: After calling next\r\n");
});


// 3️⃣ Run() marks the terminal middleware
// This is where the pipeline stops — no more middleware runs after this.
app.Run();
