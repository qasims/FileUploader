using PdfUploader.Models;
using System;
using System.Collections.Generic;

namespace PdfUploader.Services
{
    public interface IDatabaseRepository
    {
        IFile Add(IFile pdfFile);
        List<IFile> GetAll();
        IFile Get(Guid id);
        void Remove(IFile fileIn);
        void Remove(Guid id);
    }
}