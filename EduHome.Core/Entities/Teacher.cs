﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class Teacher:BaseModel
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Mail { get; set; }
        [Required]  
        public int? ExperienceYear { get; set; }
        [Required]
        public string? Skype { get; set; }
        [Required]
        public string? Faculty { get; set; }
        [Required]
        public int? PositionId { get; set; }
        [Required]
        public Position? Position { get; set; }
        [Required]
        public int DegreeId { get; set; }   
        [Required]
        public Degree? Degree { get; set; }

    }
}