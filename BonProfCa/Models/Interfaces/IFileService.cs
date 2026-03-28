namespace BonProfCa.Services.Interfaces;

public interface IFileService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string folderPath);
    Task<(Stream Content, string ContentType)> DownloadFileAsync(string filePath);
    Task<bool> DeleteFileAsync(string filePath);
}