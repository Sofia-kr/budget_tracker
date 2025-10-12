using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budget_tracker
{
    public enum ViewType
    {
        Expenses,
        Income,
        Savings
    }
    public enum PeriodType
    {
        Day = 0,
        Week = 1,
        Month = 2,
        Year = 3,
        Custom = 4
    }

}
