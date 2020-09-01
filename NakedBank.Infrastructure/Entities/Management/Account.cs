using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NakedBank.Infrastructure.Entities
{
    public class Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }

        [Required]
        public int BranchId { get; set; }

        [Required]
        public string AccountNumber { get; set; }

        [Required]
        public decimal Balance { get; set; } = 0;

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
