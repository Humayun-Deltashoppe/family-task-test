
using System;
using System.ComponentModel;

namespace Domain.Commands
{
   public class CreateTaskCommand
    {
        public string Subject { get; set; }

        public bool IsComplete { get; set; } = false; 
        public Guid? AssignedToId { get; set; }
    }
}
