using System;
using System.IO;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.IO.Compression;
using Newtonsoft.Json;
using FunctionApp6.Services;

namespace FunctionApp6
{
    public class Function
    {
        private readonly ILogger _logger;

        private IAPIServices _services;

        public Function(IAPIServices services, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function>();
            _services = services;
        }

        //[Function("BulkInsert")]
        //public void Run([BlobTrigger("multifilecontainer/{name}", Connection = "AzureWebJobsStorage")] byte[] myBlob, string name)
        //{
        //    _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {myBlob}");
        //    try
        //    {
        //        _logger.LogInformation($"Processing blob '{name}'...");
        //        using (MemoryStream memoryStream = new MemoryStream(myBlob))
        //        {
        //            // Process the blob contents (memoryStream) here
        //            // For example, you can read the contents using a StreamReader
        //            using (StreamReader reader = new StreamReader(memoryStream))
        //            {
        //                string jsonData = reader.ReadToEnd();

        //                List<Model1> items = JsonConvert.DeserializeObject<List<Model1>>(jsonData);

        //                foreach (Model1 item in items)
        //                {
        //                    // Perform bulk insertion by calling _services.CreateItem(item)
        //                    _services.CreateItem(item);
        //                }

        //                _logger.LogInformation("Bulk insertion completed successfully.");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error processing blob '{name}': {ex.Message}");
        //    }
        //}
    }
}
