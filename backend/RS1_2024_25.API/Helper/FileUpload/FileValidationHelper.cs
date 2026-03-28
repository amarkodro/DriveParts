using System;
using System.Linq;

namespace RS1_2024_25.API.Helper.FileUpload
{
    public class FileValidationHelper
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

        private static readonly Dictionary<string, byte[]> MagicBytes = new()
        {
            { ".jpg",  new byte[] { 0xFF, 0xD8, 0xFF } },
            { ".jpeg", new byte[] { 0xFF, 0xD8, 0xFF } },
            { ".png",  new byte[] { 0x89, 0x50, 0x4E, 0x47 } },
            { ".webp", new byte[] { 0x52, 0x49, 0x46, 0x46 } }
        };

        private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5MB

        public static (bool isValid, string errorMessage) Validate(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return (false, "No file uploaded.");

            if (file.Length > MaxFileSizeBytes)
                return (false, "File size exceeds 5MB limit.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
                return (false, "Only JPG, PNG and WebP files are allowed.");

            // Magic bytes provjera
            using var reader = new BinaryReader(file.OpenReadStream());
            var headerBytes = reader.ReadBytes(4);

            if (!MagicBytes.TryGetValue(extension, out var expectedBytes))
                return (false, "Invalid file type.");

            for (int i = 0; i < expectedBytes.Length; i++)
            {
                if (i >= headerBytes.Length || headerBytes[i] != expectedBytes[i])
                    return (false, "File content does not match its extension.");
            }

            return (true, string.Empty);
        }

        public static string GenerateSafeFileName(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return Guid.NewGuid().ToString() + extension;
        }
    }
}
