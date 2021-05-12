using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Tunrecrute.Models
{
    public class ProfSkill
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public int Percent { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("ProfSkills")]
        public virtual User User { get; set; }
    }
}
