using BlazorClient.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WMS.UI;
using WMS.UI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri("http://localhost:5083/")
});

builder.Services.AddScoped<ResourceService>();
builder.Services.AddScoped<UnitOfMeasureService>();
builder.Services.AddScoped<ReceiptsService>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

await builder.Build().RunAsync();