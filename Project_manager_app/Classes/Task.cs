using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_manager_app.Enums;
using TaskStatus = Project_manager_app.Enums.TaskStatus;

namespace Project_manager_app.Classes
{
    public class Task
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; }
        public int ExpectedDurationMinutes { get; set; }
        public Project AssociatedProject { get; set; }

        public Task(string name, string description, DateTime dueDate,TaskStatus status, int expectedDurationMinutes, Project associatedProject)
        {
            Name = name;
            Description = description;
            DueDate = dueDate;
            Status = status;
            ExpectedDurationMinutes = expectedDurationMinutes;
            AssociatedProject = associatedProject;
        }
    }
}
