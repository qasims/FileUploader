using Chambers.PdfUploader.Models;
using Chambers.PdfUploader.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Chambers.PdfUploader
{
    public class FileUploaderService : IFileUploaderService
    {
        private const long maxPdfFileSize = 5242880;  //5MB
        private readonly IDatabaseRepository _databaseRepository;

        public FileUploaderService(IDatabaseRepository databaseService)
        {
            _databaseRepository = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
        }

        public List<IFile> GetFiles(bool sortDescending)
        {
            if(sortDescending)
            {
                return _databaseRepository.GetAll().OrderByDescending(o => o.Name).ToList();
            }

            return _databaseRepository.GetAll().OrderBy(o => o.Name).ToList(); ;
        }

        public List<IFile> AddFile(List<IFile> uploadedFiles)
        {
            List<IFile> result = new List<IFile>();

            foreach (var file in uploadedFiles)
            {
                var uploadedFile = _databaseRepository.Add(file);
                result.Add(uploadedFile);
            }

            return result;
        }

        public bool DeleteFile(Guid Id)
        {
            var isFileExists = _databaseRepository.Get(Id);

            if (isFileExists != null)
            {
                _databaseRepository.Remove(isFileExists.Id);
                return true;
            }

            return false;
        }

        public (bool, string) ValidateFile(List<IFile> uploadedfiles)
        {
            string errorMessage = null;
            foreach (var uploadedfile in uploadedfiles)
            {
                uploadedfile.SetMaxFileSize(maxPdfFileSize);

                if (!uploadedfile.IsAllowedSize())
                {
                    errorMessage ="Pdf file should not be more than 5MB, Please try again!";
                }

                if (!PdfFile.CheckIfPdfFile(uploadedfile.Name))
                {
                    errorMessage = "Please upload only pdf file(s)!";
                }
            }

            bool isValid = string.IsNullOrEmpty(errorMessage);

            return (isValid, errorMessage);
        }

        public async Task<byte[]> GetFileContentAsync(IFormFile file)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


}
