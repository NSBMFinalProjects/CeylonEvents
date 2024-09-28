namespace EventHandler.Helper
{
    public class UploadHandler
    {
        public async Task<string> UploadAsync(IFormFile file)
        {
            List<string> validExtensions = new List<string> { ".jpg", ".png", ".gif" };
            string extension = Path.GetExtension(file.FileName).ToLower();

            if (!validExtensions.Contains(extension))
            {
                return $"Extension is not valid ({string.Join(", ", validExtensions)})";
            }

            long size = file.Length;
            if (size > (5 * 1024 * 1024))
            {
                return "Maximum size can be 5 MB";
            }

            string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path); // Create directory if it doesn't exist
            }

            string fileName = Guid.NewGuid().ToString() + extension;
            string fullPath = Path.Combine(path, fileName);
            using FileStream stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);
            return fileName;
        }
    }
}
