using System.Net;
using FunctionApp6.Services;
using FunctionApp6.TESTINGF;
using Azure.Storage.Blobs;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System;
using System.IO;
using System.IO.Compression;
using FunctionApp6.Models;
using HttpTriggerAttribute = Microsoft.Azure.Functions.Worker.HttpTriggerAttribute;

namespace FunctionApp6
{
    public class Function1
    {
        private readonly ILogger _logger;

        private IAPIServices _services;


        public Function1(IAPIServices services,ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
            _services = services;
        }
        [Function("takeBlobInput")]
        public async Task<Exception> takeBlobInput([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            List<Model1> item = JsonConvert.DeserializeObject<List<Model1>>(requestBody);

            string connectionString = "DefaultEndpointsProtocol=https;AccountName=multifile624;AccountKey=Hp49Z4eIKNSSEi7yhjGj/3Onl+K0OHBb2tLhBUW9m8ZUDDIKsowqHJLVN3tjrnSPD0EZ4ZBrG9Pq+AStovIiIw==;EndpointSuffix=core.windows.net";
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            try{
                Model1 insertItem = new Model1 { DishName = "Checking3", Price = 10, Weight = 10 };
                _services.CreateItem(insertItem);
            }
            catch (Exception err){
                return err;
            }
           

            // Get a reference to the container
            string containerName = "multifilecontainer";
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Generate a unique blob name
            string blobName = Guid.NewGuid().ToString("N") + ".json";

            // Convert the item object back to JSON
            string jsonContent = JsonConvert.SerializeObject(item);

            // Upload the JSON content to the blob
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonContent)))
            {
                await containerClient.UploadBlobAsync(blobName, stream);
            }

            _logger.LogInformation("Data received: {0}", item);
            Exception x=new Exception();
            return x;
        }

        [Function("BulkInsert")]
        public void BulkInsert([Microsoft.Azure.Functions.Worker.BlobTrigger("multifilecontainer/{name}", Connection = "AzureWebJobsStorage")] byte[] myBlob, string name)
        {
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {myBlob}");
            try
            {
                _logger.LogInformation($"Processing blob '{name}'...");
                using (MemoryStream memoryStream = new MemoryStream(myBlob))
                {
                    // Process the blob contents (memoryStream) here
                    // For example, you can read the contents using a StreamReader
                    using (StreamReader reader = new StreamReader(memoryStream))
                    {
                        string jsonData = reader.ReadToEnd();

                        List<Model1> items = JsonConvert.DeserializeObject<List<Model1>>(jsonData);

                        foreach (Model1 item in items)
                        {
                            // Perform bulk insertion by calling _services.CreateItem(item)
                            _services.CreateItem(item);
                        }

                        _logger.LogInformation("Bulk insertion completed successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing blob '{name}': {ex.Message}");
            }
        }
    }
}
