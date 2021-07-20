using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnlineAPI.Ultilities
{
    public static class GenerateNewImageFileName
    {
        public static string Generate(IFormFile image)
        {
            //Getting FileName
            var fileName = Path.GetFileName(image.FileName);

            //Getting file Extension
            var fileExtension = Path.GetExtension(image.FileName);

            //Concatenating FileName + FileExtension
            var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

            return newFileName;
        }
    }
}
