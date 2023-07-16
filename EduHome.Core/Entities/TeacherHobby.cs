using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class TeacherHobby:BaseModel
    {
        [Required]
        public int TeacherId { get; set; }
        [Required]
        public Teacher? Teacher { get; set; }
        [Required]
        public int? HobbyId { get;set; }
        [Required]
        public Hobby? Hobby { get; set; }

    }
}
