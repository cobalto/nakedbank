using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace NakedBank.Shared.Models.Requests
{
    public class TransactionRequest
    {
        [Required]
        public TransactionType TransactionType { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [JsonIgnore]
        [StringLength(44, MinimumLength = 44)]
        public string Barcode { get; set; }
    }
}
