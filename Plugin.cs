using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Threading;
using Microsoft.Web.WebView2.WinForms;
using System.Threading.Tasks;
using QuickLook.Common.Plugin;
using QuickLook.Plugin.HtmlViewer;

// dotnet add QuickLook.Plugin.FolderReadme.csproj package Microsoft.Web.WebView2

namespace QuickLook.Plugin.FolderReadme
{
    public class Plugin : IViewer
    {
        public int Priority => 1;

        private static readonly string[] _extensions =
        [
            ".md", ".markdown", // The most common Markdown extensions
                ".mdx", // MDX (Markdown + JSX), used in React ecosystems
                ".mmd", // MultiMarkdown (MMD), an extended version of Markdown
                ".mkd", ".mdwn", ".mdown", // Early Markdown variants, used by different parsers like Pandoc, Gitit, and Hakyll
                ".mdc", // A Markdown variant used by Cursor AI [Repeated format from ImageViewer]
                ".qmd", // Quarto Markdown, developed by RStudio for scientific computing and reproducible reports
                ".rmd", ".rmarkdown", // R Markdown, mainly used in RStudio
                ".apib", // API Blueprint, a Markdown-based format
                ".mdtxt", ".mdtext", // Less common
            ];

        public void Init()
        {
            // Initialization logic if needed
        }

        private WebpagePanel _panel;
        private string _currentHtmlPath;
        private readonly string _resourcePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            @"pooi.moe\QuickLook\QuickLook.Plugin.MarkdownViewer");

        public bool CanHandle(string path)
        {
            if (File.Exists(path)) return false; // Return 

            // 查找所有可能的readme文件名和扩展名组合
            var readmeFile = Directory.GetFiles(path)
            .FirstOrDefault(f =>
                Path.GetFileNameWithoutExtension(f).Equals("readme", StringComparison.OrdinalIgnoreCase) &&
                _extensions.Contains(Path.GetExtension(f).ToLower()));

            if (readmeFile == null) return false;
            return true;
        }

        public void Prepare(string path, ContextObject context)
        {
            context.PreferredSize = new Size { Width = 600, Height = 400 };
        }

        public void View(string path, ContextObject context)
        {
            context.IsBusy = true;
            // 直接查找文件名为readme的文件，不区分大小写
            var readmeFile = Directory.GetFiles(path)
                .FirstOrDefault(f =>
                    string.Equals(Path.GetFileNameWithoutExtension(f), "readme", StringComparison.OrdinalIgnoreCase));

            _panel = new WebpagePanel();
            context.ViewerContent = _panel;
            context.Title = Path.GetFileName(path)+" - README";

            var htmlPath = GenerateHtml(readmeFile);
            _panel.FallbackPath = Path.GetDirectoryName(path);
            _panel.NavigateToFile(htmlPath);
            _panel.Dispatcher.Invoke(() => { context.IsBusy = false; }, DispatcherPriority.Loaded);
            context.IsBusy = false;
        }

        private string GenerateHtml(string readmePath)
        {
            var templatePath = Path.Combine(_resourcePath, "md2html.html");

            if (!File.Exists(templatePath))
                throw new FileNotFoundException($"Required template file template.html not found in {templatePath}");

            // 读取模板内容
            var template = File.ReadAllText(templatePath);
            var content = File.ReadAllText(readmePath);

            var html = template.Replace("{{content}}", content);

            // 生成唯一的临时文件名
            string outputPath;
            do
            {
                var uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8);
                var outputFileName = $"temp_{uniqueId}.html";
                outputPath = Path.Combine(_resourcePath, outputFileName);
            } while (File.Exists(outputPath));

            // 清理之前的临时文件
            // CleanupTempHtmlFile();

            File.WriteAllText(outputPath, html);
            _currentHtmlPath = outputPath;

            return outputPath;
        }

        private void CleanupTempHtmlFile()
        {
            if (!string.IsNullOrEmpty(_currentHtmlPath) && File.Exists(_currentHtmlPath))
            {
                try
                {
                    File.Delete(_currentHtmlPath);
                    _currentHtmlPath = null;
                }
                catch { }
            }
        }

        public void Cleanup()
        {
            CleanupTempHtmlFile();

            if (_panel != null)
            {
                _panel = null;
            }
        }
    }
}