using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Helper
{
    public class FileHelper
    {
        #region
        public string SaveFile(string path, string oldfilename, IFormFile file)
        {
            string FileName = "";
            string filePath = "";
            if (!Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(path);
            }

            FileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            filePath = Path.Combine(path, FileName);

            //delete old file if edit the record
            if (oldfilename != null)
            {
                string oldpath= Path.Combine(path, oldfilename);
                if (System.IO.File.Exists(oldpath))
                {
                    System.IO.File.Delete(oldpath);
                }
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return FileName;
        }

        public string DeleteFileAnyException(string path, string filename)
        {
            string filePath = "";
            filePath = Path.Combine(path, filename);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            return filename;
        }
        public string SaveFile1(string path, IFormFile file)
        {
            string FileName = "";
            string filePath = "";
            if (!Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(path);
            }
            FileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            filePath = Path.Combine(path, FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return FileName;
        }
        public MemoryStream GetMemory(string path)
        {
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;
            return memory;
        }
        public string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        public Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
        #endregion
    }
}
