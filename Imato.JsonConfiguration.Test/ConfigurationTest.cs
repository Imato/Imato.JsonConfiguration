using NUnit.Framework;
using System;
using System.IO;

namespace Imato.JsonConfiguration.Test
{
    public class ConfigurationTest
    {
        private MyConfig config = new MyConfig
        {
            Id = 101,
            Internal = new Internal
            {
                Key1 = "Test1",
                Key2 = "Test2"
            }
        };

        [Test]
        public void GetNotExists()
        {
            Assert.Throws<FileNotFoundException>(() => Configuration<MyConfig>.Get("sameConfig.json"));
        }

        [Test]
        public void Get()
        {
            var r = Configuration<MyConfig>.Get("configuration.json");
            Assert.AreEqual(config.Id, r.Id);
            Assert.AreEqual(config?.Internal?.Key1, r?.Internal?.Key1);
        }

        [Test]
        public void Save()
        {
            config.Date = DateTime.Now;
            Configuration<MyConfig>.Save(config, "configuration.json");
            var r = Configuration<MyConfig>.Get("configuration.json");
            Assert.AreEqual(config.Date, r.Date);
        }

        [Test]
        public void SaveDefaultFile()
        {
            config.Date = DateTime.Now;
            Configuration<MyConfig>.Save(config);
            var r = Configuration<MyConfig>.Get();
            Assert.AreEqual(config.Date, r.Date);
            File.Exists(Configuration<MyConfig>.DefaultFile);
        }
    }
}