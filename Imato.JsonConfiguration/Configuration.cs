using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace Imato.JsonConfiguration
{
    public static class Configuration<T>
    {
        private static JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        private static Semaphore semaphore = new Semaphore(1, 1);

        private static string GetFilePath(string file)
        {
            if (!file.Contains("."))
            {
                throw new FileNotFoundException("Specify file with extension");
            }

            var fileName = "";

            if (Environment.IsRelease)
            {
                fileName = file;
            }

            var fa = file.Split('.');
            fileName = $"{fa[0]}.{Environment.Name}.{fa[1]}";

            var filePath = Directory
                .EnumerateFiles(System.Environment.CurrentDirectory,
                    fileName,
                    SearchOption.AllDirectories)
                .FirstOrDefault();

            if (filePath == null || !File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            return filePath;
        }

        /// <summary>
        /// Read exists file configuration.json (configuration.Debug.json,  configuration.Test.json)
        /// </summary>
        /// <typeparam name="T">Serialized configuration class</typeparam>
        /// <param name="fileName">JSON file name</param>
        /// <returns>Configuration</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static T Get(string fileName = "configuration.json")
        {
            var str = File.ReadAllText(GetFilePath(fileName));
            var result = JsonSerializer.Deserialize<T>(str, options);
            if (result == null)
            {
                throw new FileNotFoundException($"Cannot deserialize {str}");
            }
            return result;
        }

        /// <summary>
        /// Save configuration to file
        /// </summary>
        /// <typeparam name="T">Serialized configuration class</typeparam>
        /// <param name="config"></param>
        /// <param name="fileName">JSON file name</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static void Save(T config, string fileName = "configuration.json")
        {
            var filePath = GetFilePath(fileName);
            var str = JsonSerializer.Serialize(config, options);
            semaphore.WaitOne(10000);
            File.WriteAllText(filePath, str);
            semaphore.Release();
        }
    }
}