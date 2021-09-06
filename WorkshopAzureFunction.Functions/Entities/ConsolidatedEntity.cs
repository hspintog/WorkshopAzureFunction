using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkshopAzureFunction.Functions.Entities
{
    public class ConsolidatedEntity : TableEntity
    {
        public int IdEmployee { get; set; }
        public DateTime Date { get; set; }

        public int MinutesWorked { get; set; }
    }
}
