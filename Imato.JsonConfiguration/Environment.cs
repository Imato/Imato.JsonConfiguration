namespace Imato.JsonConfiguration
{
    public static class Environment
    {
        public static string Name
        {
            get
            {
                var name = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                    ?? System.Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
                if (name == null)
                {
                    name =
#if DEBUG
                    DEBUG;
#endif
#if RELEASE
                    RELEASE;
#endif
#if TEST
                    TEST;
#endif
                }
                return name;
            }
        }

        public static bool IsDebug = Name == DEBUG;
        public static bool IsRelease = Name == RELEASE;

        public static string DEBUG = "Debug";
        public static string RELEASE = "Release";
        public static string TEST = "Debug";
    }
}