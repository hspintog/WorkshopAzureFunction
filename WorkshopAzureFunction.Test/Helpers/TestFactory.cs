using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WorkshopAzureFunction.Common.Models;
using WorkshopAzureFunction.Functions.Entities;

namespace WorkshopAzureFunction.Test.Helpers
{
    public class TestFactory
    {
        public static TimesEmployeesEntity GetTodoEntity()
        {
            return new TimesEmployeesEntity
            {
                DateInputOutput = Convert.ToDateTime("2021-09-06"),
                ETag = "*",
                Consolidated = false,
                Type = 0,
                PartitionKey = "TIMESEMPLOYEES",
                RowKey = Guid.NewGuid().ToString(),
                IdEmployee = 999
            };
        }


        public static ConsolidatedEntity GetConsolidatedEntity()
        {
            return new ConsolidatedEntity
            {
                Date =  Convert.ToDateTime("2021-09-06"),
                ETag = "*",
                MinutesWorked = 150,
                PartitionKey = "TIMESCONSOLIDATEDEMPLOYEES",
                RowKey = Guid.NewGuid().ToString(),
                IdEmployee = 999
            };
        }


        public static DefaultHttpRequest CreateHttpRequest(Guid timeEmployeedId, TimesEmployees todoRequest)
        {
            string request = JsonConvert.SerializeObject(todoRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request),
                Path = $"/{timeEmployeedId}"
            };
        }



        public static DefaultHttpRequest CreateHttpRequest(Guid timeEmployeedId)
        {

            return new DefaultHttpRequest(new DefaultHttpContext())
            {

                Path = $"/{timeEmployeedId}"
            };
        }


        public static DefaultHttpRequest CreateHttpRequest(TimesEmployees todoRequest)
        {
            string request = JsonConvert.SerializeObject(todoRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request)
            };
        }


        public static DefaultHttpRequest CreateHttpRequest()
        {

            return new DefaultHttpRequest(new DefaultHttpContext());

        }


        public static TimesEmployees GetTodoRequest()
        {
            return new TimesEmployees
            {
                DateInputOutput = DateTime.UtcNow,
                Type = 0,
                IdEmployee = 999,
            };
        }


        public static Stream GenerateStreamFromString(string stringToConvert)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(stringToConvert);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }



        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;
            if (type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }
            return logger;
        }

    }
}
