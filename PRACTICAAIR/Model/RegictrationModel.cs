using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRACTICAAIR.Model
{
    public class RegictrationModel
    {
        [Key]
        public int IdAuthorization {  get; set; }
        public string Login {  get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
