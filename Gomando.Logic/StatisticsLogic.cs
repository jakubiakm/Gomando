using Gomando.Model.Enums;
using Gomando.Model.Models;
using Gomando.Repository.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomando.Logic
{
    public class StatisticsLogic
    {
        private TrainingRepository trainingRepository;

        public StatisticsLogic() => trainingRepository = new TrainingRepository();

        public TrainingTypeStatistic GetTrainingTypeStatistic(TrainingType? type = null)
        {

            var trainingTypeStatistics = new TrainingTypeStatistic();
            var trainings = trainingRepository.GetTrainings();
            
            if (type == null)
            {

                trainingTypeStatistics.Count = trainings.Count();
                if (trainingTypeStatistics.Count > 0)
                {
                    trainingTypeStatistics.AllDistance = trainings
                                    .Sum(training => training.Distance);
                    trainingTypeStatistics.AllTime = (int)trainings
                                    .Sum(training => training.Time);
                    trainingTypeStatistics.AverageDistance = trainings
                                    .Average(training => training.Distance);
                    trainingTypeStatistics.AverageTime = (int)trainings
                                    .Average(training => training.Time);
                    trainingTypeStatistics.AverageTempo = trainingTypeStatistics.AllTime / 60 / trainingTypeStatistics.AllDistance;
                    trainingTypeStatistics.AverageVelocity = trainingTypeStatistics.AllDistance / ((double)trainingTypeStatistics.AllTime / 60 / 60);
                }
            }
            else
            {
                trainingTypeStatistics.Count = trainings.Count(training => training.Type == type);
                if (trainingTypeStatistics.Count > 0)
                {
                    trainingTypeStatistics.AllDistance = trainings
                                    .Where(training => training.Type == type)
                                    .Sum(training => training.Distance);
                    trainingTypeStatistics.AllTime = (int)trainings
                                    .Where(training => training.Type == type)
                                    .Sum(training => training.Time);
                    trainingTypeStatistics.AverageDistance = trainings
                                    .Where(training => training.Type == type)
                                    .Average(training => training.Distance);
                    trainingTypeStatistics.AverageTime = (int)trainings
                                    .Where(training => training.Type == type)
                                    .Average(training => training.Time);
                    trainingTypeStatistics.AverageTempo = trainingTypeStatistics.AllTime / 60 / trainingTypeStatistics.AllDistance;
                    trainingTypeStatistics.AverageVelocity = trainingTypeStatistics.AllDistance / ((double)trainingTypeStatistics.AllTime / 60 / 60);
                }

            }
            return trainingTypeStatistics;
        }
    }
}
