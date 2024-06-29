namespace FileUploadAPI.Services
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file);
        Task<List<string>> UploadFilesAsync(IEnumerable<IFormFile> files);
        Task<byte[]> GetFileAsync(string filename);
        Task<bool> DeleteFileAsync(string filename);
    }
}
