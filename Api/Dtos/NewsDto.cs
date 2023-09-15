using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Dtos
{
    public class NewsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public IFormFile ImageFile { get; set; }
        public DateTime Created { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ApplicationLinkDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public IFormFile ImageFile { get; set; }
        public DateTime Created { get; set; }
        public bool IsDeleted { get; set; }
    }
}
