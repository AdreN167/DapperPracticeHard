using System;

namespace DapperLesson.Models
{
    public class Player
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; }
        public int Number { get; set; }
        public Guid? TeamId { get; set; }
    }
}