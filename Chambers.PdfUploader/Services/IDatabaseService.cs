using Chambers.PdfUploader.Models;
using System;
using System.Collections.Generic;

namespace Chambers.PdfUploader.Services
{
    public interface IDatabaseService
    {
        IFile Add(IFile pdfFile);
        List<IFile> Get();
        IFile Get(Guid id);
        void Remove(IFile fileIn);
        void Remove(Guid id);
    }
}