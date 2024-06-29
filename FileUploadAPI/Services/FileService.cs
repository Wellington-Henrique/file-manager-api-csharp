
namespace FileUploadAPI.Services
{
    public class FileService : IFileService
    {
        private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        private readonly long _fileSizeLimit = 1048576 * 1; // 1048576(bytes) * 1 = 1MB

        public FileService()
        {
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is invalid.");

            if (file.Length > _fileSizeLimit)
                throw new ArgumentException($"File size exceeds the limit of {_fileSizeLimit / (1024 * 1024)}MB.");

            var uniqueFileName = GenerateUniqueFileName(file.FileName);
            var filePath = Path.Combine(_storagePath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return uniqueFileName;
        }

        public async Task<List<string>> UploadFilesAsync(IEnumerable<IFormFile> files)
        {
            var filenames = new List<string>();

            foreach (var file in files)
            {
                var filename = await UploadFileAsync(file);
                filenames.Add(filename);
            }

            return filenames;
        }

        public async Task<byte[]> GetFileAsync(string filename)
        {
            var filePath = Path.Combine(_storagePath, filename);

            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found.");

            return await File.ReadAllBytesAsync(filePath);
        }

        public async Task<bool> DeleteFileAsync(string filename)
        {
            var filePath = Path.Combine(_storagePath, filename);

            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found.");

            File.Delete(filePath);

            return true;
        }

        private string GenerateUniqueFileName(string fileName)
        {
            var uniqueName = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(fileName);

            return uniqueName + extension;
        }
    }
}
