using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PL.Helper
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file , string folderName)
        {
            // 1.Get Loacted folder path
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);
            //2.Get File Name  and Make it unique
            string fileName = $"{Guid.NewGuid()}{file.FileName}";
            //3.Get File Path [folder path , file name]
            string filePath = Path.Combine(folderPath, fileName);
            //4.Save File As Streams
            var FS = new FileStream(filePath, FileMode.Create);
            file.CopyTo(FS);
            //5.Return file name
            return fileName;
        }
        public static  void DeleteFile(string fileName, string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\files",folderName,folderName);
            if (File.Exists(filePath))
            {
            File.Delete(filePath);
            }
        }
    }
}
