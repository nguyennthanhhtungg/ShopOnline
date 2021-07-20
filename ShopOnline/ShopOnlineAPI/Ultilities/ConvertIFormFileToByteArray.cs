using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnlineAPI.Ultilities
{
    public static class ConvertIFormFileToByteArray
    {
        public static byte[] Convert(IFormFile image)
        {
            using (var target = new MemoryStream())
            {
                image.CopyTo(target);
                return target.ToArray();
            }
        }
    }
}
