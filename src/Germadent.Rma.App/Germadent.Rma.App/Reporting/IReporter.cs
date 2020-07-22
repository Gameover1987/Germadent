using System;
using System.Text;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Reporting
{
    public interface IReporter
    {
        void CreateReport(int workOrderId);
    }

    public class ClipboardReporter : IReporter
    {
        private readonly IClipboardHelper _clipboard;
        private readonly IRmaServiceClient _rmaServiceClient;

        public ClipboardReporter(IClipboardHelper clipboard, IRmaServiceClient rmaServiceClient)
        {
            //TODO Nekrasov:нул
            _clipboard = clipboard;
            _rmaServiceClient = rmaServiceClient;
        }

        public void CreateReport(int workOrderId)
        {
            var reports = _rmaServiceClient.GetWorkReport(workOrderId);
            if (reports.Length == 0)
                return;

            var builder = new StringBuilder();

            foreach (var report in reports)
            {
                //TODO Nekrasov: че бля? $"" не, не слышал?
                //TODO Nekrasov:адово длинная строка, легко накосячить, можно сделать многострочной
                var line = string.Concat(report.Created == DateTime.MinValue ? string.Empty : report.Created.ToString(), "\t", report.DocNumber, "\t", report.Customer, "\t", report.EquipmentSubstring, "\t", report.Patient, "\t", report.ProstheticSubstring, "\t", report.MaterialsStr, "\t", report.ColorAndFeatures, report.CarcassColor, "\t", report.Quantity, "\t", "\t", "\t", "\t", "\t", report.ProstheticArticul, "\t", report.ImplantSystem + "\n").Trim();
                builder.AppendLine(line);
            }

            var data = builder.ToString();

            _clipboard.CopyToClipboard(data);
        }
    }
}
