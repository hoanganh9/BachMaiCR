using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;

namespace BachMaiCR.Utilities.Ftp
{
  public class FtpClient : IFtpClient
  {
    private const string FTPSERVERKEY = "ftpServer";
    private const string FTPUSERNAMEKEY = "ftpUser";
    private const string FTPPASSWORDKEY = "ftpPassword";
    private const string FTPPROXYKEY = "ftpProxy";
    private static ICredentials ftpCredential;
    private static string ftpRootPath;
    private static IWebProxy ftpProxy;

    public FtpClient()
    {
      NameValueCollection appSettings = ConfigurationManager.AppSettings;
      FtpClient.ftpRootPath = appSettings["ftpServer"];
      FtpClient.ftpCredential = (ICredentials) new NetworkCredential(appSettings["ftpUser"], appSettings["ftpPassword"]);
      FtpClient.ftpProxy = string.IsNullOrWhiteSpace(appSettings["ftpProxy"]) ? null : (IWebProxy) new WebProxy(appSettings["ftpProxy"]);
      if (!string.IsNullOrWhiteSpace(FtpClient.ftpRootPath))
        ;
    }

    public FtpStatusCode UploadFile(string sourcePath, string destinationPath)
    {
      destinationPath = this.ClearnPath(destinationPath);
      string folderPath = this.GetFolderPath(destinationPath);
      if (!string.IsNullOrWhiteSpace(folderPath) && !this.IsExistFolder(folderPath))
      {
        int folder = (int) this.CreateFolder(folderPath);
      }
      WebRequest webRequest = WebRequest.Create(Path.Combine(FtpClient.ftpRootPath, destinationPath));
      webRequest.Proxy = FtpClient.ftpProxy;
      webRequest.Credentials = FtpClient.ftpCredential;
      webRequest.Method = "STOR";
      byte[] buffer = (byte[]) null;
      using (FileStream fileStream = System.IO.File.OpenRead(sourcePath))
        buffer = FtpClient.ReadFully((Stream) fileStream);
      webRequest.ContentLength = (long) buffer.Length;
      Stream requestStream = webRequest.GetRequestStream();
      requestStream.Write(buffer, 0, buffer.Length);
      requestStream.Close();
      FtpWebResponse response = (FtpWebResponse) webRequest.GetResponse();
      FtpStatusCode statusCode = response.StatusCode;
      response.Close();
      return statusCode;
    }

    public FtpStatusCode UploadFile(Stream fileStream, string destinationPath)
    {
      destinationPath = this.ClearnPath(destinationPath);
      string folderPath = this.GetFolderPath(destinationPath);
      if (!string.IsNullOrWhiteSpace(folderPath) && !this.IsExistFolder(folderPath))
      {
        int folder = (int) this.CreateFolder(folderPath);
      }
      destinationPath = this.ClearnPath(destinationPath);
      fileStream.Position = 0L;
      WebRequest webRequest = WebRequest.Create(Path.Combine(FtpClient.ftpRootPath, destinationPath));
      webRequest.Proxy = FtpClient.ftpProxy;
      webRequest.Credentials = FtpClient.ftpCredential;
      webRequest.Method = "STOR";
      byte[] buffer = FtpClient.ReadFully(fileStream);
      webRequest.ContentLength = (long) buffer.Length;
      Stream requestStream = webRequest.GetRequestStream();
      requestStream.Write(buffer, 0, buffer.Length);
      requestStream.Close();
      FtpWebResponse response = (FtpWebResponse) webRequest.GetResponse();
      FtpStatusCode statusCode = response.StatusCode;
      response.Close();
      return statusCode;
    }

