using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tunrecrute.Models;

namespace Tunrecrute.ViewModels
{
    public class LanguageViewModel
    {
        public Language language { get; set; }
        public List<SelectListItem> list { get; set; }
    }
}
