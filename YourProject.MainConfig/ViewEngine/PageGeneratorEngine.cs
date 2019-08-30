using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace YourProject.MainConfig.ViewEngine
{
    /// <summary> Класс наследник IViewEngine (asp.net core) необходимый для поиска вьюшки в файлах</summary>
    public class PageGeneratorEngine : IViewEngine
    {
        /// <summary> Словарь с конфигурациями </summary>
        private IConfiguration Configuration { get; set; }
        /// <summary> Имя ключа контроллека в ActionContext </summary>
        private const string ControllerKey = "controller";

        /// <summary> Конструктор </summary>
        /// <param name="Configuration">Конфиги приложения </param>
        public PageGeneratorEngine(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }

        /// <summary> Заглушка, метод необходимый для интерфейса IViewEngine </summary>
        public ViewEngineResult GetView(string executingFilePath, string viewPath, bool isMainPage)
        {
            return ViewEngineResult.NotFound(viewPath, new string[] { });
        }

        /// <summary> Поиск вьюшки. Метод вызывает Startup.cs через DI.</summary>
        public ViewEngineResult FindView(ActionContext context, string viewName, bool isMainPage)
        {
            // получаем путь где лежат все view-шки (корневая папка)
            var rootPath = $"{Configuration["ViewRootPath_Release"]}"; // для релиза путь такой т.к. при релизе не сохраняется структура папок
            #if DEBUG
                rootPath = $"..\\{Configuration["ViewAssembly_Debug"]}/{Configuration["ViewRootPath_Debug"]}";
            #endif

            // получаем имя контроллера вызвавшего вьюшку, чтобы выснить в какой папке нам искать вьюшку
            var controllerName = NormalizedRouteValue.GetNormalizedRouteValue(context, ControllerKey);

            // если не пришел viewName, то берем сами метод контроллера, что вызвал IActionResult
            viewName = String.IsNullOrEmpty(viewName) ? context.RouteData.Values["action"].ToString() : viewName;

            // формируем конечный путь поиска вьюшки
            string viewPath = $"{rootPath}/{controllerName}/{viewName}.html";

            if (File.Exists(viewPath))
            {
                return ViewEngineResult.Found(viewPath, new PageGenerator(viewPath));
            }
            else
            {
                return ViewEngineResult.NotFound(viewName, new string[] { viewPath });
            }
        }
    }
}
