using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FunctionApp6.Models
{
    public class Model1
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string DishName { get; set; }
        [Range(0, 2000)]
        public int Price { get; set; }
        [Range(0, 999.99)]
        public double Weight { get; set; }


    }
}
