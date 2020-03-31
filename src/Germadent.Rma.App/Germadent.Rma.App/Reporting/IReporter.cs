using System.Windows;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Reporting
{
    public interface IReporter
    {
        void CreateReport(ReportListDto[] reports);
    }

    public class ClipboardReporter : IReporter
    {
        private readonly IClipboardWorks _clipboard;

        public ClipboardReporter(IClipboardWorks clipboard)
        {
            _clipboard = clipboard;
        }

        public void CreateReport(ReportListDto[] reports)
        {
            string stackString = null;
            foreach (var report in reports)
            {
                stackString += string.Concat(report.Created, "\t", report.DocNumber, "\t", report.Customer, "\t", report.EquipmentSubstring, "\t", report.Patient, "\t", report.ProstheticSubstring, "\t", report.MaterialsStr, "\t", report.ColorAndFeatures, "\t", report.Quantity, "\t", "\t", "\t", "\t", report.ProstheticArticul + "\n");
            }
            Clipboard.SetText(stackString);
        }
    }
}
