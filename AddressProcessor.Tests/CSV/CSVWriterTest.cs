using System;
using System.Linq;
using AddressProcessing.CSV;
using AddressProcessing.Wrappers;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace AddressProcessing.Tests.CSV
{
    [TestFixture]
    public class CSVWriterTest
    {
        private IFixture fixture;
        private Mock<IFileInfo> fileInfoMock;
        private Mock<IStreamWriter> streamWriterMock;

        private string fileName;
        private string[] columns;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            streamWriterMock = fixture.Create<Mock<IStreamWriter>>();
            fileInfoMock = fixture.Freeze<Mock<IFileInfo>>();
        }

        [TearDown]
        public void TearDown()
        {
            streamWriterMock.Reset();
            fileInfoMock.Reset();
        }

        [SetUp]
        public void Setup()
        {
            fileName = fixture.Create<string>();
            columns = fixture.CreateMany<string>().ToArray();

            fileInfoMock.Setup(x => x.CreateText()).Returns(streamWriterMock.Object);
        }

        [Test]
        public void Open_WhenInvoked_ShouldOpenFileForWrite()
        {
            // Arrange
            var subject = fixture.Create<CSVWriter>();

            // Act
            subject.Open(fileName);

            // Assert
            fileInfoMock.Verify(x => x.Initialize(fileName), Times.Once);
            fileInfoMock.Verify(x => x.CreateText(), Times.Once);
        }

        [Test]
        public void Close_WhenFileIsOpened_ShouldCloseStream()
        {
            // Arrange
            var subject = fixture.Create<CSVWriter>();
            subject.Open(fileName);

            // Act
            subject.Close();

            // Assert
            streamWriterMock.Verify(x => x.Close(), Times.Once);
        }

        [Test]
        public void Close_WhenFileIsNotOpened_ShouldNotCloseStream()
        {
            // Arrange
            var subject = fixture.Create<CSVWriter>();

            // Act
            subject.Close();

            // Assert
            streamWriterMock.Verify(x => x.Close(), Times.Never);
        }

        [Test]
        public void Write_WhenFileIsNotOpened_ShouldThrowException()
        {
            // Arrange
            var subject = fixture.Create<CSVWriter>();

            // Act
            TestDelegate action = () => subject.Write(columns);

            // Assert
            Assert.That(action, Throws.InstanceOf<NullReferenceException>());
        }

        [TestCase("col1\tcol2", "col1", "col2")]
        [TestCase("col1", "col1")]
        [TestCase("col1\tcol2\tcol3", "col1", "col2", "col3")]
        public void Write_WhenFileIsOpened_ShouldWriteLine(string expectedOutput, params string[] input)
        {
            // Arrange
            var subject = fixture.Create<CSVWriter>();
            subject.Open(fileName);

            // Act
            subject.Write(input);

            // Assert
            streamWriterMock.Verify(x => x.WriteLine(expectedOutput), Times.Once);
        }
    }
}
