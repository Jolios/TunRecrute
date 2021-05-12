using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Tunrecrute.ViewModels
{
    public class AboutUserViewModel
    {
        [Required(ErrorMessage ="This field is required")]
        public string AboutMe { get; set; }

    }
}
