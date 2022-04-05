using System.IO;
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
            var directory = Path.GetDirectoryName(file);
            directory = directory == string.Empty ? System.Environment.CurrentDirectory : directory;
            var fileName = Path.GetFileName(file);

            if (!file.Contains("."))
            {
                throw new FileNotFoundException("Specify file with extension");
            }

            if (!Environment.IsRelease)
            {
                var fa = fileName.Split('.');
                fileName = $"{fa[0]}.{Environment.Name}.{fa[1]}";
            }

            return Path.Combine(directory, fileName);
        }

        public static string DefaultFile
        {
            get
            {
                var f = $"{typeof(T).Name}.json";
                f = $"{f.Substring(0, 1).ToLower()}{f.Substring(1, f.Length - 1)}";
                return Path.Combine(System.Environment.CurrentDirectory, f);
            }
        }

        /// <summary>
        /// Read exists file configuration.json (configuration.Debug.json,  configuration.Test.json)
        /// </summary>
        /// <typeparam name="T">Serialized configuration class</typeparam>
        /// <param name="fileName">JSON file name</param>
        /// <returns>Configuration</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static T Get(string fileName)
        {
            var filePath = GetFilePath(fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            var str = File.ReadAllText(filePath);
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
        public static void Save(T config, string fileName)
        {
            var filePath = GetFilePath(fileName);
            var str = JsonSerializer.Serialize(config, options);
            semaphore.WaitOne(10000);
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            File.WriteAllText(filePath, str);
            semaphore.Release();
        }

        /// <summary>
        /// Read exists file nameof(T).json (nameof(T).Debug.json,  nameof(T).Test.json)
        /// </summary>
        /// <typeparam name="T">Serialized configuration class</typeparam>
        /// <param name="fileName">JSON file name</param>
        /// <returns>Configuration</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static T Get()
        {
            return Get(DefaultFile);
        }

        /// <summary>
        /// Save configuration to file nameof(T).json (nameof(T).Debug.json,  nameof(T).Test.json)
        /// </summary>
        /// <typeparam name="T">Serialized configuration class</typeparam>
        /// <param name="config"></param>
        /// <param name="fileName">JSON file name</param>
        /// <returns></returns>
        public static void Save(T config)
        {
            Save(config, DefaultFile);
        }
    }
}