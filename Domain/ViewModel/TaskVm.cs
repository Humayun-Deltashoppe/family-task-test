﻿using Domain.Commands;
using System;

namespace Domain.ViewModel
{
   public class TaskVm
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public bool IsComplete { get; set; }
        public Guid? AssignedToId { get; set; }
        public string AssignedToAvatar { get; set; }
    }
}
