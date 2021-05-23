using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Tunrecrute.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            Advertisements = new HashSet<Advertisement>();
            Candidacies = new HashSet<Candidacy>();
            Educations = new HashSet<Education>();
            Languages = new HashSet<Language>();
            ProfSkills = new HashSet<ProfSkill>();
            WorkExperiences = new HashSet<WorkExperience>();
        }

        [StringLength(255)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string Firstname { get; set; }
        [StringLength(255)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string Lastname { get; set; }
        [StringLength(255)]
        public string Address { get; set; }
        [StringLength(255)]
        public string CompanyName { get; set; }
        [StringLength(255)]
        public string Picture { get; set; }
        public string AboutMe { get; set; }
        [StringLength(10)]
        public string Sex { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Birthdate { get; set; }
        [StringLength(255)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string Nationality { get; set; }
        [StringLength(255)]
        public string CVFilename { get; set; }
        [StringLength(255)]
        public string CoverLetterFilename { get; set; }
        public short IsActive { get; set; }
        
        [InverseProperty(nameof(Advertisement.User))]
        public virtual ICollection<Advertisement> Advertisements { get; set; }
        [InverseProperty(nameof(Candidacy.User))]
        public virtual ICollection<Candidacy> Candidacies { get; set; }
        [InverseProperty("User")]
        public virtual SocialMedia SocialMedia { get; set; }
        [InverseProperty(nameof(Education.User))]
        public virtual ICollection<Education> Educations { get; set; }
        [InverseProperty(nameof(Language.User))]
        public virtual ICollection<Language> Languages { get; set; }
        [InverseProperty(nameof(ProfSkill.User))]
        public virtual ICollection<ProfSkill> ProfSkills { get; set; }
        [InverseProperty(nameof(WorkExperience.User))]
        public virtual ICollection<WorkExperience> WorkExperiences { get; set; }
    }
}
