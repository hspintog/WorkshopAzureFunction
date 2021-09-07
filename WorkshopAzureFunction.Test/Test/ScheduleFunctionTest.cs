using System;
using System.Collections.Generic;
using System.Text;
using WorkshopAzureFunction.Functions.Functions;
using WorkshopAzureFunction.Test.Helpers;
using Xunit;
using Microsoft.Extensions.Logging;

namespace WorkshopAzureFunction.Test.Test
{
    public class ScheduleFunctionTest
    {
        [Fact]
        public void ScheduleFunction_Should_Log_Message()
        {
            //arrenge
            MockCloudTableTimesEmployees mockTableEmployees = new MockCloudTableTimesEmployees(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            MockCloudTableConsolidatedEmployees mockConsolidatedTableEmployees = new MockCloudTableConsolidatedEmployees(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            ListLogger logger = (ListLogger)TestFactory.CreateLogger(LoggerTypes.List);
            //act
            ScheduledFunction.Run(null, mockTableEmployees, mockConsolidatedTableEmployees, logger);
            string message = logger.Logs[0];
            //assert
            Assert.Contains("Registers", message);
        }
    }
}
