using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCAssingment1.Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public float Runtime { get; set; }
        public string Synopsis { get; set; }
        public int Rating { get; set; }
    }
}
