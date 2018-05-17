using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

using SQLite;

using Gomando.Model.Models;


namespace Gomando.Repository.DataLayer
{
    /// <summary>
    /// TrainingDatabase buduje lub dobudowuje SQLite.Net i reprezentuje specyficzną bazę danych, w naszym przypadku.
    /// Zawiera metody do otrzymywania, 'persistance' a także tworzeniu bazy danych, wszystko bazuje na ORM.
    /// </summary>
    public class TrainingDatabase : SQLiteConnection
    {
        static object locker = new object();

        static string DatabaseFilePath
        {
            get
            {
                var sqliteFilename = "GomandoDB.db3";
                string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var path = Path.Combine(libraryPath, sqliteFilename);

                return path;
            }
        }

        public TrainingDatabase() : base(DatabaseFilePath)
        {
            CreateTable<Training>();
        }

        public IEnumerable<T> GetItems<T>() where T : IBusinessEntity, new()
        {
            lock (locker)
            {
                return (from i in Table<T>() select i).ToList();
            }
        }

        public T GetItem<T>(int id) where T : IBusinessEntity, new()
        {
            lock (locker)
            {
                return Table<T>().FirstOrDefault(x => x.Id == id);
            }
        }

        public int SaveItem<T>(T item) where T : IBusinessEntity
        {
            lock (locker)
            {
                if (item.Id != 0)
                {
                    Update(item);
                    return item.Id;
                }
                else
                {
                    return Insert(item);
                }
            }
        }

        public int DeleteItem<T>(int id) where T : IBusinessEntity, new()
        {
            lock (locker)
            {
                return Delete(new T() { Id = id });
            }
        }
    }
}