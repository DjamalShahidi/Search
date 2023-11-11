using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AmootSearch.Entities
{
    public class Synonym
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Main { get; set; }

        public string Equivalent { get; set; }

    }
}
