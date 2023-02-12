using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Areas.Extensions
{
    public static class FileReader
    {
        public static bool IsImage(this IFormFile formFile)
        {
            return formFile.ContentType.Contains("image/");
        }
        public static bool IsSizeOk(this IFormFile FormFile, int Num)
        {
            return FormFile.Length / 1024 / 1024 <= Num;
        }

        public static string SaveImage(this IFormFile FirmFile, string root, string folder)
        {
            string RootPath = Path.Combine(root, folder);
            string FileName = Guid.NewGuid().ToString() + FirmFile.FileName;
            string FullPath = Path.Combine(RootPath, FileName);
            using (FileStream fileStream = new FileStream(FullPath, FileMode.Create))
            {
                FirmFile.CopyTo(fileStream);
            }
            return FileName;
        }
    }
}
