using MambaMVC.Utilities.Enums;
using Microsoft.Identity.Client;

namespace MambaMVC.Utilities.Extentions
{
    public static class FileValidation
    {
        public static bool ValidateType(this IFormFile file, string type)
        {
            if (file.ContentType.Contains(type))
            {
                return true;
            }
            return false;
        }
        public static bool ValidateSize(this IFormFile file, FileSize filesize, int size)
        {
            switch (filesize)
            {
                case FileSize.KB:
                    return file.Length <= size * 1024;
                case FileSize.MB:
                    return file.Length <= size * 1024 * 1024;
            }
            return false;

        }

        public static string CreatePath(this string file, params string[] roots)
        {
            string path = string.Empty;
            for (int i = 0; i < roots.Length; i++)
            {
                path = Path.Combine(path, roots[i]);
            }
            path = Path.Combine(path, file);
            return path;
        }

        public async static Task<string> CreateFileAsync(this IFormFile File, params string[] roots)
        {
            string filename = File.Name;

            string file=string.Concat(Guid.NewGuid().ToString(), filename.Substring(filename.LastIndexOf(".")));

            using(FileStream fileStream=new(file.CreatePath(roots), FileMode.Create))
            {
                await File.CopyToAsync(fileStream);
            }
            return file;
        }

        public static void DeleteAsync(this string file,params string[] roots)
        {
            File.Delete(file.CreatePath(roots));
        }
    }
}
