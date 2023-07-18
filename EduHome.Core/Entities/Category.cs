using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class Category:BaseModel
    {
        public string Name { get; set; }
        public List<CourseCategory>? courseCategories { get; set; }
        public List<BlogCategory>? blogCategories { get; set;}

    }
}
