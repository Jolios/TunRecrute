using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Tunrecrute.Models
{
    public class WorkExperience
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        [Required]
        [StringLength(255)]
        public string CompanyName { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public string Description { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("WorkExperiences")]
        public virtual User User { get; set; }
    }
}
