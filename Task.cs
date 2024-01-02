using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DataBase;
using Task = System.Threading.Tasks.Task;
using TaskManagerTask = TaskManager.Task;

namespace TaskManager
{
    internal class Program
    {
        private static DbManager dbManager;
        private static DbTaskManager taskManager;


        static void Main(string[] args)
        {
            string currentDirectory = Environment.CurrentDirectory;
            string dbFileName = "task.db";
            string dbPath = Path.Combine(currentDirectory, dbFileName);
            taskManager = new DbTaskManager(dbPath);
            
            while (true)
            {
                
                
                ShowMenu();

            }
        }
        public static void ShowMenu()
        {
            Console.WriteLine("°°°°°°°°°°°°°°°°°°°° Welcome to our Task Management App °°°°°°°°°°°°°°°°°° ");

            Console.WriteLine("°°°°°°°°°°°°°°°°°°°°  MENU  °°°°°°°°°°°°°°°°°°°°°°°°");
            Console.WriteLine("1- Create A Task");
            Console.WriteLine("2- Modify a Task");
            Console.WriteLine("3- Delete a Task");
            Console.WriteLine("4- View a Task");
            Console.WriteLine("5- End A Task");
            Console.WriteLine("6- Exit");
            Console.WriteLine("°°°°°°°°°°°°°°°°°°°°  MENU  °°°°°°°°°°°°°°°°°°°°°°°°");

            Console.WriteLine("Enter your choice : ");

            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    CreateTask();
                    break;
                case 2:
                    ModifyTask();
                    break;
                case 3:
                    DeleteTask();
                    break;
                case 4:
                    ViewTaskMenu();
                    break;
                case 5:
                    EndTask();
                    break;
                case 6:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }

       
        public static void CreateTask()
        {

            Console.WriteLine("Enter the task details");
            Console.WriteLine("Title : ");
            string title = Console.ReadLine();
            Console.WriteLine("Description : ");
            string description = Console.ReadLine();
            Console.WriteLine("Statut : ");
            string statut = Console.ReadLine();
            Console.WriteLine("Importance, give a number between one and 3: ");
            int importance = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Due date (dd-MM-yyyy) : ");
            DateTime dueDate = DateTime.Parse(Console.ReadLine());
            
            DateTime creationDate = DateTime.Now;
            DateTime completionDate = DateTime.MinValue;

            Task newTask = new Task(0,title,description,statut,importance,dueDate,creationDate,completionDate);
            taskManager.CreateTask(newTask);
            Console.WriteLine("Task added successfully");
        }

        public static void ModifyTask()
        {
            Console.WriteLine("You can modify the task its Id");
            Console.WriteLine("Enter the Id : ");
            int id = Convert.ToInt32(Console.ReadLine());
            Task taskModified = taskManager.GetSpecificTaskById(id);
            if (taskModified == null)
            {
                Console.WriteLine("Task not found");
            }

            Console.WriteLine("Enter the task details");
            Console.WriteLine("Title : ");
            string title = Console.ReadLine();
            Console.WriteLine("Description : ");
            string description = Console.ReadLine();
            Console.WriteLine("Statut : ");
            string statut = Console.ReadLine();
            Console.WriteLine("Importance, give a number between one and 3: ");
            int importance = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Due date (dd-MM-yyyy) : ");
            DateTime dueDate = DateTime.Parse(Console.ReadLine());

            taskModified.Title = title;
            taskModified.Description = description;
            taskModified.Statut = statut;
            taskModified.Due_date = dueDate;
            
            taskManager.UpdateTask(taskModified);
            Console.WriteLine("Task Updated successfully");


        }

        public static void DeleteTask()
        {
            Console.WriteLine("Enter the Id of the task you want to delete ? ");
            int id = Convert.ToInt32(Console.ReadLine());
            taskManager.DeleteTaskById(id);
            Console.WriteLine("Task Delete successfully");
            
        }
        public static void EndTask()
        {
            Console.WriteLine("Enter the Id of the task you want to mark as completed ? ");
            int id = Convert.ToInt32(Console.ReadLine());
            Task task = taskManager.GetSpecificTaskById(id);
            if (task == null)
            {
                Console.WriteLine("Task Not Found");
            }
            task.Completion_date = DateTime.Now;
            taskManager.UpdateTask(task);
            Console.WriteLine("Task Completed succesfully");

        }

        public static void ViewTaskMenu()
        {
            while (true)
            {
                Console.WriteLine("1. View a task");
                Console.WriteLine("2. View all tasks with 'In progress' status");
                Console.WriteLine("3. View all tasks with 'In progress' status ending in less than 3 days");
                Console.WriteLine("4. View all tasks with 'In progress' status and level 3 importance");
                Console.WriteLine("5. View all tasks with 'In progress' status whose Due Date has passed");
                Console.WriteLine("6. Return to the main menu");

                Console.WriteLine("Enter your choice : ");

                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        ViewTask();
                        break;
                    case 2:

                        ViewTaskStatut("In Progress");
                       ;
                        break;
                    case 3:
                        ViewDeadLineTask();
                        break;
                    case 4:
                        ViewPriority(); 
                        break;
                    case 5:
                        ViewOverDueTask();
                        break;
                    case 6:
                        ShowMenu();
                        break;
                        
                }
            }
        }
        public static void DisplayTask(Task task)
        {
            Console.WriteLine($"[TASK #{task.Id}]");
            Console.WriteLine($"Title: {task.Title}");
            Console.WriteLine($"Description: {task.Description}");
            Console.WriteLine($"Due Date: {task.Due_date.ToString("dd-MM-yyyy")}");
            Console.WriteLine($"Status: {task.Statut}");
            Console.WriteLine($"Importance: {task.Importance}");

            if (task.Statut == "Completed")
            {
                Console.WriteLine($"Completion Date: {task.Completion_date.ToString("dd-MM-yyyy")}");
            }

            Console.WriteLine("-----");
        }
        public static void ViewTask()
        {
            Console.WriteLine("Enter the Id of the task you want to see ? ");
            int id = Convert.ToInt32(Console.ReadLine());
            Task task =taskManager.GetSpecificTaskById(id);
            if (task == null)
            {
                Console.WriteLine("Task not found!");
                return;
            }
            DisplayTask(task);

        }

        public static void ViewTaskStatut(string statut)
        {
            List<Task> tasks = taskManager.GetByStatus(statut);
            foreach (Task task in tasks)
            {
                DisplayTask(task);
            }
        }

        public static void ViewDeadLineTask()
        {
            List<Task> tasks = taskManager.GetDeadLineTask("In Progress");
            foreach (Task task in tasks)
            {
                DisplayTask(task);
            }
        }
        public static void ViewPriority()
        {
            List<Task> tasks = taskManager.GetPriorityTasks("In Progress");
            foreach (Task task in tasks)
            {
                DisplayTask(task);
            }
        }
        public static void ViewOverDueTask()
        {
            List<Task> tasks = taskManager.OverDueTask("In Progress");
            foreach (Task task in tasks)
            {
                DisplayTask(task);
            }
        }

    }

    
        
}


