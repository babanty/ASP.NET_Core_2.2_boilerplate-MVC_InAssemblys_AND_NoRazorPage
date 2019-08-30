using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace YourProject.MainConfig
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            // если используется Release, то структура папок затирается и надо использовать этот путь
            string path = "Views";

            // если используется Debug, то у нас другой путь до статических файлов
            #if DEBUG
                path = "..\\YourProject.Views\\Views";
            #endif

            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseWebRoot(path);   // установка папки со статическими файлами

        }
    }
}
