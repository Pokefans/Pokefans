using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Data
{
    [Table("content_boilerplates")]
    class ContentBoilerplate
    {
        [Key]
        public int Id { get; set; }

        public int ContentId { get; set; }
        public int BoilerplateId { get; set; }
        public string ContentBoilerplateName { get; set; }
    }
}
