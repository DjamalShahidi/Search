using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmootSearch.Entities
{
    public class Model
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Index { get; set; }

        public int TotalNumber { get; set; }

        public string Info { get; set; }

        [NotMapped]
        public List<SubModel> Subs { get; set; } = new List<SubModel> { };

        public void PrepareSubs()
        {
            Subs = JsonConvert.DeserializeObject<List<SubModel>>(Info);
        }
    }
    public class SubModel
    {
        public long TextId { get; set; }

        public double TF { get; set; }
    }
}
