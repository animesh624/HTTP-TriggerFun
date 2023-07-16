using CURD_APP.Data;
using CURD_APP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
//namespace keyword is used to declare a scope that contains a set of related objects. 
namespace CURD_APP.Controllers
{
    [Route("api/v1")]
    //[ApiController]  // this tells that this class is an API controller
    public class APIController : ControllerBase // manage operations related to controller
    {
        private readonly IAPIServices _service;

        public APIController(IAPIServices service)
        {
            _service = service;
        }
        //[Authorize]
        [HttpGet]
        public Response<List<Model1>> getItems()
        {
            List<Model1> items = _service.getItems();
            Response<List<Model1>> returnItems = new Response<List<Model1>>("Items retrieved successfully.", items);
            return returnItems;
        }
        //[Authorize]
        [HttpGet("{id:int}")]
        public ActionResult<Response<Model1>> getItem(int? id)
        {
            if (id == null || !ModelState.IsValid)
            {
                return BadRequest(new Response<Model1>("Please provide valid state.", null));
            }
            Model1 item = _service.getItem(id);

            if (item == null)
            {
                return NotFound(new Response<Model1>("Given Item with ID is not present", null));
            }

            return new Response<Model1>("Item retrieved successfully.", item);
        }
        //[Authorize]
        [HttpDelete("{id:int}")]
        public ActionResult<Response<Model1>> DeleteItem(int? id)
        {
            if(id==null)
            {
                return BadRequest(new Response<Model1>("Please provide valid ID.", null));
            }
            Model1 item = _service.DeleteItem(id);

            if (item==null)
            {
                return NotFound(new Response<Model1>("Given ID is not present", null));
            }

            return new Response<Model1>("Item deleted successfully.", item);

        }
        //[Authorize]
        [HttpPost]
        public ActionResult<Response<Model1>> CreateItem([FromBody]Model1 obj)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new Response<Model1>("Please provide valid Object", null));
            }
            var x=_service.CreateItem(obj);

            return new Response<Model1>("Item created successfully.", obj);

        }
        //[Authorize]
        [HttpPut("update/{id:int}")]
        public ActionResult<Response<Model1>> UpdateItem(int? id, [FromBody] Model1 obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<Model1>("Please provide valid Object to update", null));
            }
            if (id == null)
            {
                return BadRequest(new Response<Model1>("ID is inappropriate", null));
            }

            Model1 item = _service.UpdateItem(id, obj);

            if (item == null)
            {
                return NotFound(new Response<Model1>("Given item is not present to be Updated", null));
            }

            return new Response<Model1>("Item updated successfully.",obj);
        }
    }
}
