using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyUI.Models
{
    public class Tenant
    {
        [Key]
        public int TenantId { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public int? PropertyId { get; set; } //foreign key
        public Property? MyProperty { get; set; }
    }
}