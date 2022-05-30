using System;
using System.ComponentModel.DataAnnotations;
namespace Web_App
{
    public class Rate
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Score { get; set; }
        public string Content { get; set; }
        public DateTime DateTime { get; set; }
    }
}

