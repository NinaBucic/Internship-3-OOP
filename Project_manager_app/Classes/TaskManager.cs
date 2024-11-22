using Project_manager_app.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStatus = Project_manager_app.Enums.TaskStatus;

namespace Project_manager_app.Classes
{
    public static class TaskManager
    {
        public static TaskStatus GetValidTaskStatus()
        {
            string input = Console.ReadLine();
            while (input.ToLower() != "active" && input.ToLower() != "postponed" && input.ToLower() != "completed")
            {
                Console.Write("Invalid input. Enter (Active, Postponed, Completed): ");
                input = Console.ReadLine();
            }
            return (TaskStatus)Enum.Parse(typeof(TaskStatus), input, true);
        }

        public static void ShowTaskSubMenu(Task selectedTask)
        {
            var exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine($"Task: {selectedTask.Name}\n");
                Console.WriteLine("1. View task details");
                Console.WriteLine("2. Update task status");
                Console.WriteLine("0. Go back");

                var choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        ShowTaskDetails(selectedTask);
                        break;
                    case "2":
                        UpdateTaskStatus(selectedTask);
                        break;
                    case "0":
                        exit = true;
                        return;
                    default:
                        Console.WriteLine("Unknown option. Press any key to return...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void ShowTaskDetails(Task task)
        {
            Console.WriteLine($"Details of Task '{task.Name}':");
            Console.WriteLine($"  Description: {task.Description}");
            Console.WriteLine($"  Due Date: {task.DueDate.ToShortDateString()}");
            Console.WriteLine($"  Status: {task.Status}");
            Console.WriteLine($"  Expected Duration: {task.ExpectedDurationMinutes} minutes");
            Console.WriteLine("\nPress any key to return ...");
            Console.ReadKey();
        }

        private static void UpdateTaskStatus(Task task)
        {
            if (task.Status == TaskStatus.Completed)
            {
                Console.WriteLine("This task is already completed. You cannot change its status.");
                Console.WriteLine("Press any key to return ...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Current status: {task.Status}");
            Console.Write("Enter new status (Active, Completed, Postponed):");

            var statusInput = GetValidTaskStatus();
            task.Status = statusInput;

            Console.WriteLine($"\nTask status updated to {statusInput}.");
            Console.WriteLine("Press any key to return ...");
            Console.ReadKey();
        }


    }
}
