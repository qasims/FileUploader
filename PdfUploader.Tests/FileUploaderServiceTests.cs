using PdfUploader.Models;
using PdfUploader.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PdfUploader.Tests
{
    public class FileUploaderServiceTests
    {
        private FileUploaderService _sut;
        private readonly Mock<IDatabaseRepository> _databaseRepositoryMock = new Mock<IDatabaseRepository>();
        List<IFile> files;
        IFile file;

        [SetUp]
        public void Setup()
        {
            files = new List<IFile>();
            file = new PdfFile
            {
                Id = Guid.NewGuid(),
                Name = "file.pdf",
                Content = Enumerable.Repeat((byte)0x20, 100).ToArray(),
                Size = 200000
            };

            _databaseRepositoryMock.Setup(dbs => dbs.Get(It.IsAny<Guid>())).Returns(file);
            _databaseRepositoryMock.Setup(dbs => dbs.Add(It.IsAny<IFile>()));
            _databaseRepositoryMock.Setup(dbs => dbs.Remove(It.IsAny<Guid>()));
        }

        [Test]
        public void When_Given_A_Pdf_File_Size_Must_Be_Less_Than_5MB()
        {
            //Arrange
            files.Add(file);

            // Act
            _sut = new FileUploaderService(_databaseRepositoryMock.Object);
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
            _sut = new FileUploaderService(_databaseRepositoryMock.Object);
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
            _sut = new FileUploaderService(_databaseRepositoryMock.Object);
            var result = _sut.ValidateFile(files);

            // Assert
            Assert.AreEqual(false, result.Item1);
            Assert.IsNotNull(result.Item2);
        }

        [Test]
        public void When_Given_Get_A_File_Should_Call_Add_From_Database_Repository()
        {
            //Arrange
            files.Add(file);

            // Act
            _sut = new FileUploaderService(_databaseRepositoryMock.Object);
            var result = _sut.AddFile(files);

            // Assert
            _databaseRepositoryMock.Verify(d => d.Add(It.IsAny<IFile>()), Times.Once);
        }

        [TestCase(false, "A.pdf")]
        [TestCase(true, "Z.pdf")]
        public void When_Given_Get_All_Uploaded_File_With_Sort_Order_Should_Call_GetAll_From_Database_Repository(bool sortDescending, string fileName)
        {
            //Arrange
            file.Name = "A.pdf";
            var file1 = new PdfFile
            {
                Id = Guid.NewGuid(),
                Name = "Z.pdf",
                Content = Enumerable.Repeat((byte)0x20, 100).ToArray(),
                Size = 200000
            };

            files.Add(file);
            files.Add(file1);

            _databaseRepositoryMock.Setup(dbs => dbs.GetAll()).Returns(files);

            // Act
            _sut = new FileUploaderService(_databaseRepositoryMock.Object);
            var result = _sut.GetFiles(sortDescending);

            // Assert
            _databaseRepositoryMock.Verify(d => d.GetAll(), Times.AtLeastOnce);
            Assert.AreEqual(result[0].Name, fileName);
        }

        [Test]
        public void When_Given_Delete_A_File_Should_Call_Remove_From_Database_Repository()
        {
            //Arrange
            var fileId = Guid.NewGuid();

            // Act
            _sut = new FileUploaderService(_databaseRepositoryMock.Object);
            var result = _sut.DeleteFile(fileId);

            // Assert
            _databaseRepositoryMock.Verify(d => d.Get(It.IsAny<Guid>()), Times.Once);
            _databaseRepositoryMock.Verify(d => d.Remove(It.IsAny<Guid>()), Times.Once);
        }

    }
}