using System;

namespace Imato.JsonConfiguration.Test
{
    public class MyConfig
    {
        public int Id { get; set; }
        public Internal? Internal { get; set; }
        public DateTime Date { get; set; }
    }

    public class Internal
    {
        public string? Key1 { get; set; }
        public string? Key2 { get; set; }
    }
}