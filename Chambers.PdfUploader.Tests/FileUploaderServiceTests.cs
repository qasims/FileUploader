using Chambers.PdfUploader.Models;
using Chambers.PdfUploader.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chambers.PdfUploader.Tests
{
    public class FileUploaderServiceTests
    {
        private FileUploaderService _sut;
        private readonly Mock<IDatabaseService> _databaseServieMock = new Mock<IDatabaseService>();
        List<IFile> files;
        IFile file;

        [SetUp]
        public void Setup()
        {
            _databaseServieMock.Setup(dbs => dbs.Add(It.IsAny<IFile>()));

            files = new List<IFile>();
            file = new PdfFile
            {
                Id = Guid.NewGuid(),
                Name = "file.pdf",
                Content = Enumerable.Repeat((byte)0x20, 100).ToArray(),
                Size = 200000
            };
        }

        [Test]
        public void When_Given_A_Pdf_File_Size_Must_Be_Less_Than_5MB()
        {
            //Arrange
            files.Add(file);

            // Act
            _sut = new FileUploaderService(_databaseServieMock.Object);
            var result =_sut.ValidateFile(files);

            // Assert
            Assert.AreEqual(true, result.Item1);
            Assert.IsNull(result.Item2);
        }

        [Test]
        public void When_Given_A_Pdf_File_Size_Must_Fail_Valiation_If_More_Than_5MB()
        {
            //Arrange
            file.Size = 20000000;
            files.Add(file);

            // Act
            _sut = new FileUploaderService(_databaseServieMock.Object);
            var result = _sut.ValidateFile(files);

            // Assert
            Assert.AreEqual(false, result.Item1);
            Assert.IsNotNull(result.Item2);
        }

        [Test]
        public void When_Given_A_File_Must_Be_A_Pdf_File()
        {
            //Arrange
            file.Name = "file.txt";
            files.Add(file);

            // Act
            _sut = new FileUploaderService(_databaseServieMock.Object);
            var result = _sut.ValidateFile(files);

            // Assert
            Assert.AreEqual(false, result.Item1);
            Assert.IsNotNull(result.Item2);
        }
    }
}