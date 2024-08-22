using System.ComponentModel.DataAnnotations;

namespace responseexample.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string ?Name { get; set; }
    }
}
