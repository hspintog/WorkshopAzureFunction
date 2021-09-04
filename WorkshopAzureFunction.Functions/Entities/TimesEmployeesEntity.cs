using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace WorkshopAzureFunction.Functions.Entities
{
    public class TimesEmployeesEntity : TableEntity
    {
        public int IdEmployee { get; set; }
        public DateTime DateInputOutput { get; set; }

        public int Type { get; set; }

        public bool Consolidated { get; set; }
    }
}
