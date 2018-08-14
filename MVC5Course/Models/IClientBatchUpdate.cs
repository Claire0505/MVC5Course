using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC5Course.Models
{
    public interface IClientBatchUpdate
    {
        int ClientId { get; set; }
        [Required]
        string FirstName { get; set; }
        [Required]
        string LastName { get; set; }
        [Required]
        string MiddleName { get; set; }
    }
}
