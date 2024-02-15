using Moq;
using NUnit.Framework;
using ProcessCsvFunction.Data.Abstraction;
using ProcessCsvFunction.Data.Models;
using ProcessCsvFunction.Services.Extensions;
using ProcessCsvFunction.Services.Models;
using ProcessCsvFunction.Services.Services;
using Serilog;

namespace ProcessCsvFunction.Services.Tests.Services
{
    [TestFixture]
    public class ProcessEnrichDataServiceTests
    {
        private MockRepository _mockRepository;

        private Mock<ILogger> _mockLogger;
        private Mock<IGleifService> _mockGleifService;
        private Mock<IEnrichedDataSetRepository> _mockEnrichedDataSetRepository;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockLogger = _mockRepository.Create<ILogger>();
            _mockGleifService = _mockRepository.Create<IGleifService>();
            _mockEnrichedDataSetRepository = _mockRepository.Create<IEnrichedDataSetRepository>();
        }

        private ProcessEnrichDataService CreateService()
        {
            return new ProcessEnrichDataService(
                _mockLogger.Object,
                _mockGleifService.Object,
                _mockEnrichedDataSetRepository.Object);
        }

        [Test]
        public async Task EnrichCsvAsync_WhenDataIsAsExpected_ThenLogNoErrorAndReturnTrue()
        {
            // Arrange
            var service = this.CreateService();
            var csvData = GetCsvString(20, true);
            string? fileName = "fileName.txt";

            _mockGleifService.Setup(x => x.GetLeiRecordByLeiAsync(It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(It.IsAny<LeiRecordRoot>);
            _mockEnrichedDataSetRepository.Setup(x => x.SaveEnrichedDataSetAsync(It.IsAny<EnrichedDataSet>())).ReturnsAsync(true);

            // Act
            var result = await service.EnrichCsvAsync(csvData.GenerateStreamFromString(), fileName);

            // Assert
            Assert.IsTrue(result);
            _mockGleifService.Verify(x => x.GetLeiRecordByLeiAsync(It.IsAny<IEnumerable<string>>()), Times.Exactly(2));
            _mockEnrichedDataSetRepository.Verify(x => x.SaveEnrichedDataSetAsync(It.IsAny<EnrichedDataSet>()), Times.Once);
            _mockRepository.VerifyAll();
        }

        [Test]
        public async Task EnrichCsvAsync_WhenNoHeadersFound_ThenLogErrorAndReturnFalse()
        {
            // Arrange
            var service = this.CreateService();
            var csvData = GetCsvString(20, false);
            string? fileName = "fileName.txt";

            _mockLogger.Setup(x => x.Error(It.IsAny<string>()));

            // Act
            var result = await service.EnrichCsvAsync(csvData.GenerateStreamFromString(), fileName);

            // Assert
            Assert.IsFalse(result);
            _mockGleifService.Verify(x => x.GetLeiRecordByLeiAsync(It.IsAny<IEnumerable<string>>()), Times.Never);
            _mockEnrichedDataSetRepository.Verify(x => x.SaveEnrichedDataSetAsync(It.IsAny<EnrichedDataSet>()), Times.Never);
            _mockLogger.Verify(x => x.Error("Invalid File Received: fileName.txt"), Times.Once);
            _mockRepository.VerifyAll();
        }

        [Test]
        public async Task CompileDataInBatchesAsync_IfMoreThan10RecordsInCsv_ThenReturnDataInBatchesOf10()
        {
            // Arrange
            var service = this.CreateService();
            var csvData = GetCsvString(20, false);
            var reader = new StreamReader(csvData.GenerateStreamFromString());

            // Act
            var result = await service.CompileDataInBatchesAsync(reader);

            // Assert
            Assert.That(result.Count, Is.EqualTo(10));
            reader.Close();
            _mockRepository.VerifyAll();
        }

        [Test]
        public async Task CompileDataInBatchesAsync_IfLessThan10RecordsInCsv_ThenReturnNumberOfRecords()
        {
            // Arrange
            var service = this.CreateService();

            var csvData = GetCsvString(8, false);
            var reader = new StreamReader(csvData.GenerateStreamFromString());

            // Act
            var result = await service.CompileDataInBatchesAsync(reader);

            // Assert
            Assert.That(result.Count, Is.EqualTo(8));
            reader.Close();
            _mockRepository.VerifyAll();
        }

        [Test]
        public void ValidateCsvHeaders_WhenHeaderIsAsExpected_ThenReturnTrue()
        {
            // Arrange
            var service = this.CreateService();
            string csvStreamHeader = "transaction_uti,isin,notional,notional_currency,transaction_type,transaction_datetime,rate,lei";

            // Act
            var result = service.ValidateCsvHeaders(csvStreamHeader);

            // Assert
            Assert.IsTrue(result);
        }


        [Test]
        public void ValidateCsvHeaders_WhenHeaderContainsLesserFields_ThenReturnFalse()
        {
            // Arrange
            var service = this.CreateService();
            string csvStreamHeader = "transaction_uti,isin,notional,transaction_type,transaction_datetime,rate,lei";

            // Act
            var result = service.ValidateCsvHeaders(csvStreamHeader);

            // Assert
            Assert.IsFalse(result);
        }

        private string GetCsvString(int numberOfRecords, bool includeHeaders)
        {
            string result = "";
            if (includeHeaders)
            {
                result = $"transaction_uti,isin,notional,notional_currency,transaction_type,transaction_datetime,rate,lei" +
                    $"\n{Guid.NewGuid().ToString()},{0},{0},{Guid.NewGuid().ToString()},{Guid.NewGuid().ToString()},{DateTime.Now},{0},{Guid.NewGuid().ToString()}";
            }
            else
            {
                result = $"{Guid.NewGuid().ToString()},{0},{0},{Guid.NewGuid().ToString()},{Guid.NewGuid().ToString()},{DateTime.Now},{0},{Guid.NewGuid().ToString()}";
            }

            for (int i = 1; i < numberOfRecords; i++)
            {
                result = result + $"\n{Guid.NewGuid().ToString()},{i},{i},{Guid.NewGuid().ToString()},{Guid.NewGuid().ToString()},{DateTime.Now},{i},{Guid.NewGuid().ToString()}";
            }

            return result;
        }
    }
}
