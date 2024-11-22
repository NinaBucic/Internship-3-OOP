using Project_manager_app.Classes;
using Project_manager_app.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Project_manager_app.Classes.Task;
using TaskStatus = Project_manager_app.Enums.TaskStatus;


namespace Project_manager_app
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Dictionary<Project, List<Task>> projects = new Dictionary<Project, List<Task>>();

            ProjectManager projectManager = new ProjectManager(projects);
            TaskManager taskManager = new TaskManager();

            var project1 = new Project("Website Redesign", "Redesign and update the company website.", DateTime.Today, DateTime.Today.AddMonths(1), ProjectStatus.Active);
            var project2 = new Project("Mobile App Development", "Develop a mobile app for online store.", DateTime.Today.AddMonths(1), DateTime.Today.AddMonths(2), ProjectStatus.Pending);
            var project3 = new Project("Marketing Campaign for Product Launch", "Marketing campaign for the launch of a new product.", DateTime.Today.AddMonths(2), DateTime.Today.AddMonths(3), ProjectStatus.Completed);

            var task1 = new Task("Design New Homepage Layout", "Create a new homepage layout for the website.", DateTime.Today.AddDays(2), TaskStatus.Active, 240, project1);
            var task2 = new Task("Test Mobile App Beta Version", "Test the beta version of the mobile app on various devices.", DateTime.Today.AddDays(3), TaskStatus.Active, 180, project2);
            var task3 = new Task("Create Marketing Content for Social Media", "Create content for marketing campaign on social media platforms.", DateTime.Today.AddDays(5), TaskStatus.Completed, 120, project3);
            var task4 = new Task("Conduct HR System User Training", "Provide training for the new HR system users.", DateTime.Today.AddDays(7), TaskStatus.Postponed, 150, project1);
            var task5 = new Task("Optimize Website Speed", "Increase website loading speed by optimizing images and code.", DateTime.Today.AddDays(10), TaskStatus.Active, 180, project2);
            var task6 = new Task("Implement New Payment Gateway", "Implement a new payment gateway for the mobile app.", DateTime.Today.AddDays(12), TaskStatus.Postponed, 200, project2);
            var task7 = new Task("Create Customer Feedback Survey", "Create a survey to collect customer feedback about the product.", DateTime.Today.AddDays(15), TaskStatus.Active, 60, project1);

            projects.Add(project1, new List<Task> { task1, task4, task7 });
            projects.Add(project2, new List<Task> { task2, task5, task6 });
            projects.Add(project3, new List<Task> { task3 });

            var exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("MAIN MENU: \n");
                Console.WriteLine("1. Display all projects with tasks");
                Console.WriteLine("2. Add new project");
                Console.WriteLine("3. Delete project");
                Console.WriteLine("4. Display all tasks due in the next 7 days");
                Console.WriteLine("5. Filter projects by status");
                Console.WriteLine("6. Manage individual project");
                Console.WriteLine("7. Manage individual task");
                Console.WriteLine("0. Exit");

                var choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        projectManager.PrintAllProjectsWithTasks();
                        break;
                    case "2":
                        projectManager.AddNewProject();
                        break;
                    case "3":
                        projectManager.DeleteProject();
                        break;
                    case "4":
                        projectManager.DisplayTasksDueInNext7Days();
                        break;
                    case "5":
                        projectManager.DisplayProjectsByStatus();
                        break;
                    case "6":
                        
                        break;
                    case "7":
                        
                        break;
                    case "0":
                        Console.WriteLine("Exiting application. Goodbye!");
                        exit=true;
                        break;
                    default:
                        Console.WriteLine("Unknown option. Press any key to return...");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
