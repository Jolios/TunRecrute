using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Tunrecrute.Models
{
    public class SocialMedia
    {
        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        [Url]
        public string Facebook { get; set; }
        [StringLength(255)]
        [Url]

        public string Twitter { get; set; }
        [StringLength(255)]
        [Url]

        public string Google { get; set; }
        [StringLength(255)]
        [Url]

        public string Linkedin { get; set; }
        [StringLength(255)]
        [Url]

        public string Pintrest { get; set; }
        [StringLength(255)]
        [Url]
        public string Instagram { get; set; }
        [StringLength(255)]
        [Url]
        public string Behance { get; set; }
        [StringLength(255)]
        [Url]
        public string Dribbble { get; set; }
        [StringLength(255)]
        [Url]
        public string Github { get; set; }
        public string UserId { get; set; }
        [StringLength(255)]
        [Url]
        public string Youtube { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("SocialMedia")]
        public virtual User User { get; set; }
    }
}
