using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public int AccountBalance { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
