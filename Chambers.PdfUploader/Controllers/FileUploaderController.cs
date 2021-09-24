using Chambers.PdfUploader.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Chambers.PdfUploader.Controllers
{
    [Route("api/[controller]")]
    public class FileUploaderController : Controller
    {
        private readonly IFileUploaderService _fileUploaderService;

        public FileUploaderController(IFileUploaderService fileUploaderService)
        {
            _fileUploaderService = fileUploaderService ?? throw new System.ArgumentNullException(nameof(fileUploaderService));
        }

        // GET: FileUploaderController
        [Route("getall")]
        [HttpGet]
        public ActionResult<List<IFile>> GetAll(bool sortDescending = false)
        {
            return _fileUploaderService.GetFiles(sortDescending);
        }

        [HttpPost]
        [Route("upload")]
        public async Task<ActionResult<List<IFile>>> UploadPdfAsync(List<IFormFile> files, CancellationToken cancellationToken)
        {
    
            if (files == null || files.Count <= 0)
            {
                return BadRequest("Please select pdf file(s)!");
            }


            List<IFile> filesToUpload = new List<IFile>();

            foreach (var file in files)
            {
                var fileContent = await _fileUploaderService.GetFileContentAsync(file);
                IFile pdffile = new PdfFile
                {
                    Id = Guid.NewGuid(),
                    Name = file.FileName,
                    Size = file.Length,
                    Content = fileContent,
                };

                filesToUpload.Add(pdffile);
            }

            var (isValid, errorMessage) = _fileUploaderService.ValidateFile(filesToUpload);
            if (!isValid)
            {
                return BadRequest(errorMessage);
            }


            List<IFile> uploadedFiles = _fileUploaderService.AddFile(filesToUpload);

            if (uploadedFiles != null)
            {
                return Ok(uploadedFiles);
            }

            return BadRequest("File(s) not uploaded!");
        }
      
        [Route("delete")]
        [HttpGet]
        public ActionResult Delete(Guid Id)
        {
            var isDeleted = _fileUploaderService.DeleteFile(Id);
            if (isDeleted)
            {
                return Ok("File deleted!");
            }

            return NotFound("File you trying to delete not foud!");
        }
    }
}
