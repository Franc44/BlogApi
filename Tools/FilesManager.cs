using System;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace BlogApi.Tools
{
    public class FilesManager
    {
        public FilesManager()
        {

        }

        public static bool EliminaArchivo(string FileName, string Directory)
        {
            var folderName = Path.Combine("StaticFiles", Directory);
            var pathToSave = Path.Combine("C:", folderName);
            var filepath = Path.Combine(pathToSave,FileName);

            try
            {
                if(System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }

                return true;
            }  
            catch(Exception es)
            {
                Console.WriteLine(es);
                return false;
            } 
        }

        public static string GuardaArchivo(IFormFile file, string Directory)
        {
            var rand = new Random();
            int Id = rand.Next(1000);

            try
            {
                var folderName = Path.Combine("StaticFiles", Directory);
                var pathToSave = Path.Combine("C:", folderName);

                if (file.Length > 0)
                {
                    var fileName = Id + "." + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                       file.CopyTo(stream);
                    }

                    return fileName;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception es)
            {
                Console.WriteLine(es);
                return null;
            }
        }
    }
}