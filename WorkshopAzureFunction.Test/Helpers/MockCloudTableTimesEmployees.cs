using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

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
    }
}
