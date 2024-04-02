using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NakedBank.Infrastructure.Entities
{
    public class Transaction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TransactionId { get; set; }

        [Required]
        public int TransactionType { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public decimal BalanceAfterTransaction { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        public string BarCode { get; set; }

        [Required]
        public int AccountId { get; set; }

        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
    }
}
