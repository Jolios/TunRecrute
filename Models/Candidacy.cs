using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Tunrecrute.Models
{
    public class Candidacy
    {
        [Key]
        public string UserId { get; set; }
        [Key]
        public int AdvertisementId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public short IsDeleted { get; set; }

        [ForeignKey(nameof(AdvertisementId))]
        [InverseProperty("Candidacies")]
        public virtual Advertisement Advertisement { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Candidacies")]
        public virtual User User { get; set; }
    }
}
