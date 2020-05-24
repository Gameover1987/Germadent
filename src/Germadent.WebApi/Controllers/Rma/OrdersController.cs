using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Germadent.Common.FileSystem;
using Germadent.Rma.Model;
using Germadent.WebApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Germadent.WebApi.Controllers.Rma
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IRmaDbOperations _rmaDbOperations;
        private readonly IFileManager _fileManager;

        public OrdersController(IRmaDbOperations rmaDbOperations, IFileManager fileManager)
        {
            _rmaDbOperations = rmaDbOperations;
            _fileManager = fileManager;
        }

        [HttpGet("{id:int}")]
        public OrderDto GetWorkOrderById(int id)
        {
            return _rmaDbOperations.GetOrderDetails(id);
        }


        [HttpPost]
        public OrderDto AddOrder([FromBody] OrderDto orderDto)
        {
            return _rmaDbOperations.AddOrder(orderDto);
        }

        [HttpPost]
        [Route("fileUpload/{id}/{fileName}")]
        public void FileUpload(int id, string fileName)
        {
            var stream = Request.Form.Files.GetFile("DataFile").OpenReadStream();
            _rmaDbOperations.AttachDataFileToOrder(id, fileName, stream);
        }

        [HttpGet]
        [Route("fileDownload/{id}")]
        public FileStreamResult FileDownload(int id)
        {
            var fullFileName = _rmaDbOperations.GetFileByWorkOrder(id);
            if (fullFileName == null)
                return null;

            var stream = _fileManager.OpenFileAsStream(fullFileName);

            var fileStreamResult = new FileStreamResult(stream, "application/octet-stream");
            return fileStreamResult;
        }

        [HttpPut]
        public OrderDto UpdateOrder([FromBody] OrderDto orderDto)
        {
            Stream stream = null;

            _rmaDbOperations.UpdateOrder(orderDto, stream);

            return orderDto;
        }

        [HttpDelete("{id:int}")]
        public OrderDto CloseOrder(int id)
        {
            return _rmaDbOperations.CloseOrder(id);
        }
    }
}