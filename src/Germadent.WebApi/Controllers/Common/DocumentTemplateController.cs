using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Germadent.Common.FileSystem;
using Germadent.Common.Logging;
using Germadent.Model;
using Microsoft.AspNetCore.Authorization;

namespace Germadent.WebApi.Controllers.Common
{
    [Route("api/Common/DocumentTemplate")]
    [ApiController]
    [Authorize]
    public class DocumentTemplateController : ControllerBase
    {
        private const string PathToMC = @"Templates\GermadentLab_MC.docx";
        private const string PathToZtl = @"Templates\GermadentLab_ZTL.docx";
        private const string PathToSalary = @"Templates\GermadentLab_Salary.docx";

        private readonly ILogger _logger;
        private readonly IFileManager _fileManager;

        public DocumentTemplateController(ILogger logger, IFileManager fileManager)
        {
            _logger = logger;
            _fileManager = fileManager;
        }

        [HttpGet("GetDocumentTemplate/{documentTemplateType}")]
        public IActionResult GetDocumentTemplate(DocumentTemplateType documentTemplateType)
        {
            try
            {
                _logger.Info(nameof(GetDocumentTemplate));
                var template = _fileManager.ReadAllBytes(GetTemplateFilePath(documentTemplateType));
                return Ok(template);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return BadRequest(exception);
            }
        }

        private static string GetTemplateFilePath(DocumentTemplateType documentTemplateType)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            switch (documentTemplateType)
            {
                case DocumentTemplateType.Laboratory:
                    return Path.Combine(baseDirectory, PathToZtl);

                case DocumentTemplateType.MillingCenter:
                    return Path.Combine(baseDirectory, PathToMC);
                
                case DocumentTemplateType.Salary:
                    return Path.Combine(baseDirectory, PathToSalary);

                default:
                    throw new InvalidOperationException("Неизвестный тип шаблона");
            }
        }
    }
}
