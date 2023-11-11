using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmootSearch.Entities
{
    public class Text
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Content { get; set; }

        [NotMapped]
        public double Score { get; set; }
    }
}
