using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chambers.PdfUploader
{
    public class DatabaseStoreSettings : IDatabaseStoreSettings
    {
        public string FileCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
