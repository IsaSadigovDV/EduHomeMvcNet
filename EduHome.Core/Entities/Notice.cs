using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class Notice:BaseModel
    {
        [Required(ErrorMessage ="Description can not be empty")]
        public string Description { get; set; }
        [Required]
        public DateTime dateTime { get; set; }
    }
}
