using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;
using System.Reflection;

namespace WorkshopAzureFunction.Test.Helpers
{
    public class MockCloudTableTimesEmployees : CloudTable
    {
        public MockCloudTableTimesEmployees(Uri tableAddress) : base(tableAddress)
        {
        }

        public MockCloudTableTimesEmployees(Uri tableAbsoluteUri, StorageCredentials credentials) : base(tableAbsoluteUri, credentials)
        {
        }

        public MockCloudTableTimesEmployees(StorageUri tableAddress, StorageCredentials credentials) : base(tableAddress, credentials)
        {
        }

        public override async Task<TableResult> ExecuteAsync(TableOperation operation)
        {
            return await Task.FromResult(new TableResult
            {
                HttpStatusCode = 200,
                Result = TestFactory.GetTodoEntity()
            });
        }



        public override async Task<TableQuerySegment<TimesEmployeesEntity>> ExecuteQuerySegmentedAsync<TimesEmployeesEntity>(TableQuery<TimesEmployeesEntity> query, TableContinuationToken token)
        {
            ConstructorInfo constructor = typeof(TableQuerySegment<TimesEmployeesEntity>)
                   .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0];
            return await Task.FromResult(constructor.Invoke(new object[] { new List<TimesEmployeesEntity>() }) as TableQuerySegment<TimesEmployeesEntity>);
        }
    }
}
