using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OfflineTest.Models
{
    public class Book
    {
        [Key]
        public int BookId { set; get; }
        public string BookName { set; get; }
        public string Desc { set; get; }
    }
}
