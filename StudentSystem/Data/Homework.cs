using System;
using System.ComponentModel.DataAnnotations;

namespace StudentSystem.Data
{
    public class Homework
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public ContentType Type { get; set; }

        public DateTime Submissiondate { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public int StudentId { get; set; }

        public Student Student { get; set; }

    }
}
