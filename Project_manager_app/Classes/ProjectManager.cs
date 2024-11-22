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
    }
}
