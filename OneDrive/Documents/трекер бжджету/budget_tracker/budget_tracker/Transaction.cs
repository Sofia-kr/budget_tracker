using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budget_tracker
{
    using System;

   
        public class Transaction
        {
            public int Id { get; set; }
            public decimal Amount { get; set; }
            public DateTime Date { get; set; }
            public string Type { get; set; } 
            public string Description { get; set; }
            public string Category { get; set; }

            public Transaction()
            {
                Date = DateTime.Now;
            }
        
    }
}
