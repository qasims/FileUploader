using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace PdfUploader.Models
{
    public interface IFile
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

        public void SetMaxFileSize(long maxFileSize);

        public bool IsAllowedSize();
    }
}