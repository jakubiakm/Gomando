using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gomando.Model.Models;
using Gomando.Repository.DataAccessLayer;

namespace Gomando.Logic
{
    public class TrainingDetailsLogic
    {
        private TrainingRepository trainingRepository = new TrainingRepository();

        public Training GetTraining(int trainingId)
        {
            return trainingRepository.GetTraining(trainingId);
        }

        public void DeleteTraining(int trainingId)
        {
            trainingRepository.DeleteTraining(trainingId);
        }

        public void EditTraining(Training training)
        {
            trainingRepository.SaveTraining(training);
        }
    }
}
