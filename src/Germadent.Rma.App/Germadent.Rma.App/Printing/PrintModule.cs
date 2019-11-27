using System;
using System.IO;
using Germadent.Common.Extensions;
using Germadent.Common.FileSystem;
using Germadent.Rma.App.Printing.Implementation;
using Germadent.Rma.Model;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.Printing
{
    public class PrintModule : IPrintModule
    {
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IWordAssembler _wordAssembler;
        private readonly IFileManager _fileManager;

        private const string PathToMcTemplate = @"Templates\GermadentLab_MC.docx";
        private const string PathToZtlTemplate = @"Templates\GermadentLab_ZTL.docx";

        public PrintModule(IShowDialogAgent dialogAgent, IWordAssembler wordAssembler, IFileManager fileManager)
        {
            _dialogAgent = dialogAgent;
            _wordAssembler = wordAssembler;
            _fileManager = fileManager;
        }

        public void Print(Order order)
        {
            var pathToTemplate = GetTemplatePathForOrder(order);
            var template = _fileManager.ReadAllBytes(pathToTemplate);
            var wordDocument = _wordAssembler.Assembly(template, order.SerializeToJson());

            const string fileFilter = "Word XML (*.docx)|*.docx";
            string fileName;
            if (_dialogAgent.ShowSaveFileDialog(fileFilter, GetOrderDocumentName(order), out fileName) == true)
            {
                _fileManager.Save(wordDocument, fileName);
            }
        }

        private string GetTemplatePathForOrder(Order order)
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

        private string GetOrderDocumentName(Order order)
        {
            if (order.BranchType == BranchType.Laboratory)
            {
                return string.Format("Заказ-наряд в зуботехническую лабораторию №{0}", order.Number);
            }
            else if (order.BranchType == BranchType.MillingCenter)
            {
                return string.Format("Заказ-наряд во фрезерный центр №{0}", order.Number);
            }

            throw new NotSupportedException("Неизвестный тип филиала");
        }
    }
}