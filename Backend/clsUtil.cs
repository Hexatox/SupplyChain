namespace Backend
{
    public static class clsUtil
    {
        public static byte[] ConverToBinaryImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null; 

            using var memoryStream = new MemoryStream();
            file.CopyToAsync(memoryStream);
            byte[] imageData = memoryStream.ToArray();
            return imageData;
        }

        public static string SaveImage(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return null;

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return "/images/" + uniqueFileName;
        }

        public static bool DeleteImage(string? imagePath)
        {
            imagePath = "wwwroot" + imagePath;
            try
            {
                if (File.Exists(imagePath)) // Check if the file exists
                {
                    File.Delete(imagePath); // Delete the file
                    return true; // Successfully deleted
                }
                return false; // File does not exist
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {ex.Message}");
                return false;
            }
        }


    }
}
