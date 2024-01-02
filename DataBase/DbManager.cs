using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.DataBase
{
    public abstract class DbManager
    {
        protected SQLiteConnection _dbConnection;

        // DbManager est le constructeur et il prend le chemin d'accès vers notre fichier
        // Chemin de fichier = Chemin relatif
        public DbManager(string dbPath)
        {
            _dbConnection = new SQLiteConnection($"Data source = {dbPath}");
        }

        // Ouverture de la connection à la DB
        protected void OpenConnection()
        {
            _dbConnection.Open();
        }



        // Fermeture de la connection à la DB
        protected void CloseConnection()
        {
            if (_dbConnection != null)
            {
                _dbConnection.Close();
            }
        }
        
    }
}
