using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class BlogTag:BaseModel
    {
        public int BlogId { get; set; }
        public Blog? Blog { get; set; }
        public int Tagid { get; set; }
        public Tag? Tag { get; set; }
    }
}
