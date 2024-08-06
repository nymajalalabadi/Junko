using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.Entities.Common
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }

        public bool IsDelete { get; set; } = false;

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public DateTime LastUpdateDate { get; set; } = DateTime.Now;
    }
}
