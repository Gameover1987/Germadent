using System;
using System.IO;
using Germadent.Client.Common.Reporting.TemplateProcessing;
using Germadent.Client.Common.ServiceClient;
using Germadent.Common.Extensions;
using Germadent.Common.FileSystem;
using Germadent.Model;
using Germadent.UI.Infrastructure;

namespace Germadent.Client.Common.Reporting
{
    public class PrintModule : IPrintModule
    {
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IWordAssembler _wordAssembler;
        private readonly IFileManager _fileManager;
        private readonly IPrintableOrderConverter _converter;
        private readonly IBaseClientOperationsServiceClient _baseClientOperationsServiceClient;

        public PrintModule(IShowDialogAgent dialogAgent, IWordAssembler wordAssembler, IFileManager fileManager, IPrintableOrderConverter converter, IBaseClientOperationsServiceClient baseClientOperationsServiceClient)
        {
            _dialogAgent = dialogAgent;
            _wordAssembler = wordAssembler;
            _fileManager = fileManager;
            _converter = converter;
            _baseClientOperationsServiceClient = baseClientOperationsServiceClient;
        }

        public void Print(OrderDto order)
        {
            var template = _baseClientOperationsServiceClient.GetTemplate(GetDocumentTemplateTypeByWorkOrder(order));
            var printableOrder = _converter.ConvertFrom(order);
            var wordDocument = _wordAssembler.Assembly(template, printableOrder.SerializeToJson());

            const string fileFilter = "Word XML (*.docx)|*.docx";
            string fileName;
           
            if (_dialogAgent.ShowSaveFileDialog(fileFilter, GetOrderDocumentName(order), out fileName) != true) 
                return;

            _fileManager.Save(wordDocument, fileName);
            _fileManager.OpenFileByOS(fileName);
        }

        private DocumentTemplateType GetDocumentTemplateTypeByWorkOrder(OrderDto order)
        {
            switch (order.BranchType)
            {
                case BranchType.Laboratory:
                    return DocumentTemplateType.Laboratory;

                case BranchType.MillingCenter:
                    return DocumentTemplateType.MillingCenter;

                default:
                    throw new NotSupportedException("Неизвестный тип филиала");
            }
        }

        private string GetOrderDocumentName(OrderDto order)
        {
            switch (order.BranchType)
            {
                case BranchType.Laboratory:
                    return string.Format("Заказ-наряд в зуботехническую лабораторию №{0}", order.DocNumber);

                case BranchType.MillingCenter:
                    return string.Format("Заказ-наряд во фрезерный центр №{0}", order.DocNumber);

                default:
                    throw new NotSupportedException("Неизвестный тип филиала");
            }
        }
    }
}