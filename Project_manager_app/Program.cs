using Project_manager_app.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Project_manager_app.Classes.Task;

namespace Project_manager_app
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Dictionary<Project, List<Task>> projects = new Dictionary<Project, List<Task>>();

            ProjectManager projectManager = new ProjectManager(projects);
            TaskManager taskManager = new TaskManager();

        }
    }
}
