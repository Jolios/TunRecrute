using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Tunrecrute.Models
{
    public class Education
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        [StringLength(255)]
        public string DiplomaTitle { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        [StringLength(255)]
        public string InstitutionName { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public string Description { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("Educations")]
        public virtual User User { get; set; }
    }
}
