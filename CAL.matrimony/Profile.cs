using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace matrimony.core
{
    [Table("Profile")]
    public class Profile
    {
        [Key]
        public long ProfileId { get; set; }
    }
}
