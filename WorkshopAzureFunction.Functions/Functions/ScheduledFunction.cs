using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;
using WorkshopAzureFunction.Functions.Entities;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using WorkshopAzureFunction.Common.Models;
using System.Linq;
using WorkshopAzureFunction.Common.Responses;

namespace WorkshopAzureFunction.Functions.Functions
{
    public static class ScheduledFunction
    {
        
        [FunctionName("ScheduledFunction")]
        public static async Task Run([TimerTrigger("0 */2 * * * *")] TimerInfo myTimer,
            [Table("TimesEmployees", Connection = "AzureWebJobsStorage")] CloudTable timeTable,
            [Table("ConsolidatedEmployes", Connection = "AzureWebJobsStorage")] CloudTable consolidatedTable,
            ILogger log)
        {
            log.LogInformation($"Deleting completed function executed at: {DateTime.Now}");
            string filter = TableQuery.GenerateFilterConditionForBool("Consolidated", QueryComparisons.Equal, false);

            TableQuery<TimesEmployeesEntity> query = new TableQuery<TimesEmployeesEntity>().Where(filter);
            TableQuerySegment<TimesEmployeesEntity> getAllEmployees = await timeTable.ExecuteQuerySegmentedAsync(query, null);

            

            List<TimesEmployeesEntity> collectionEmploye1 = getAllEmployees.Results.Where(x => x.Type == 0).ToList();
            List<TimesEmployeesEntity> collectionEmploye2 = getAllEmployees.Results.Where(x => x.Type == 1).ToList();

            int update = 0;
            for (int i = 0; i < collectionEmploye1.Count; i++)
            {
                for (int j = 0; j < collectionEmploye2.Count; j++)
                {
                    if (collectionEmploye1[i].IdEmployee == collectionEmploye2[j].IdEmployee)
                    {
                        ConsolidatedEntity consolidated = new ConsolidatedEntity
                        {
                            MinutesWorked = CalculateMinutesWorked(collectionEmploye1[i].DateInputOutput, collectionEmploye2[j].DateInputOutput),
                            ETag = "*",                            
                            PartitionKey = "TIMESCONSOLIDATEDEMPLOYEES",
                            RowKey = Guid.NewGuid().ToString(),
                            IdEmployee = collectionEmploye1[i].IdEmployee,
                            Date = Convert.ToDateTime(collectionEmploye2[j].DateInputOutput.ToString("yyyy-MM-dd"))
                        };


                        TableOperation addOperation = TableOperation.Insert(consolidated);
                        await consolidatedTable.ExecuteAsync(addOperation);

                        string message = "New Register stored in table ConsolidatedEntity";
                        log.LogInformation(message);


                        TableOperation findOperation = TableOperation.Retrieve<TimesEmployeesEntity>("TIMESEMPLOYEES", collectionEmploye2[j].RowKey);

                        TableResult findResult = await timeTable.ExecuteAsync(findOperation);

                        TableOperation findOperation1 = TableOperation.Retrieve<TimesEmployeesEntity>("TIMESEMPLOYEES", collectionEmploye1[j].RowKey);
                        TableResult findResult1 = await timeTable.ExecuteAsync(findOperation1);

                        if (findResult.Result == null)
                        {
                            string message2 = "Register stored in table TimesEmployees not found";
                            log.LogInformation(message2);
                        }


                        //Update input
                        TimesEmployeesEntity timeEmployeeEntity = (TimesEmployeesEntity)findResult.Result;
                        timeEmployeeEntity.Consolidated = true;

                        TimesEmployeesEntity timeEmployeeEntity1 = (TimesEmployeesEntity)findResult1.Result;
                        timeEmployeeEntity1.Consolidated = true;



                        TableOperation addOperation3 = TableOperation.Replace(timeEmployeeEntity);
                        await timeTable.ExecuteAsync(addOperation3);

                        TableOperation addOperation4 = TableOperation.Replace(timeEmployeeEntity1);
                        await timeTable.ExecuteAsync(addOperation4);

                        string message3 = $"Register: {collectionEmploye2[j].RowKey}, update in table.";
                        log.LogInformation(message);
                        update++;
                    }
                }
            }
            log.LogInformation($"Registers {update} consolidated at {DateTime.Now}");

        }


        private static int CalculateMinutesWorked(DateTime DateInput, DateTime DateOutput)
        {

            DateTime DateHourStart = DateTime.Parse(DateInput.ToString());
            DateTime DateHourFinish = DateTime.Parse(DateOutput.ToString());

            TimeSpan difference = DateHourFinish - DateHourStart;
            int differenceMinutes = Convert.ToInt32(difference.TotalMinutes);
            return differenceMinutes;
        }
    }
}
