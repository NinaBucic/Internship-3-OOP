using Project_manager_app.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStatus = Project_manager_app.Enums.TaskStatus;
using Project_manager_app.Classes;

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

        public static int PositiveIntegerValidation()
        {
            string input = Console.ReadLine();
            int result;
            while (!int.TryParse(input, out result) || result <= 0)
            {
                Console.Write("Invalid input. Please enter a positive integer: ");
                input = Console.ReadLine();
            }
            return result;
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

        public static bool ConfirmAction(string message)
        {
            Console.Write(message + " (yes/no): ");
            string input = Console.ReadLine().ToLower();
            while (input != "yes" && input != "no")
            {
                Console.Write("Invalid input. Please type 'yes' to confirm or 'no' to cancel: ");
                input = Console.ReadLine().ToLower();
            }
            return input == "yes";
        }

        private bool IsProjectNameTaken(string projectName)
        {
            return _projects.Keys.Any(p => p.Name.Equals(projectName, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsTaskNameTaken(Project project, string taskName)
        {
            if (!_projects.ContainsKey(project))
            {
                return false;
            }
            return _projects[project].Any(t => t.Name.Equals(taskName, StringComparison.OrdinalIgnoreCase));
        }

        public void CheckAndUpdateProjectStatus(Project project)
        {
            if (project.Status == ProjectStatus.Completed)
            {
                return;
            }
        
            var allTasksCompleted = _projects[project].All(t => t.Status == TaskStatus.Completed);
            if (allTasksCompleted)
            {
                project.Status = ProjectStatus.Completed;
            }
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
                Console.WriteLine("Tasks:");

                if (tasks.Count == 0)
                {
                    Console.WriteLine("  No tasks available.");
                }
                else
                {
                    foreach (var task in tasks)
                    {
                        Console.WriteLine($"  - {task.Name}");
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

        public void DeleteProject()
        {
            Console.Write("Enter the name of the project you want to delete: ");
            var projectName = GetValidStringInput();
            Console.Clear();

            if (!IsProjectNameTaken(projectName))
            {
                Console.WriteLine("Project not found.");
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
                return;
            }

            var projectToDelete = _projects.Keys.FirstOrDefault(p => p.Name.Equals(projectName, StringComparison.OrdinalIgnoreCase));

            if (ConfirmAction($"Are you sure you want to delete the project '{projectToDelete.Name}'?"))
            {
                _projects.Remove(projectToDelete);
                Console.WriteLine($"\nProject '{projectToDelete.Name}' has been successfully deleted.");
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("\nAction canceled. Press any key to return...");
                Console.ReadKey();
            }
        }

        public void DisplayTasksDueInNext7Days()
        {
            Console.WriteLine("Tasks due in the next 7 days:");

            var today = DateTime.Today;
            var nextWeek = today.AddDays(7);

            var tasksFound = false;

            foreach (var project in _projects)
            {
                var projectHasTasksDueInNextWeek = false;
                foreach (var task in project.Value)
                {
                    if (task.DueDate.Date >= today && task.DueDate.Date <= nextWeek)
                    {
                        if (!projectHasTasksDueInNextWeek)
                        {
                            Console.WriteLine($"\nProject: {project.Key.Name}");
                            projectHasTasksDueInNextWeek = true;
                        }
                        Console.WriteLine($"  Task: {task.Name}");
                        Console.WriteLine($"    Due Date: {task.DueDate.ToShortDateString()}");
                        Console.WriteLine($"    Status: {task.Status}");
                        tasksFound = true;
                    }
                }
            }
            if (!tasksFound)
            {
                Console.WriteLine("No tasks due in the next 7 days.");
            }
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        public void DisplayProjectsByStatus()
        {
            Console.Write("Enter the status to filter by (Active, Pending, Completed): ");
            var selectedStatus = GetValidProjectStatus();

            Console.Clear();
            Console.WriteLine($"Projects with status '{selectedStatus}':");

            var projectsFound = false;

            foreach (var project in _projects.Keys)
            {
                if (project.Status == selectedStatus)
                {
                    Console.WriteLine($"\n- {project.Name}");
                    Console.WriteLine($"  Description: {project.Description}");
                    Console.WriteLine($"  Start Date: {project.StartDate.ToShortDateString()}");
                    Console.WriteLine($"  End Date: {project.EndDate.ToShortDateString()}");
                    projectsFound = true;
                }
            }

            if (!projectsFound)
            {
                Console.WriteLine($"No projects found with status '{selectedStatus}'.");
            }
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        private void PrintTasksInProject(Project project)
        {
            Console.WriteLine($"Tasks in project '{project.Name}':\n");
            var tasks = _projects[project];
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks available.");
            }
            else
            {
                foreach (var task in tasks)
                {
                    Console.WriteLine($"- {task.Name} (Status: {task.Status})");
                }
            }
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        private void ShowProjectDetails(Project project)
        {
            Console.WriteLine($"Project Details for '{project.Name}':");
            Console.WriteLine($"Description: {project.Description}");
            Console.WriteLine($"Start Date: {project.StartDate.ToShortDateString()}");
            Console.WriteLine($"End Date: {project.EndDate.ToShortDateString()}");
            Console.WriteLine($"Status: {project.Status}");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        private void UpdateProjectStatus(Project project, List<Task> tasks)
        {
            if (project.Status == ProjectStatus.Completed)
            {
                Console.WriteLine("This project is already completed. You cannot change its status.");
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Current status: {project.Status}");
            Console.Write("Enter new status (Active, Pending, Completed): ");
            var newStatus = GetValidProjectStatus();

            if (newStatus == ProjectStatus.Completed)
            {
                foreach (var task in tasks)
                {
                    if (task.Status != TaskStatus.Completed)
                    {
                        task.Status = TaskStatus.Completed;
                    }
                }
                Console.WriteLine("\nAll tasks in this project have been marked as Completed.");
            }

            project.Status = newStatus;
            Console.WriteLine($"\nProject status updated to {newStatus}.");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }


        private void AddTaskToProject(Project project)
        {
            Console.Write("Enter the name of the new task: ");
            var taskName = GetValidStringInput();

            while (IsTaskNameTaken(project, taskName))
            {
                Console.Write("Task name already exists in this project. Please enter a different name: ");
                taskName = GetValidStringInput();
            }

            Console.Write("Enter the description of the task: ");
            var description = GetValidStringInput();

            Console.Write("Enter the due date (YYYY-MM-DD): ");
            var dueDate = GetValidDateInput();

            Console.Write("Enter the expected duration (in minutes): ");
            var duration = PositiveIntegerValidation();

            Console.Write("Enter the status of the project (Active, Postponed or Completed):");
            var status = TaskManager.GetValidTaskStatus();

            var newTask = new Task(taskName, description, dueDate, status, duration, project);
            _projects[project].Add(newTask);

            Console.Clear();
            Console.WriteLine($"Task '{newTask.Name}' added to project '{project.Name}' successfully.");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        private void DeleteTaskFromProject(Project project)
        {
            if (_projects[project].Count == 0)
            {
                Console.WriteLine("No tasks available in this project. Press any key to return...");
                Console.ReadKey();
                return;
            }
            
            Console.WriteLine($"Tasks in project '{project.Name}':");
            foreach(var task in _projects[project])
            {
                Console.WriteLine($" - {task.Name}");
            }

            Console.Write("\nEnter the name of the task to delete: ");
            var taskName = GetValidStringInput();
            var taskToDelete = _projects[project].FirstOrDefault(t => t.Name.Equals(taskName, StringComparison.OrdinalIgnoreCase));
            Console.Clear();

            if (taskToDelete == null)
            {
                Console.WriteLine("Task not found. Press any key to return...");
                Console.ReadKey();
                return;
            }

            if (taskToDelete.Status == TaskStatus.Completed)
            {
                Console.WriteLine("Cannot delete a completed task.");
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
                return;
            }

            if (ConfirmAction($"Are you sure you want to delete the task '{taskToDelete.Name}'?"))
            {
                _projects[project].Remove(taskToDelete);
                Console.WriteLine($"\nTask '{taskToDelete.Name}' has been successfully deleted.");
            }
            else
            {
                Console.WriteLine("\nAction canceled. Press any key to return...");
            }
            Console.ReadKey();
        }

        private void ShowTotalActiveTaskTime(Project project)
        {
            var activeTasks = _projects[project].Where(t => t.Status == TaskStatus.Active).ToList();
            if (activeTasks.Count == 0)
            {
                Console.WriteLine("No active tasks in this project.");
            }
            else
            {
                var totalDuration = activeTasks.Sum(t => t.ExpectedDurationMinutes);
                Console.WriteLine($"Total expected duration for all active tasks in project '{project.Name}': {totalDuration} minutes.");
            }
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        public void ManageProject()
        {
            foreach (var project in _projects.Keys)
            {
                Console.WriteLine($"- {project.Name}");
            }

            Console.Write("\nSelect a project by its name to manage: ");
            var projectName = GetValidStringInput();
            var selectedProject = _projects.Keys.FirstOrDefault(p => p.Name.Equals(projectName, StringComparison.OrdinalIgnoreCase));
            Console.Clear();
            if (selectedProject == null)
            {
                Console.WriteLine("Project not found. Press any key to return...");
                Console.ReadKey();
                return;
            }

            var exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine($"Managing project: {selectedProject.Name}\n");
                Console.WriteLine("1. View all tasks in the project");
                Console.WriteLine("2. View project details");
                Console.WriteLine("3. Update project status");
                Console.WriteLine("4. Add a new task to the project");
                Console.WriteLine("5. Delete a task from the project");
                Console.WriteLine("6. View total expected time for active tasks");
                Console.WriteLine("0. Go back");

                var choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        PrintTasksInProject(selectedProject);
                        break;
                    case "2":
                        ShowProjectDetails(selectedProject);
                        break;
                    case "3":
                        UpdateProjectStatus(selectedProject, _projects[selectedProject]);
                        break;
                    case "4":
                        if (selectedProject.Status != ProjectStatus.Completed)
                        {
                            AddTaskToProject(selectedProject);
                            CheckAndUpdateProjectStatus(selectedProject);
                        }
                        else
                        {
                            Console.WriteLine("Cannot add tasks to a completed project.");
                            Console.WriteLine("Press any key to return...");
                            Console.ReadKey();
                        }
                        break;
                    case "5":
                        if (selectedProject.Status != ProjectStatus.Completed)
                        {
                            DeleteTaskFromProject(selectedProject);
                            CheckAndUpdateProjectStatus(selectedProject);
                        }
                        else
                        {
                            Console.WriteLine("Cannot delete tasks of a completed project.");
                            Console.WriteLine("Press any key to return...");
                            Console.ReadKey();
                        }
                        break;
                    case "6":
                        ShowTotalActiveTaskTime(selectedProject);
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to return...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public void ManageTask()
        {
            foreach (var project in _projects.Keys)
            {
                Console.WriteLine($"- {project.Name}");
            }
            Console.Write("\nSelect a project by its name to view tasks: ");

            var projectName = GetValidStringInput();
            var selectedProject = _projects.Keys.FirstOrDefault(p => p.Name.Equals(projectName, StringComparison.OrdinalIgnoreCase));
            Console.Clear();
            if (selectedProject == null)
            {
                Console.WriteLine("Project with that name not found. Press any key to return...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Tasks in project '{selectedProject.Name}':");
            var tasks = _projects[selectedProject];
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks available in this project. Press any key to return...");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tasks[i].Name}");
            }

            Console.Write("\nSelect a task by its number: ");
            var taskNumber = 0;
            while (!int.TryParse(Console.ReadLine(), out taskNumber) || taskNumber < 1 || taskNumber > tasks.Count)
            {
                Console.Write($"Invalid task selection. Please enter a number between 1 and {tasks.Count}: ");
            }

            var selectedTask = tasks[taskNumber - 1];
            TaskManager.ShowTaskSubMenu(selectedTask);
            CheckAndUpdateProjectStatus(selectedProject);
        }

    }
}
