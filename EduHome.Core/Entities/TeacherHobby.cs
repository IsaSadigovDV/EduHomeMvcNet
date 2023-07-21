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
        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public int? HobbyId { get;set; }
        public Hobby? Hobby { get; set; }

    }
}
