using FunctionApp6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FunctionApp6.Services
{
    public interface IAPIServices
    {
        public List<Model1> getItems();
        public Model1 getItem(int? id);
        public Model1 DeleteItem(int? id);
        public Model1 CreateItem(Model1 obj);
        public Model1 UpdateItem(int? id,Model1 obj);
    }
}
