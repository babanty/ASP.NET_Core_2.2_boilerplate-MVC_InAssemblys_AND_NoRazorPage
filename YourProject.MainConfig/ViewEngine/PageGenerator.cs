using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System;
using System.IO;
using System.Threading.Tasks;

namespace YourProject.MainConfig.ViewEngine
{

    /// <summary> Класс наследник IView (asp.net core) - возвращает саму вьюшку (разметку) </summary>
    public class PageGenerator : IView
    {
        public string Path { get; set; }

        /// <summary> Конструктор </summary>
        /// <param name="viewPath">Конечный путь включающий сам файл с расширением</param>
        public PageGenerator(string viewPath)
        {
            Path = viewPath;
        }

        /// <summary> Отрендерить разметку. Метод вызывается asp.net_core-ом через DI</summary>
        public async Task RenderAsync(ViewContext context)
        {
            // html разметка
            string content = String.Empty;
            using (FileStream viewStream = new FileStream(Path, FileMode.Open))
            {
                using (StreamReader viewReader = new StreamReader(viewStream))
                {
                    content = viewReader.ReadToEnd();
                }
            }
            await context.Writer.WriteAsync(content);
        }
    }
}
