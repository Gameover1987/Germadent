using System;
using System.IO;
using Germadent.Common.Extensions;
using Germadent.Common.FileSystem;
using Germadent.Rma.App.Reporting.TemplateProcessing;
using Germadent.Rma.Model;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.Reporting
{
    public class PrintModule : IPrintModule
    {
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IWordAssembler _wordAssembler;
        private readonly IFileManager _fileManager;
        private readonly IPrintableOrderConverter _converter;

        private const string PathToMcTemplate = @"Templates\GermadentLab_MC.docx";
        private const string PathToZtlTemplate = @"Templates\GermadentLab_ZTL.docx";

        public PrintModule(IShowDialogAgent dialogAgent, IWordAssembler wordAssembler, IFileManager fileManager, IPrintableOrderConverter converter)
        {
            _dialogAgent = dialogAgent;
            _wordAssembler = wordAssembler;
            _fileManager = fileManager;
            _converter = converter;
        }

        public void Print(OrderDto order)
        {
            var pathToTemplate = GetTemplatePathForOrder(order);
            var template = _fileManager.ReadAllBytes(pathToTemplate);
            var printableOrder = _converter.ConvertFrom(order);
            var wordDocument = _wordAssembler.Assembly(template, printableOrder.SerializeToJson());

            const string fileFilter = "Word XML (*.docx)|*.docx";
            string fileName;
            if (_dialogAgent.ShowSaveFileDialog(fileFilter, GetOrderDocumentName(order), out fileName) == true)
            {
                _fileManager.Save(wordDocument, fileName);
                _fileManager.OpenFileByOS(fileName);
            }
        }

        private string GetTemplatePathForOrder(OrderDto order)
        {
            var fullPathToTemplate = string.Empty;
            if (order.BranchType == BranchType.Laboratory)
            {
                fullPathToTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PathToZtlTemplate);
            }
            else if (order.BranchType == BranchType.MillingCenter)
            {
                fullPathToTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PathToMcTemplate);
            }

            return fullPathToTemplate;
        }

        private string GetOrderDocumentName(OrderDto order)
        {
            if (order.BranchType == BranchType.Laboratory)
            {
                return string.Format("Заказ-наряд в зуботехническую лабораторию №{0}", order.DocNumber);
            }
            else if (order.BranchType == BranchType.MillingCenter)
            {
                return string.Format("Заказ-наряд во фрезерный центр №{0}", order.DocNumber);
            }

            throw new NotSupportedException("Неизвестный тип филиала");
        }
    }
}