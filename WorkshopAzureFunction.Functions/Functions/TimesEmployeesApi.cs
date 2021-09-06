using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using WorkshopAzureFunction.Common.Models;
using WorkshopAzureFunction.Common.Responses;
using WorkshopAzureFunction.Functions.Entities;

namespace WorkshopAzureFunction.Functions.Functions
{
    public static class TimesEmployeesApi
    {

        [FunctionName(nameof(CreateInput))]
        public static async Task<IActionResult> CreateInput(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "TimesEmployees")] HttpRequest req,
            [Table("TimesEmployees", Connection = "AzureWebJobsStorage")] CloudTable timeTable,
            ILogger log)
        {
            log.LogInformation("Recieved a new register");


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            TimesEmployees timeEmployee = JsonConvert.DeserializeObject<TimesEmployees>(requestBody);

            if (timeEmployee?.IdEmployee == 0)
            {
                return new BadRequestObjectResult(new Response { IsSuccess = false, Message = "the request must have  a IdEmployee different of 0." });
            }

            
             TimesEmployeesEntity timeEmployeEntity = new TimesEmployeesEntity
            {
                DateInputOutput = timeEmployee.DateInputOutput,
                ETag = "*",
                Consolidated = false,
                Type = timeEmployee.Type,
                PartitionKey = "TIMESEMPLOYEES",
                RowKey = Guid.NewGuid().ToString(),
                IdEmployee = timeEmployee.IdEmployee
            };


            TableOperation addOperation = TableOperation.Insert(timeEmployeEntity);
            await timeTable.ExecuteAsync(addOperation);

            string message = "New Register stored in table";
            log.LogInformation(message);


            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = timeEmployeEntity
            });
        }




        [FunctionName(nameof(UpdateInput))]
        public static async Task<IActionResult> UpdateInput(
           [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "TimesEmployees/{id}")] HttpRequest req,
           [Table("TimesEmployees", Connection = "AzureWebJobsStorage")] CloudTable timeTable,
           string id,
           ILogger log)
        {
            log.LogInformation($"Update for register: {id}, received.");


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            TimesEmployees timeEmployee = JsonConvert.DeserializeObject<TimesEmployees>(requestBody);

            //Validate input id
            TableOperation findOperation = TableOperation.Retrieve<TimesEmployeesEntity>("TIMESEMPLOYEES", id);
            TableResult findResult = await timeTable.ExecuteAsync(findOperation);

            if (findResult.Result == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "Todo not found."
                });
            }


            //Update input
            TimesEmployeesEntity timeEmployeeEntity = (TimesEmployeesEntity)findResult.Result;
            
            if (!string.IsNullOrEmpty(timeEmployee.IdEmployee.ToString()))
            {
                timeEmployeeEntity.IdEmployee = timeEmployee.IdEmployee;
                timeEmployeeEntity.Type = timeEmployee.Type;
                timeEmployeeEntity.DateInputOutput = timeEmployee.DateInputOutput;
            }


            TableOperation addOperation = TableOperation.Replace(timeEmployeeEntity);
            await timeTable.ExecuteAsync(addOperation);

            string message = $"Register: {id}, update in table.";
            log.LogInformation(message);


            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = timeEmployeeEntity
            });
        }






        [FunctionName(nameof(GetAllRegisters))]
        public static async Task<IActionResult> GetAllRegisters(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "TimesEmployees")] HttpRequest req,
        [Table("TimesEmployees", Connection = "AzureWebJobsStorage")] CloudTable timeTable,
        ILogger log)
        {
            log.LogInformation("Get all registers received.");

            TableQuery<TimesEmployeesEntity> query = new TableQuery<TimesEmployeesEntity>();
            TableQuerySegment<TimesEmployeesEntity> todos = await timeTable.ExecuteQuerySegmentedAsync(query, null);

            string message = "Retrieve all registers.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = todos
            });
        }



        [FunctionName(nameof(GetRegisterById))]
        public static IActionResult GetRegisterById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "TimesEmployees/{id}")] HttpRequest req,
        [Table("TimesEmployees", "TIMESEMPLOYEES", "{id}")] TimesEmployeesEntity timeEmployeeEntity,
        ILogger log)
        {
            if (timeEmployeeEntity == null)
            {
                return new NotFoundObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "Register not found."
                });
            }

            log.LogInformation($"Get for register: {timeEmployeeEntity.RowKey}, received.");

            // Send response
            string message = $"Register: {timeEmployeeEntity.RowKey}, retrieved.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = timeEmployeeEntity
            });
        }



        [FunctionName(nameof(DeleteRegister))]
        public static async Task<IActionResult> DeleteRegister(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "TimesEmployees/{id}")] HttpRequest req,
            [Table("TimesEmployees", "TIMESEMPLOYEES", "{id}")] TimesEmployeesEntity timeEmployeeEntity,
            [Table("TimesEmployees", Connection = "AzureWebJobsStorage")] CloudTable timeTable,
            ILogger log)
        {
            if (timeEmployeeEntity == null)
            {
                return new NotFoundObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "Register not found."
                });
            }

            log.LogInformation($"Delete for register: {timeEmployeeEntity.RowKey}, received.");
            await timeTable.ExecuteAsync(TableOperation.Delete(timeEmployeeEntity));

            // Send response
            string message = $"Register: {timeEmployeeEntity.RowKey}, deleted.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = timeEmployeeEntity
            });
        }



    }
}
