﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace MoneyGo.Helpers
{
    internal class UploadService
    {
        PathProvider pathProvider;

        public UploadService(PathProvider pathProvider, IConfiguration configuration)
        {
            this.pathProvider = pathProvider;
        }

        public async Task<String> UploadFileAsync(IFormFile fichero, Folders folder)
        {
            //Task => void. Task<??>
            String filename = fichero.FileName;
            String path = this.pathProvider.MapPath(filename, Folders.Images);
            using (var Stream = new FileStream(path, FileMode.Create))
            {
                await fichero.CopyToAsync(Stream);
            }

            return path;
        }
    }
}