using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gomando.Model.Models;
using Gomando.Repository.DataAccessLayer;

namespace Gomando.Logic
{
    public class AddManualTrainingLogic
    {
        private TrainingRepository trainingRepository;

        public AddManualTrainingLogic() => trainingRepository = new TrainingRepository();

        public int SaveTraining(Training training)
        {
            return trainingRepository.SaveTraining(training);
        }
    }
}
