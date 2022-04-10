using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;


namespace Pustok.Helpers
{
    public static class Helper
    {
        public static void DeleteFile(IWebHostEnvironment _env,string filename, params string[] folders)
        {
            string path = _env.WebRootPath;
            foreach (string item in folders)
            {
                path = Path.Combine(path, item);
            }

            path = Path.Combine(path, filename);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

        }
    }
}
