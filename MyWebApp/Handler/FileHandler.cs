using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyWebApp.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp.Handler
{
    public class FileHandler
    {
        internal static async Task<Tuple<string, string, string>> UploadFileAsync(List<IFormFile> files, IHostingEnvironment env)
        {
            if (files == null) throw new ArgumentNullException(nameof(files), "files cannot be empty or null.");
            if (env == null) throw new ArgumentNullException(nameof(env), "env cannot be empty or null.");

            string sErr = "";
            List<String> fileNames = new List<string>();
            string fileName = "";
            string rawDataFolderPath = "";
            try
            {

                if (env.IsDevelopment())
                {
                    rawDataFolderPath = Path.Combine(env.ContentRootPath, "bin", "Debug", "RawData");
                }
                else
                {
                    rawDataFolderPath = Path.Combine(Path.GetDirectoryName(env.ContentRootPath), "RawData");
                }

                sErr = CreateDirectoryIfNotExists(rawDataFolderPath);
                if (sErr.Length > 0)
                {
                    goto Get_Out;
                }
                foreach (IFormFile file in files)
                {
                    fileName = Path.Combine(rawDataFolderPath, file.FileName);
                    using (FileStream fs = System.IO.File.Create(fileName))
                    {
                        await file.CopyToAsync(fs);
                        fs.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                sErr += "Upload files failed.";
                sErr += "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }
        Get_Out:
            return new Tuple<string, string, string>("", sErr, fileName);
        }

        private static string CreateDirectoryIfNotExists(string dir)
        {
            if (string.IsNullOrEmpty(dir)) throw new ArgumentNullException("dir cannot be empty.");

            string sErr = "";
            try
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
            catch (Exception ex)
            {
                sErr += $"Failed to create directory: {dir}";
                sErr += "\r\n" + ex.Message + "\r\n" + ex.StackTrace;
                goto Get_Out;
            }
        Get_Out:
            return sErr;
        }
    }
}
