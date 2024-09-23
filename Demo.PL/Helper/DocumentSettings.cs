using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PL.Helper
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file , string FolderName)
        {
            // 1.Get Loacted folder path
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", FolderName);
            //2.Get File Name  and Make it unique
            string FileName = $"{Guid.NewGuid()}{file.FileName}";
            //3.Get File Path [folder path , file name]
            string FilePath = Path.Combine(folderPath, FileName);
            //4.Save File As Streams
            var FS = new FileStream(FilePath,FileMode.Create);
            //5.Return file name
            return FilePath;
        }
    }
}
