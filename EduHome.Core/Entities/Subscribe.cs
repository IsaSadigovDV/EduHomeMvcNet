using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class Subscribe:BaseModel
    {
        [Required]
        [EmailAddress(ErrorMessage ="Invalid email address")]
        public string Email { get; set; }
    }
}
