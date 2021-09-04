using System;
using System.Collections.Generic;
using System.Text;

namespace WorkshopAzureFunction.Common.Models
{
    public class TimesEmployees
    {
        public int IdEmployee { get; set; }
        public DateTime DateInputOutput { get; set; }

        public int Type { get; set; }

        public bool Consolidated { get; set; }
    }
}
