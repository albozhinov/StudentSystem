using System;
using System.ComponentModel.DataAnnotations;

namespace StudentSystem.Data
{
    public class Resource
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public RecourceType Type { get; set; }

        [Required]
        public string URL { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }
    }
}
