using System;
using System.Text;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Reporting
{
    public interface IReporter
    {
        void CreateReport(ReportListDto[] reports);
    }

    public class ClipboardReporter : IReporter
    {
        private readonly IClipboardHelper _clipboard;

        public ClipboardReporter(IClipboardHelper clipboard)
        {
            _clipboard = clipboard;
        }

        public void CreateReport(ReportListDto[] reports)
        {
            var builder = new StringBuilder();

            foreach (var report in reports)
            {
                var line = string.Concat(report.Created == DateTime.MinValue ? string.Empty : report.Created.ToString(), "\t", report.DocNumber, "\t", report.Customer, "\t", report.EquipmentSubstring, "\t", report.Patient, "\t", report.ProstheticSubstring, "\t", report.MaterialsStr, "\t", report.ColorAndFeatures, "\t", report.Quantity, "\t", "\t", "\t", "\t", report.ProstheticArticul + "\n").Trim();
                builder.AppendLine(line);
            }

            var data = builder.ToString();

            _clipboard.CopyToClipboard(data);
        }
    }
}
