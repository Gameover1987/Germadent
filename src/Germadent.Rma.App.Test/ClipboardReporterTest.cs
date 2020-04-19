using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Rma.App.Reporting;
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
            var reporter = new ClipboardReporter(mockClipboard.Object);
             
            // When
            reporter.CreateReport(testData.Reports);

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
                    ExpectedData = "18"
                },
                new ReporterTestData
                {
                    Reports = new ReportListDto[]
                    {
                        new ReportListDto {Quantity = 18},
                    },
                    ExpectedData = "18"
                }
            };
        }
    }
}
