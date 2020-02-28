using System;
using System.IO;
using System.Net.Http;

namespace Germadent.Rma.App.ServiceClient
{
    public interface IFileResponse : IDisposable
    {
        Stream GetFileStream();
    }

    public class FileResponse : IFileResponse
    {
        private readonly HttpResponseMessage _response;

        public FileResponse(HttpResponseMessage response)
        {
            _response = response;
        }

        public void Dispose()
        {
            _response.Dispose();
        }

        public Stream GetFileStream()
        {
            return _response.Content.ReadAsStreamAsync().Result;
        }
    }
}