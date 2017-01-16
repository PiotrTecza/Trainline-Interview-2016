using System;
using AddressProcessing.CSV;
using AddressProcessing.Wrappers;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace AddressProcessing.Tests.CSV
{
    [TestFixture]
    public class CSVReaderTest
    {
        private IFixture fixture;
        private Mock<IFileInfo> fileInfoMock;
        private Mock<IStreamReader> streamReaderMock;

        private string fileName;
        private string line;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            streamReaderMock = fixture.Create<Mock<IStreamReader>>();
            fileInfoMock = fixture.Freeze<Mock<IFileInfo>>();
        }

        [TearDown]
        public void TearDown()
        {
            streamReaderMock.Reset();
            fileInfoMock.Reset();
        }

        [SetUp]
        public void Setup()
        {
            fileName = fixture.Create<string>();
            line = fixture.Create<string>();

            fileInfoMock.Setup(x => x.OpenText()).Returns(streamReaderMock.Object);
            streamReaderMock.Setup(x => x.ReadLine()).Returns(() => line);
        }

        [Test]
        public void Open_WhenInvoked_ShouldOpenFileForRead()
        {
            // Arrange
            var subject = fixture.Create<CSVReader>();

            // Act
            subject.Open(fileName);

            // Assert
            fileInfoMock.Verify(x => x.Initialize(fileName), Times.Once);
            fileInfoMock.Verify(x => x.OpenText(), Times.Once);
        }

        [Test]
        public void Close_WhenFileIsOpened_ShouldCloseStream()
        {
            // Arrange
            var subject = fixture.Create<CSVReader>();
            subject.Open(fileName);

            // Act
            subject.Close();

            // Assert
            streamReaderMock.Verify(x => x.Close(), Times.Once);
        }

        [Test]
        public void Close_WhenFileIsNotOpened_ShouldNotCloseStream()
        {
            // Arrange
            var subject = fixture.Create<CSVReader>();

            // Act
            subject.Close();

            // Assert
            streamReaderMock.Verify(x => x.Close(), Times.Never);
        }

        [Test]
        public void Read_WhenFileIsNotOpened_ShouldThrowException()
        {
            // Arrange
            var subject = fixture.Create<CSVReader>();
            string column1, column2;

            // Act
            TestDelegate action = () => subject.Read(out column1, out column2);

            // Assert
            Assert.That(action, Throws.InstanceOf<NullReferenceException>());
        }

        [Test]
        public void Read_WhenNothingToRead_ShouldReturnFalse()
        {
            // Arrange
            var subject = fixture.Create<CSVReader>();
            line = null;
            subject.Open(fileName);
            string column1, column2;

            // Act
            var result =  subject.Read(out column1, out column2);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(column1, Is.Null);
            Assert.That(column2, Is.Null);
        }

        [Test]
        public void Read_WhenLineIsNotEmpty_ShouldReturnTrue()
        {
            // Arrange
            var subject = fixture.Create<CSVReader>();
            subject.Open(fileName);
            string column1, column2;

            // Act
            var result = subject.Read(out column1, out column2);

            // Assert
            Assert.That(result, Is.True);
        }

        [TestCase("col1\tcol2", "col1", "col2")]
        [TestCase("col1\t", "col1", "")]
        [TestCase("\tcol2", "", "col2")]
        public void Read_WhenInvoked_ShouldReadColumns(string input, string expectedColumn1, string expectedColumn2)
        {
            // Arrange
            var subject = fixture.Create<CSVReader>();
            line = input;
            subject.Open(fileName);
            string column1, column2;

            // Act
            var result = subject.Read(out column1, out column2);

            // Assert
            Assert.That(column1, Is.EqualTo(expectedColumn1));
            Assert.That(column2, Is.EqualTo(expectedColumn2));
        }
    }
}
