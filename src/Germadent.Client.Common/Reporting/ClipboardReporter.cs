using System;
using System.Text;
using Germadent.Client.Common.Infrastructure;

namespace Germadent.Client.Common.Reporting
{
    public class ClipboardReporter : IReporter
    {
        private readonly IClipboardHelper _clipboard;

        public ClipboardReporter(IClipboardHelper clipboard)
        {
            _clipboard = clipboard;
            //_rmaServiceClient = rmaServiceClient;
        }

        public void CreateReport(int workOrderId)
        {
            //var reports = _rmaServiceClient.GetWorkReport(workOrderId);
            //if (reports.Length == 0)
            //    return;

            //var builder = new StringBuilder();

            //foreach (var report in reports)
            //{
            //    var line = string.Concat(report.Created == DateTime.MinValue ? string.Empty : report.Created.ToString(), "\t", report.DocNumber, "\t", report.Customer, "\t", report.EquipmentSubstring, "\t", report.Patient, "\t", report.ProstheticSubstring, "\t", report.MaterialsStr, "\t", report.ConstructionColor, "\t", report.Quantity, "\t", "\t", "\t", "\t", "\t", report.ImplantSystem, "\t", report.TotalPriceCashless, "\t", report.TotalPrice, "\t", report.ResponsiblePerson, "\n").Trim();
            //    builder.AppendLine(line);
            //}

            //var data = builder.ToString();

            //_clipboard.CopyToClipboard(data);
        }
    }
}