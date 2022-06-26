using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyBlogWebAssembly.Client;
using MyBlogWebAssembly.Client.Authentication;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddHttpClient("Authenticated", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler <BaseAddressAuthorizationMessageHandler>();

builder.Services.AddHttpClient("Public", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddApiAuthorization().AddAccountClaimsPrincipalFactory<RoleAccountClaimsPrincipalFactory> ();

await builder.Build().RunAsync();
