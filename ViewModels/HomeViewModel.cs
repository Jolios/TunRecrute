using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tunrecrute.Models;

namespace Tunrecrute.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Advertisement> Ads { get; set; }

        public AdvertisementsIndexViewModel Model { get; set; }
    }
}
