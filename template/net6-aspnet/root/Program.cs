
// Create a root project for the template 
using function;

var builder = WebApplication.CreateBuilder(args);

var function = new Startup();
function.Build(builder);

var app = builder.Build();
function.Configure(app);

app.Run();
