using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class Teacher : BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Mail { get; set; }
        [Required]
        public int ExperienceYear { get; set; }
        [Required]
        public string AboutMe { get; set; }
        [Required]
        public string Skype { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Faculty { get; set; }
        public string? Image { get; set; }
        [Required]
        public int PositionId { get; set; }
        public Position? Position { get; set; }
        [Required]
        public int DegreeId { get; set; }   
        public Degree? Degree { get; set; }
        public List<Skill>? Skills { get; set; }
        public List<Social>? Socials { get; set; }
        public List<TeacherHobby>? TeacherHobbies { get; set; }
        [NotMapped]
        public IFormFile? FormFile { get; set; }

		[NotMapped]
		public List<int>? HobbyIds { get; set; }

	}
}
