using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NakedBank.Infrastructure.Entities
{
    public class Token
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TokenId { get; set; }

        [Required]
        public string TokenString { get; set; }

        [Required]
        public DateTime IssuedAt { get; set; }

        [Required]
        public DateTime ValidSince { get; set; }

        [Required]
        public DateTime ValidUntil { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
