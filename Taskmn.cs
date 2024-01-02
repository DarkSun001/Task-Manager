using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager
{
    public class Task
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            private set { _id = value; }
        }


        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _statut;

        public string Statut
        {
            get { return _statut; }
            set
            {
                _statut = value;
            }

        }

        private int _importance;

        public int Importance
        {
            get { return _importance; }
            set
            {
                _importance = value;
            }
        }

        private DateTime _creation_date;
        public DateTime Creation_date
        {
            get { return _creation_date; }
            set{ _creation_date = value;}
        }

        private DateTime _due_date;
        public DateTime Due_date
        {
            get { return _due_date; }
            set
            {
                _due_date = value;
            }
        }
        private DateTime _completion_date;
        public DateTime Completion_date
        {
            get { return _completion_date; }
            set
            {
                _completion_date = value;
            }
        }






        public Task(int id, string title, string description, string statut, int importance, DateTime creation_date, DateTime due_date, DateTime completion_date)
        {
            Id = id;
            Title = title;
            Description = description;
            Statut = statut;
            Importance = importance;
            Creation_date = creation_date;
            Due_date = due_date;
            Completion_date = completion_date;
        }


    }

}

