using Germadent.Client.Common.Infrastructure;
using Germadent.Model;
using Germadent.Rma.App.Reporting;
using Germadent.Rma.App.ServiceClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Germadent.Rma.App.Test
{
    public class ReporterTestData
    {
        public ReportListDto[] Reports { get; set; }

        public string ExpectedData { get; set; }
    }

    [TestClass]
    public class ClipboardReporterTest
    {
        [DataSource(nameof(GetTestData))]
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
                        new ReportListDto {DocNumber = "3030-MC~20", Customer = "ООО СК МЕЧКОВСКИХ", EquipmentSubstring = "1", Patient = "Воинцев", Quantity = 1},
                    },
                    ExpectedData = "3030-MC~20\tООО СК МЕЧКОВСКИХ\t1\tВоинцев\t\t\t\t1\r\n"
                }
            };
        }
    }
}