    public FtpStatusCode CreateFolder(string folderPath)
    {
      folderPath = this.ClearnPath(folderPath);
      string[] strArray = folderPath.Split('/');
      if (folderPath.Any<char>())
      {
        string str = "";
        foreach (string path2 in strArray)
        {
          str = !string.IsNullOrWhiteSpace(str) ? Path.Combine(str, path2) : path2;
          if (!this.IsExistFolder(str))
          {
            WebRequest webRequest = WebRequest.Create(Path.Combine(FtpClient.ftpRootPath, str));
            webRequest.Proxy = FtpClient.ftpProxy;
            webRequest.Credentials = FtpClient.ftpCredential;
            webRequest.Method = "MKD";
            FtpWebResponse response = (FtpWebResponse) webRequest.GetResponse();
            FtpStatusCode statusCode = response.StatusCode;
            response.Close();
            return statusCode;
          }
        }
      }
      return FtpStatusCode.Undefined;
    }

    public byte[] DownloadFile(string filePath)
    {
      filePath = this.ClearnPath(filePath);
      FtpWebRequest ftpWebRequest = (FtpWebRequest) WebRequest.Create(Path.Combine(FtpClient.ftpRootPath, filePath));
      ftpWebRequest.Method = "RETR";
      ftpWebRequest.Credentials = FtpClient.ftpCredential;
      ftpWebRequest.Proxy = FtpClient.ftpProxy;
      FtpWebResponse response = (FtpWebResponse) ftpWebRequest.GetResponse();
      byte[] numArray = FtpClient.ReadFully(response.GetResponseStream());
      response.Close();
      return numArray;
    }

    public FtpStatusCode DeleteFile(string filePath)
    {
      filePath = this.ClearnPath(filePath);
      FtpWebRequest ftpWebRequest = (FtpWebRequest) WebRequest.Create(Path.Combine(FtpClient.ftpRootPath, filePath));
      ftpWebRequest.Method = "DELE";
      ftpWebRequest.Credentials = FtpClient.ftpCredential;
      ftpWebRequest.Proxy = FtpClient.ftpProxy;
      FtpWebResponse response = (FtpWebResponse) ftpWebRequest.GetResponse();
      FtpStatusCode statusCode = response.StatusCode;
      response.Close();
      return statusCode;
    }

    public bool IsExistFolder(string folderPath)
    {
      bool flag = true;
      try
      {
        folderPath = this.ClearnPath(folderPath);
        FtpWebRequest ftpWebRequest = (FtpWebRequest) WebRequest.Create(Path.Combine(FtpClient.ftpRootPath, folderPath));
        ftpWebRequest.Method = "NLST";
        ftpWebRequest.Credentials = FtpClient.ftpCredential;
        ftpWebRequest.Proxy = FtpClient.ftpProxy;
        FtpWebResponse response = (FtpWebResponse) ftpWebRequest.GetResponse();
        FtpStatusCode statusCode = response.StatusCode;
        response.Close();
        return statusCode == FtpStatusCode.OpeningData;
      }
      catch (WebException ex)
      {
        if (ex.Response != null)
        {
          if (((FtpWebResponse) ex.Response).StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
            flag = false;
        }
      }
      return flag;
    }

    private string ClearnPath(string path)
    {
      if (string.IsNullOrWhiteSpace(path))
        return path;
      path = path.Replace("//", "/").Replace("\\\\", "/").Replace("\\", "/");
      if (path.EndsWith("\\") || path.EndsWith("/"))
        path = path.Substring(0, path.Length - 1);
      return path;
    }

    private string GetFolderPath(string filePath)
    {
      if (string.IsNullOrWhiteSpace(filePath))
        return "";
      List<string> list = ((IEnumerable<string>) filePath.Split('/')).ToList();
      if (list.Count<string>() < 2)
        return "";
      list.RemoveAt(list.Count - 1);
      return string.Join("/", (IEnumerable<string>) list);
    }

    private static byte[] ReadFully(Stream input)
    {
      if (input == null)
        return new byte[0];
      if (input.CanSeek && input.Position > 0L)
        input.Position = 0L;
      byte[] buffer = new byte[16384];
      using (MemoryStream memoryStream = new MemoryStream())
      {
        int count;
        while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
          memoryStream.Write(buffer, 0, count);
        return memoryStream.ToArray();
      }
    }
  }
}
