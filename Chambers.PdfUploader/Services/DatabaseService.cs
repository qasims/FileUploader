using Chambers.PdfUploader.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chambers.PdfUploader.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IMongoCollection<IFile> _files;

        private readonly IMongoDatabase database;

        public DatabaseService(IDatabaseStoreSettings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var client = new MongoClient(settings.ConnectionString);
            database = client.GetDatabase(settings.DatabaseName);
            _files = database.GetCollection<IFile>(settings.FileCollectionName);

        }

        public List<IFile> Get()
        {
            return _files.Find(file => true).ToList();
        }

        public IFile Get(Guid Id)
        {
            var filter = Builders<IFile>.Filter.Eq("_id", Id);
            return _files.Find<IFile>(filter).FirstOrDefault();
        }

        public IFile Add(IFile pdfFile)
        {
            _files.InsertOne(pdfFile);
            return pdfFile;
        }

        public void Remove(IFile fileIn)
        {
            _files.DeleteOne(file => file.Id == fileIn.Id);
        }

        public void Remove(Guid Id)
        {
            var filter = Builders<IFile>.Filter.Eq("_id", Id);
            _files.DeleteOne(filter) ;
        }
    }
}
