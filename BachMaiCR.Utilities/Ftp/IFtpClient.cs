using System.IO;
using System.Net;

namespace BachMaiCR.Utilities.Ftp
{
    public interface IFtpClient
    {
        FtpStatusCode UploadFile(string sourcePath, string destinationPath);

        FtpStatusCode UploadFile(Stream fileStream, string destinationPath);

        FtpStatusCode CreateFolder(string folderPath);

        byte[] DownloadFile(string filePath);

        FtpStatusCode DeleteFile(string filePath);

        bool IsExistFolder(string folderPath);
    }
}