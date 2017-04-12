using System.IO;
using System.Web.Mvc;
using BachMaiCR.Utilities;

namespace BachMaiCR.Web.Common.CustomActionResult
{
    public class DownloadResult : ActionResult
    {
        private readonly string _filePath;
        private readonly string _fileName;
        private readonly byte[] _fileData;
        public DownloadResult(string filePath)
        {
            _filePath = filePath;
        }

        public DownloadResult(byte[] fileData, string fileName)
        {
            this._fileName = fileName;
            this._fileData = fileData;
        }

        /// <summary>
        /// Trả về file
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (this._fileData != null)
            {
                context.HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + this._fileName.Replace(" ","_").ToLower());
                string mimeType = MimeType.GetMime(Path.GetExtension(this._fileName));
                if (!string.IsNullOrWhiteSpace(mimeType))
                {
                    context.HttpContext.Response.AddHeader("content-type", mimeType);
                }
                context.HttpContext.Response.BinaryWrite(_fileData);
            }
            else
            {
                string fileName = Path.GetFileName(_filePath);
                context.HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                string mimeType = MimeType.GetMime(Path.GetExtension(_filePath));
                if (!string.IsNullOrWhiteSpace(mimeType))
                {
                    context.HttpContext.Response.AddHeader("content-type", mimeType);
                }
                context.HttpContext.Response.TransmitFile(_filePath);
            }
        }
    }
}