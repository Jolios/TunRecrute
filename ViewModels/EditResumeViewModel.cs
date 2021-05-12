using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tunrecrute.Models;

namespace Tunrecrute.ViewModels
{
    public class EditResumeViewModel
    {

        public User Candidate { get; set; }
        public IFormFile CVFile { get; set; }

        public IFormFile CoverLetterFile { get; set; }
    }
}
