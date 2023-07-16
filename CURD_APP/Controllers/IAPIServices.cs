using CURD_APP.Models;
using Microsoft.AspNetCore.Mvc;

namespace CURD_APP.Controllers
{
    public interface IAPIServices
    {
        public List<Model1> getItems();
        public Model1 getItem(int? id);
        public Model1 DeleteItem(int? id);
        public Model1 CreateItem([FromBody] Model1 obj);
        public Model1 UpdateItem(int? id, [FromBody] Model1 obj);
    }
}
