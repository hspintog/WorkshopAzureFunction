using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using WorkshopAzureFunction.Common.Models;
using WorkshopAzureFunction.Functions.Entities;
using WorkshopAzureFunction.Functions.Functions;
using WorkshopAzureFunction.Test.Helpers;
using Xunit;

namespace WorkshopAzureFunction.Test.Test
{
    public class TimesEmployeesApiTest
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void CreateInputOutput_Should_Return_200()
        {
            //Arrenge
            MockCloudTableTimesEmployees mockTimesEmployees = new MockCloudTableTimesEmployees(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            TimesEmployees timesEmployeesRequest = TestFactory.GetTodoRequest();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(timesEmployeesRequest);

            //Act
            IActionResult response = await TimesEmployeesApi.CreateInput(request, mockTimesEmployees, logger);

            //Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

        }


        [Fact]
        public async void UpdateInputOutput_Should_Return_200()
        {
            //Arrenge
            MockCloudTableTimesEmployees mockTimesEmployees = new MockCloudTableTimesEmployees(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            TimesEmployees timesEmployeesRequest = TestFactory.GetTodoRequest();
            Guid timeEmployeedId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(timeEmployeedId, timesEmployeesRequest);

            //Act
            IActionResult response = await TimesEmployeesApi.UpdateInput(request, mockTimesEmployees, timeEmployeedId.ToString(), logger);

            //Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

        }




        [Fact]
        public async void GetAllRegisters_Should_Return_200()
        {
            //Arrenge
            MockCloudTableTimesEmployees mockTimesEmployees = new MockCloudTableTimesEmployees(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            DefaultHttpRequest request = TestFactory.CreateHttpRequest();

            //Act
            IActionResult response = await TimesEmployeesApi.GetAllRegisters(request, mockTimesEmployees, logger);

            //Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

        }

         [Fact]
         public async void DeleteTodo_Should_Return_200()
         {
             //Arrenge
             MockCloudTableTimesEmployees mockTimesEmployees = new MockCloudTableTimesEmployees(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
             TimesEmployeesEntity timeEmployeeEntity = TestFactory.GetTodoEntity();
             Guid timeEmployeedId = Guid.NewGuid();
             DefaultHttpRequest request = TestFactory.CreateHttpRequest(timeEmployeedId);

             //Act
             IActionResult response = await TimesEmployeesApi.DeleteRegister(request, timeEmployeeEntity, mockTimesEmployees, logger);

             //Assert
             OkObjectResult result = (OkObjectResult)response;
             Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

         }


        [Fact]
        public async void GetConsolidatedDate_Should_Return_200()
        {
            //Arrenge
            MockCloudTableConsolidatedEmployees mockTimesEmployees = new MockCloudTableConsolidatedEmployees(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            string date = "2021-09-06";
            DefaultHttpRequest request = TestFactory.CreateHttpRequest();

            //Act
            IActionResult response = await TimesEmployeesApi.GetConsolidatedDate(request, mockTimesEmployees, date, logger);

            //Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

        }


        [Fact]
        public async void GetRegisterById_Should_Return_404()
        {
            //Arrenge
            MockCloudTableTimesEmployees mockTimesEmployees = new MockCloudTableTimesEmployees(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            string date = "2021-09-06";
            DefaultHttpRequest request = TestFactory.CreateHttpRequest();

            //Act
            IActionResult response = await TimesEmployeesApi.GetRegisterById(request, mockTimesEmployees, date, logger);

            //Assert
            NotFoundObjectResult result = (NotFoundObjectResult)response;
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);

        }


    }

}

