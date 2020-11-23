using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Company.DealSystem.Web.Controllers
{
    public class BaseController : ControllerBase
    {
        protected FileContentResult DownloadFile(byte[] fileContent, string fileName)
        {
            Response.Headers[HeaderNames.ContentDisposition] = new MimeKit.ContentDisposition
            {
                FileName = fileName,
                Disposition = MimeKit.ContentDisposition.Inline
            }.ToString();

            return new FileContentResult(fileContent, "application/octec-stream");
        }
    }
}
