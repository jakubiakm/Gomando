using Gomando.Model.Models;
using Gomando.Repository.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Gomando.Logic
{
    public class TrainingLogic
    {
        private TrainingRepository trainingRepository;

        public TrainingLogic() => trainingRepository = new TrainingRepository();

        public void SaveTraining(Training training, List<List<Localization>> localizations)
        {
            IFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, localizations);
                training.SerializedLocalizations = stream.ToArray();
            }
            trainingRepository.SaveTraining(training);
        }
    }
}
