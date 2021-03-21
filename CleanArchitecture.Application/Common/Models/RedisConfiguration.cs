using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Common.Models
{
    public class RedisConfiguration
    {
        public string Hosts { get; set; }
        public int Database { get; set; }
    }
}
