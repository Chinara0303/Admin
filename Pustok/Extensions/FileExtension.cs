using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.Extensions
{
    public static class FileExtension
    {
       
        public async static Task<string> FileCreateAsync(this IFormFile file, IWebHostEnvironment _env, params string[] folders)
        {
            string FileName = Guid.NewGuid().ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + file.FileName;
            //string pathHover = Path.Combine(_env.WebRootPath, "image", "products", FileName);
            string path = _env.WebRootPath;
            foreach (string item in folders)
            {
                path = Path.Combine(path, item);
            }

            path = Path.Combine(path, FileName);


            using (FileStream filestream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(filestream);
            }

            return FileName;
        }
    }
}
