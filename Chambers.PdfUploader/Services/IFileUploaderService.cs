using Chambers.PdfUploader.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chambers.PdfUploader
{
    public interface IFileUploaderService
    {
        List<IFile> AddFile(List<IFile> uploadedFiles);
        bool DeleteFile(Guid Id);
        List<IFile> GetFiles(bool sortDescending);
        (bool, string) ValidateFile(List<IFile> uploadedfiles);
        Task<byte[]> GetFileContentAsync(IFormFile file);
    }
}