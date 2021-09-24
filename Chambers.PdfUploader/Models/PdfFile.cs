using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Chambers.PdfUploader.Models
{
    public class PdfFile : IFile
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement]
        public string Name { get; set; }

        [BsonElement]
        public long Size { get; set; }

        [BsonElement]
        public string Type { get; set; }

        [BsonElement]
        public byte[] Content { get; set; }

        [BsonIgnore]
        public long MaxSize { get; set; }
  

        public bool IsAllowedSize()
        {
            return Size < MaxSize;
        }

        public void SetMaxFileSize(long maxFileSize)
        {
            MaxSize = maxFileSize;
        }

        public static bool CheckIfPdfFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            var extension = "." + fileName.Split('.')[fileName.Split('.').Length - 1];

            if (string.IsNullOrEmpty(extension) || !((extension?.ToLower()).Equals(".pdf")))
            {
                return false;
            }

            return true;
        }
    }
}
