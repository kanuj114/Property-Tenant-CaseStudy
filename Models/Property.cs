using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PropertyUI.Models
{
    public class Property
    {
         [Key]
        public int PropertyId { get; set; } //Unique identifier for the property.
        public string Address { get; set; } //Address of the property.
        public int RentalPrice { get; set; } //Monthly rental price for the property.        
        public DateOnly AvailableFrom { get; set; } //Date when the property becomes available for rent.
        [JsonIgnore] //to avoid the cyclic reference
        public ICollection<Tenant>? Tenants { get; set; } //Collection of tenants associated with the property.
    }
    }
