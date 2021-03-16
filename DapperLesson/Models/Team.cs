using System;
using System.Collections.Generic;

namespace DapperLesson.Models
{
    public class Team
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
    }
}
