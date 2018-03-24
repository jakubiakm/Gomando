using System.Collections.Generic;
using System.Linq;

using Gomando.Model.Models;


namespace Gomando.Repository.DataAccessLayer
{
    public class TrainingRepository
    {
        Gomando.Repository.DataLayer.TrainingDatabase db = null;

        public TrainingRepository()
        {
            db = new DataLayer.TrainingDatabase();
        }

        public Training GetTraining(int id)
        {
            return db.GetItem<Training>(id);
        }

        public List<Training> GetTrainings()
        {
            return db.GetItems<Training>().ToList();
        }

        public int SaveTraining(Training item)
        {
            return db.SaveItem<Training>(item);
        }

        public int DeleteTraining(int id)
        {
            return db.DeleteItem<Training>(id);
        }
    }
}

