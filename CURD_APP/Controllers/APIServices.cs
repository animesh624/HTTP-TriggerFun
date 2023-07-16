using CURD_APP.Data;
using CURD_APP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
//namespace keyword is used to declare a scope that contains a set of related objects. 
namespace CURD_APP.Controllers
{
    public class APIServices : IAPIServices // manage operations related to controller
    {
        private readonly ApplicationDbContext _db;

        public APIServices(ApplicationDbContext db)
        {
            _db = db;
        }
        public List<Model1> getItems()
        {
            List<Model1> items = _db.Dish.ToList();
            return items;
        }
        public Model1 getItem(int? id)
        {
            
            Model1 item = _db.Dish.FirstOrDefault(u => u.Id == id);

            return item;
        }
        public Model1 DeleteItem(int? id)
        {
            
            Model1 item = _db.Dish.FirstOrDefault(u => u.Id == id);

            
            _db.Dish.Remove(item);
            _db.SaveChanges();

            return item;
        }
        public Model1 CreateItem([FromBody] Model1 obj)
        {
            
            _db.Dish.Add(obj);
            _db.SaveChanges();

            return obj;
        }
        public Model1 UpdateItem(int? id, [FromBody] Model1 obj)
        {
            

            Model1 item = _db.Dish.FirstOrDefault(u => u.Id == id);
            _db.Entry(item).State = EntityState.Detached;
            _db.Dish.Update(obj);
            _db.SaveChanges();

            return obj;
        }
    }
}
