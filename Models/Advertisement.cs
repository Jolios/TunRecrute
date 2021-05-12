using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Tunrecrute.Models
{
    public class Advertisement
    {
        public Advertisement()
        {
            Candidacies = new HashSet<Candidacy>();
        }
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public DateTime? Posted { get; set; }
        [StringLength(255)]
        public string Salary { get; set; }
        [StringLength(255)]
        public string Contract { get; set; }
        [StringLength(255)]
        public string WorkAddress { get; set; }
        [StringLength(255)]
        public string Experiance { get; set; }
        [StringLength(255)]
        public string WorkHours { get; set; }
        [StringLength(255)]
        public string Diploma { get; set; }
        [StringLength(255)]
        public string Field { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("Advertisements")]
        public virtual User User { get; set; }
        [InverseProperty(nameof(Candidacy.Advertisement))]
        public virtual ICollection<Candidacy> Candidacies { get; set; }
    }
}
