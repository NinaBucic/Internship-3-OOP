using Project_manager_app.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_manager_app.Classes
{
    public class ProjectManager
    {
        private Dictionary<Project, List<Task>> _projects;

        public ProjectManager(Dictionary<Project, List<Task>> projects)
        {
            _projects = projects;
        }

        public static string GetValidStringInput()
        {
            string input = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.Write("Input cannot be empty. Please try again: ");
                input = Console.ReadLine();
            }
            return input;
        }

        public static DateTime GetValidDateInput()
        {
            string input = Console.ReadLine();
            DateTime date;
            while (!DateTime.TryParseExact(input, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out date))
            {
                Console.Write("Invalid date format. Please enter a valid date (YYYY-MM-DD): ");
                input = Console.ReadLine();
            }
            return date;
        }

        public static ProjectStatus GetValidProjectStatus()
        {
            string input = Console.ReadLine();
            while (input.ToLower() != "active" && input.ToLower() != "pending" && input.ToLower() != "completed")
            {
                Console.Write("Invalid input. Enter (Active, Pending, Completed): ");
                input = Console.ReadLine();
            }
            return (ProjectStatus)Enum.Parse(typeof(ProjectStatus), input, true);
        }

        private bool IsProjectNameTaken(string projectName)
        {
            return _projects.Keys.Any(p => p.Name.Equals(projectName, StringComparison.OrdinalIgnoreCase));
        }

        public void PrintAllProjectsWithTasks()
        {
            if (_projects.Count == 0)
            {
                Console.WriteLine("No projects available.");
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
                return;
            }

            foreach (var projectEntry in _projects)
            {
                var project = projectEntry.Key;
                var tasks = projectEntry.Value;

                Console.WriteLine($"Project: {project.Name}");
                Console.WriteLine($"Description: {project.Description}");
                Console.WriteLine($"Start Date: {project.StartDate.ToShortDateString()}");
                Console.WriteLine($"End Date: {project.EndDate.ToShortDateString()}");
                Console.WriteLine($"Status: {project.Status}");
                Console.WriteLine("Tasks:");

                if (tasks.Count == 0)
                {
                    Console.WriteLine("  No tasks available.");
                }
                else
                {
                    foreach (var task in tasks)
                    {
                        Console.WriteLine($"  Task: {task.Name}");
                        Console.WriteLine($"    Description: {task.Description}");
                        Console.WriteLine($"    Due Date: {task.DueDate.ToShortDateString()}");
                        Console.WriteLine($"    Status: {task.Status}");
                        Console.WriteLine($"    Expected Duration: {task.ExpectedDurationMinutes} minutes");
                    }
                }

                Console.WriteLine();
            }
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        public void AddNewProject()
        {
            Console.Write("Enter the name of the new project: ");
            var name = GetValidStringInput();

            while (IsProjectNameTaken(name))
            {
                Console.Write("Project name already exists. Please enter a different name: ");
                name = GetValidStringInput();
            }

            Console.Write("Enter the description of the new project: ");
            var description = GetValidStringInput();

            Console.Write("Enter the start date (YYYY-MM-DD): ");
            var startDate = GetValidDateInput();

            Console.Write("Enter the end date (YYYY-MM-DD): ");
            var endDate = GetValidDateInput();
            while (endDate < startDate)
            {
                Console.Write("End date must be later than start date. Please try again: ");
                endDate = GetValidDateInput();
            }

            Console.Write("Enter the status of the project (Active, Pending or Completed):");
            var status = GetValidProjectStatus();

            var newProject = new Project(name, description, startDate, endDate, status);
            _projects.Add(newProject, new List<Task>());

            Console.Clear();
            Console.WriteLine($"Project '{newProject.Name}' has been added successfully.");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }


    }
}
