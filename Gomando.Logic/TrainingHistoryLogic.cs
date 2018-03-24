using System.Collections.Generic;

using Gomando.Model.Models;
using Gomando.Repository.DataAccessLayer;

namespace Gomando.Logic
{
    public class TrainingHistoryLogic
    {
        private TrainingRepository trainingRepository;

        public TrainingHistoryLogic() => trainingRepository = new TrainingRepository();
        
        public List<Training> GetAllTrainings()
        {
            return trainingRepository.GetTrainings();
        }
    }
}
