using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using watchstore;
using watchstore.Data;

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(wb => wb.UseStartup<Startup>())
    .Build();

if (args.Contains("/seed"))
{
    using (var scope = host.Services.CreateScope())
    {
        Dbinitializer.Initialize(scope.ServiceProvider.GetService<ApplicationDbContext>(), scope.ServiceProvider);
    }
    Environment.Exit(0);
}

await host.RunAsync();