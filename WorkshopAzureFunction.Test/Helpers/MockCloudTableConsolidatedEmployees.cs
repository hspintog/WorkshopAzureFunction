using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopAzureFunction.Test.Helpers
{
    public class MockCloudTableConsolidatedEmployees: CloudTable
    {
        public MockCloudTableConsolidatedEmployees(Uri tableAddress) : base(tableAddress)
        {
        }

        public MockCloudTableConsolidatedEmployees(Uri tableAbsoluteUri, StorageCredentials credentials) : base(tableAbsoluteUri, credentials)
        {
        }

        public MockCloudTableConsolidatedEmployees(StorageUri tableAddress, StorageCredentials credentials) : base(tableAddress, credentials)
        {
        }

        public override async Task<TableResult> ExecuteAsync(TableOperation operation)
        {
            return await Task.FromResult(new TableResult
            {
                HttpStatusCode = 200,
                Result = TestFactory.GetConsolidatedEntity()
            });
        }

        public override async Task<TableQuerySegment<ConsolidatedEntity>> ExecuteQuerySegmentedAsync<ConsolidatedEntity>(TableQuery<ConsolidatedEntity> query, TableContinuationToken token)
        {
            ConstructorInfo constructor = typeof(TableQuerySegment<ConsolidatedEntity>)
                   .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0];
            return await Task.FromResult(constructor.Invoke(new object[] { new List<ConsolidatedEntity>() }) as TableQuerySegment<ConsolidatedEntity>);
        }


    }
}
