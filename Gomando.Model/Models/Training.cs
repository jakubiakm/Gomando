using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomando.Model.Models
{
    public class Training : IBusinessEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public double Distance { get; set; }

        public double Time { get; set; }

        public List<List<Localization>> Localizations { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
