# Imato.JsonConfiguration

Read and write app configuration from file

### Using 

```csharp
using Imato.JsonConfiguration;

MyConfig config = new MyConfig
{
    Id = 101,
    Internal = new Internal
    {
        Key1 = "Test1",
        Key2 = "Test2"
    }
};

Configuration<MyConfig>.Save(config, "configuration.json");
var configuration = Configuration<MyConfig>.Get("configuration.json");

// Or use default file name myConfig.json
Configuration<MyConfig>.Save(config);
configuration = Configuration<MyConfig>.Get();
```


