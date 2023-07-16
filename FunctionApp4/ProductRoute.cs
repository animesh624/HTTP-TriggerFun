using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using CURD_APP.Controllers;
using CURD_APP.Models;
using CURD_APP.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Raven.Database.Server.Controllers;

namespace FunctionApp4
{
    public class ProductRoute : ControllerBase
    {
        private readonly ILogger<ProductRoute> _logger;
        private IAPIServices _services;
        public ProductRoute(IAPIServices services, ILogger<ProductRoute>logger)
        {
            _services= services;
            _logger = logger;
        }

        [Function("GetAllItems")]
        public ActionResult<List<Model1>> GetAllItems([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            _logger.LogInformation("Getting All Items Please wait.....");
            List<Model1> items=_services.getItems();

            return items;
        }
        [Function("CreateItem")]
        public async Task<ActionResult<Response<Model1>>> CreateItem([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Model1 item = JsonConvert.DeserializeObject<Model1>(requestBody);
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<Model1>("Please provide valid Object", null));
            }
            item = _services.CreateItem(item);
            return new Response<Model1>("Item created successfully.", item);
        }
        [Function("GetByID")]
        public async Task<ActionResult<Response<Model1>>> GetByID([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetByID/{id}")] HttpRequestData req, int? id)
        {

            if (id == null || !ModelState.IsValid)
            {
                return BadRequest(new Response<Model1>("Please provide valid state.", null));
            }

            Model1 item = _services.getItem(id);
            if (item == null)
            {
                return NotFound(new Response<Model1>("Given Item with ID is not present", null));
            }
            return new Response<Model1>("Item retrieved successfully.", item);
        }

        [Function("DeleteItem")]
        public async Task<ActionResult<Response<Model1>>> DeleteItem([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "delete/{id}")] HttpRequestData req, int ?id)
        {
            if (id == null)
            {
                return BadRequest(new Response<Model1>("Please provide valid ID.", null));
            }
            Model1 item = _services.DeleteItem(id);

            if (item == null)
            {
                return NotFound(new Response<Model1>("Given ID is not present", null));
            }

            return new Response<Model1>("Item deleted successfully.", item);
        }

        [Function("UpdateItem")]
        public async Task<ActionResult<Response<Model1>>> UpdateItem([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "update/{id}")] HttpRequestData req, int id)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Model1 item = JsonConvert.DeserializeObject<Model1>(requestBody);
            item = _services.UpdateItem(id, item);

            return new Response<Model1>("Item created successfully.", item);
        }
    }
}
