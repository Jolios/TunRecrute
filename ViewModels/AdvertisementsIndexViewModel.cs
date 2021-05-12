using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tunrecrute.Models;

namespace Tunrecrute.ViewModels
{
    public class AdvertisementsIndexViewModel
    {
        public AdvertisementsIndexViewModel()
        {
             Fields = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "Any" },
            new SelectListItem { Value = "Banking and Finance", Text = "Banking & Finance" },
            new SelectListItem { Value = "Education", Text = "Education"  },
            new SelectListItem { Value = "Consulting", Text = "Consulting"  },
            new SelectListItem { Value = "Marketing and Advertising", Text = "Marketing & Advertising" },
            new SelectListItem { Value = "Technology", Text = "Technology"  },

        };
    }
        public PagedResult<Advertisement> Ads { get; set; }
        public string Title { get; set; }
        public string Field { get; set; }
        public string WorkAddress { get; set; }
        public string Diploma { get; set; }
        public string Contract { get; set; }
        public string Experience { get; set; }
        public bool Etat { get; set; }
        public bool Searched { get; set; }
        public List<SelectListItem> Fields { get; set; }

    }
}
