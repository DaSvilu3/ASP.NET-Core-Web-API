using System;
using Microsoft.AspNetCore.Http;

namespace RestAPIDemo.Models
{
    public class FileUpload
    {
        public IFormFile files { get; set; }
    }
}
