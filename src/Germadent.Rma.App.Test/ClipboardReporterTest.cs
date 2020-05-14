using Germadent.Rma.App.Reporting;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.Model;
using Moq;
using NUnit.Framework;

namespace Germadent.Rma.App.Test
{
    public class ReporterTestData
    {
        public ReportListDto[] Reports { get; set; }

        public string ExpectedData { get; set; }
    }

    [TestFixture]
    public class ClipboardReporterTest
    {
        [TestCaseSource(nameof(GetTestData))]
        public void ShouldCopyReportdataToClipboard(ReporterTestData testData)
        {
            // Given
            var mockClipboard = new Mock<IClipboardHelper>();
            var mockServiceClient = new Mock<IRmaServiceClient>();
            mockServiceClient.Setup(x => x.GetWorkReport(2)).Returns(testData.Reports);
            var reporter = new ClipboardReporter(mockClipboard.Object, mockServiceClient.Object);
             
            // When
            reporter.CreateReport(2);

            // Then
            mockClipboard.Verify(x => x.CopyToClipboard(testData.ExpectedData));
        }

        private static ReporterTestData[] GetTestData()
        {
            return new ReporterTestData[]
            {
                new ReporterTestData
                {
                    Reports = new ReportListDto[]
                    {
                        new ReportListDto {Quantity = 18},
                    },
                    ExpectedData = "18\r\n"
                },
                new ReporterTestData
                {
                    Reports = new ReportListDto[]
                    {
                        new ReportListDto {Quantity = 18},
                    },
                    ExpectedData = "18\r\n"
                }
            };
        }
    }
}
