# ASP.NET_Core_2.2_boilerplate-MVC_InAssemblys_AND_NoRazorPage
## Что за шаблон проекта
1. Здесь отказались от Razor Page (.cshtml). Теперь запускаются простые html-файлы в отдельной сборке. 
2. Теперь гиперразметка (в прошлом .cshtml) лежит не в отдельно от js скриптов и прочего, которые по умолчанию находятся в wwwroot. Можно как обычно класть любые файлы, например .css, .png и т.д. рядом с html и указывать ссылку на эти файлы как в обычном html.
3. В отличие от ASP.NET API, View вынесены в отдельную сборку таким образом чтобы она работала в пределах одного приложения (хостинга). А controller-ы по прежнему возвращают IActionResult.
4. Архитектура MVC теперь разбита на отдельные сборки под model, view, controller. В asp.net core сборке лежат только конфигурации приложения, вся логика вынесена в отдельные сборки.
5. Публикация проверена в "боевых" условиях.

## Пошаговые действия как что делалось
1. Создан стандартный ASP.NET Core 2.2. MVC проект - YourProject.MainConfig.
2. Удалена папка wwwroot.
3. Удалена папка Views.
4. Добавлен проект YourProject.Model - Библиотека классов .NET Standard 2.
5. Из сборки YourProject.Model удален класс Class1.cs.
6. Из сборки YourProject.MainConfig перенесен файл ErrorViewModel.cs в YourProject.Model.
7. В сборке YourProject.MainConfig удалена папка Models.
8. В сборке YourProject.MainConfig добавлена ссылка-зависимость на YourProject.Model.
9. Создан проект YourProject.Controllers - asp.net core 2.2 Библиотека классов Razor.
10. Из сборки YourProject.Controllers удалена папка Areas.
11. В сборке YourProject.Controllers добавлена ссылка-зависимость на YourProject.Model.
12. В сборке YourProject.MainConfig добавлена ссылка-зависимость на YourProject.Controllers.
13. Из сборки YourProject.MainConfig перенесен файл HomeController.cs в YourProject.Controllers c последующим изменением пространства имен на YourProject.Controllers
14. Из сборки YourProject.MainConfig удалена папка Controllers.
15. Создан проект YourProject.Views - asp.net core 2.2 Библиотека классов Razor.
16. Из сборки YourProject.Views удалена папка Areas.
17. В сборку YourProject.Views через NuGet добавлен пакет Microsoft.AspNetCore.StaticFiles
18. В сборке YourProject.MainConfig добавлена ссылка-зависимость на YourProject.Views.
19. В сборке YourProject.Controllers добавлена ссылка-зависимость на YourProject.Views.
20. В сборку YourProject.Views добавлена папка Views. Внутрь нее папка Home (т.к. у нас есть HomeController). Внутрь папки Home добавлены Index.html (т.к. в HomeController есть метод Index) и Index.css чтобы проверить работают ли статические файлы.
21. В сборку YourProject.Views в папку Views добавлен favicon.ico.
22. В сборку YourProject.MainConfig добавлен web.config (внимательно смотрите на AspNetCoreModule т.к. автоматом он пишет AspNetCoreModuleV2, а у некоторых платных хостингов стоит asp.net модуль которому надо писать именно AspNetCoreModule без V2).
23. В сборке YourProject.MainConfig создана папка ViewEngine. В нее добавлены PageGenerator.cs и PageGeneratorEngine.cs, а так же readme.txt. Это движок вместо RazorPage возвращает html страницы.
24. В сборке YourProject.MainConfig в appsettings.json вписываем конфигурации необходимые чтобы движок знал где лежат view-файлы.
25. В сборке YourProject.MainConfig в Startup.cs -> ConfigureServices // Указываем что view будут обрабатываться нашим собственным генератором, а не Razor Page
26. В сборке YourProject.MainConfig в Program.cs -> CreateWebHostBuilder переписываем метод чтобы сборка могла видеть статические файлы в других папках за пределами своей директории
27. В файле YourProject.Views.csproj  записываем что при публикации приложения необходимо, чтобы папка Views не пропадала. В пределах тега \</Project> вставляем следующий код:

   \<ItemGroup>
    \<Content Include="Views\\**">
      \<CopyToOutputDirectory>PreserveNewest\</CopyToOutputDirectory>
    \</Content>
  \</ItemGroup>

  \<Target Name="CreateAppDataFolder" AfterTargets="AfterPublish">
    \<MakeDir Directories="\$(PublishDir)Views" Condition="!Exists('\$(PublishDir)Views')" />
  \</Target>
