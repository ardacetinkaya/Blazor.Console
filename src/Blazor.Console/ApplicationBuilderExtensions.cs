namespace Blazor.Console
{
    using Microsoft.AspNetCore.Builder;
    using System.IO;
    using System.Reflection;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseConsole(this IApplicationBuilder applicationBuilder, string webRootPath)
        {
            var component = Assembly.GetExecutingAssembly();
            var componentResources = component.GetManifestResourceNames();

            var destinationFolderPath = Path.Combine(webRootPath, "Blazor.Console");
            if (!Directory.Exists(destinationFolderPath))
            {
                Directory.CreateDirectory(destinationFolderPath);
            }

            foreach (var resource in componentResources)
            {
                var position = resource.LastIndexOf(":");
                var resourceName = resource.Substring(position + 1, resource.Length - position - 1);

                using (var resourceStream = component.GetManifestResourceStream(resource))
                {
                    if (resourceStream != null)
                    {
                        var bufferSize = 1024 * 1024;
                        using var fileStream = new FileStream(Path.Combine(destinationFolderPath, resourceName)
                                                    , FileMode.OpenOrCreate, FileAccess.Write);
                        fileStream.SetLength(resourceStream.Length);
                        var bytesRead = -1;
                        var bytes = new byte[bufferSize];

                        while ((bytesRead = resourceStream.Read(bytes, 0, bufferSize)) > 0)
                        {
                            fileStream.Write(bytes, 0, bytesRead);
                        }
                    }
                }
            }
            return applicationBuilder;
        }
    }
}
