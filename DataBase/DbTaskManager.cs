using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManager.DataBase
{
    public class DbTaskManager : DbManager
    {
        public DbTaskManager(string dbPath) : base(dbPath)
        {
        }
        // Méthode permettant de créer une tâche
        public void CreateTask(Task task)
        {
            OpenConnection();
            SQLiteCommand cmd = new SQLiteCommand(_dbConnection);


            cmd.CommandText = $"INSERT INTO Task (Title, Description, Statut, Importance, Creation_date, Due_date, Completion_date) " +
                                        "VALUES (@title, @description, @statut, @importance, @creation_date, @due_date, @completion_date)";

            SQLiteParameter titleparameter = new SQLiteParameter("@title", task.Title);
            SQLiteParameter descriptionparameter = new SQLiteParameter("@description", task.Description);
            SQLiteParameter statusparameter = new SQLiteParameter("@statut", task.Statut);
            SQLiteParameter importanceparameter = new SQLiteParameter("@importance", task.Importance);
            SQLiteParameter creationparameter = new SQLiteParameter("creation_date", task.Creation_date);
            SQLiteParameter due_dateparameter = new SQLiteParameter("@due_date", task.Due_date);
            SQLiteParameter completionparameter = new SQLiteParameter("@completion_date", task.Completion_date);

            cmd.Parameters.Add(titleparameter);
            cmd.Parameters.Add(descriptionparameter);
            cmd.Parameters.Add(statusparameter);
            cmd.Parameters.Add(importanceparameter);
            cmd.Parameters.Add(creationparameter);
            cmd.Parameters.Add(due_dateparameter);
            cmd.Parameters.Add(completionparameter);



            cmd.ExecuteNonQuery();

            CloseConnection();


        }



        private Task GetTaskFromReader(SQLiteDataReader reader)
        {
            return new Task(Convert.ToInt32(reader["id"]),
                                        Convert.ToString(reader["Title"]),
                                        Convert.ToString(reader["Description"]),
                                        Convert.ToString(reader["Statut"]),
                                        Convert.ToInt32(reader["Importance"]),
                                        Convert.ToDateTime(reader["Creation_Date"]),
                                        Convert.ToDateTime(reader["Due_Date"]),
                                        Convert.ToDateTime(reader["Completion_Date"])
                                        );
        }
        // Méthode exécutant la CMD et renvoi un tâche
        private Task TaskSelected(SQLiteCommand cmd)
        {
            OpenConnection();

            SQLiteDataReader reader = cmd.ExecuteReader();

            Task taskChosen = null;


            while (reader.Read())
            {

                taskChosen = GetTaskFromReader(reader);
            }

            CloseConnection();

            return taskChosen;
        }

        // Methode  permettant de selcetionner une tâche spécifique 
        public Task GetSpecificTaskById(int id)
        {
            SQLiteCommand cmd = new SQLiteCommand(_dbConnection);
            cmd.CommandText = "SELECT * FROM Task WHERE id = @id";
            SQLiteParameter titleParameter = new SQLiteParameter("id", id);

            cmd.Parameters.Add(titleParameter);

            return TaskSelected(cmd);
        }
        // Methode permettant de supprimer une tâche
        public void DeleteTaskById(int id)
        {
            OpenConnection();
            SQLiteCommand cmd = new SQLiteCommand(_dbConnection);
            cmd.CommandText = "DELETE FROM Task WHERE id = @id";
            SQLiteParameter titleParameter = new SQLiteParameter("id", id);

            cmd.Parameters.Add(titleParameter);

            cmd.ExecuteNonQuery();

            CloseConnection();
        }
        // Méthode permettant de mettre à jour une tâche
        public void UpdateTask(Task task)
        {
            
            SQLiteCommand cmd = new SQLiteCommand(_dbConnection);

            OpenConnection();

            cmd.CommandText = "UPDATE Task SET Title = @title, Description = @description, " +
            "Due_Date = @due_date, Statut = @statut, Importance = @importance WHERE id = @id";
            cmd.Parameters.AddWithValue("@title", task.Title);
            cmd.Parameters.AddWithValue("@description", task.Description);
            cmd.Parameters.AddWithValue("@due_date", task.Due_date);
            cmd.Parameters.AddWithValue("@statut", task.Statut);
            cmd.Parameters.AddWithValue("@importance", task.Importance);
            cmd.Parameters.AddWithValue("@id", task.Id);

            cmd.ExecuteNonQuery();
            CloseConnection();
        }
        // Méthode éxécutant la commande afin de lire les éléments dans la base données.
        private List<Task> SelectTask(SQLiteCommand cmd)
        {
            OpenConnection();
            SQLiteDataReader reader = cmd.ExecuteReader();

            List<Task> Tasks = new List<Task>();

            while (reader.Read())
            {
                Tasks.Add(GetTaskFromReader(reader));
            }

            CloseConnection();

            return Tasks;
        }
        // Méthode permettant de voir toutes les tâches en cours ou non.
        public List<Task> GetAllTask()
        {
            OpenConnection();
            SQLiteCommand cmd = new SQLiteCommand(_dbConnection);
            cmd.CommandText = "SELECT * FROM Task";
            CloseConnection();
            return SelectTask(cmd);

        }

        // Méthode permettant de compléter une tâche

        public void CompleteTask(int taskId)
        {
            SQLiteCommand cmd = new SQLiteCommand(_dbConnection);
            OpenConnection();

            cmd.CommandText = "UPDATE Task SET " +
                                      "Statut = 'Completed', " +
                                      "Completion_date = @completion_date " +
                                      "WHERE id = @id";

            cmd.Parameters.AddWithValue("@completion_date", DateTime.Now);
            cmd.Parameters.AddWithValue("@id", taskId);

            cmd.ExecuteNonQuery();
            CloseConnection();

        }

        // Avanced fonctionnalities 
        // Method qui retourne la tâche selon l'importance 
        public List<Task> GetPriorityTasks(string statut)
        {
            OpenConnection();
            List<Task> tasks = new List<Task>();
            SQLiteCommand cmd = new SQLiteCommand(_dbConnection);
            cmd.CommandText = "SELECT * FROM Task WHERE Statut = @statut AND Importance = 3 ";
            cmd.Parameters.AddWithValue("@statut", statut);
            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Task task = GetTaskFromReader(reader);
                tasks.Add(task);
            }
            CloseConnection();
            return tasks;

        }

       
        

        public List<Task> GetDeadLineTask(string statut)
        {
            OpenConnection();
            List<Task> tasks = new List<Task>();
            SQLiteCommand cmd = new SQLiteCommand(_dbConnection);
            cmd.CommandText = "SELECT * FROM Task WHERE Statut = @statut AND Due_Date <= @offday ";
            cmd.Parameters.AddWithValue("@statut", statut);
            cmd.Parameters.AddWithValue("@offday", DateTime.Now.AddDays(3));
            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Task task = GetTaskFromReader(reader);
                tasks.Add(task);
            }
            CloseConnection();
            return tasks;
        }


        public List<Task> OverDueTask(string statut)
        {
            OpenConnection();
            List<Task> tasks = new List<Task>();
            SQLiteCommand cmd = new SQLiteCommand(_dbConnection); 
            cmd.CommandText = "SELECT * FROM Task WHERE Statut = @statut AND Due_date < @today";
            cmd.Parameters.AddWithValue("@statut", statut);
            cmd.Parameters.AddWithValue("@today", DateTime.Now);

            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Task task = GetTaskFromReader(reader);
                tasks.Add(task);
            }
            CloseConnection();
            return tasks;
        }
        public List<Task> GetByStatus(string statut)
        {
            OpenConnection();
            List<Task> tasks = new List<Task>();
            SQLiteCommand cmd = new SQLiteCommand(_dbConnection);
            
            cmd.CommandText = "SELECT * FROM Task WHERE Statut = @statut";
            cmd.Parameters.AddWithValue("@statut", statut);
           
            SQLiteDataReader reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                Task task = GetTaskFromReader(reader);
                tasks.Add(task);
            }
            CloseConnection();
            return tasks;
        }

    }
}

    

   
