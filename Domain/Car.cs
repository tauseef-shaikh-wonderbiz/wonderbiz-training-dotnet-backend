using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Car
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Model { get; set; }

        public string? Color { get; set; }

    }
}