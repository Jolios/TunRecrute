using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Tunrecrute.Models
{
    public class Language
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        [StringLength(255)]
        public string LanguageName { get; set; }
        [Required]
        [StringLength(255)]
        public string ProfeciencyLevel { get; set; }
        public int? Percent { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("Languages")]
        public virtual User User { get; set; }
    }
}
