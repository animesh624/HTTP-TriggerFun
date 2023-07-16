using CURD_APP.Controllers;
using CURD_APP.Data;
using CURD_APP.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using Xunit;

namespace TestProject1
{
    public class UnitTest1
    {
        private readonly Mock<IAPIServices> productService;

        public UnitTest1()
        {
            productService= new Mock<IAPIServices>();
        }
        [Fact]
        public void getItems_Test()
        {
            // Arrange
            List<Model1> items = new List<Model1>
            {
                new Model1 { Id = 1, DishName = "Item 1",Price=2,Weight=32.2 },
                new Model1 { Id = 2, DishName = "Item 2",Price=3,Weight=34.2 }
            };
            productService.Setup(x => x.getItems()).Returns(items);
            var controller = new APIController(productService.Object);

            // Act
            var result = controller.getItems();


            // Assert
            var actionResult = Assert.IsType<ActionResult<Response<List<Model1>>>>(result);
            var returnValue = Assert.IsType<Response<List<Model1>>>(actionResult.Value);
            var idea = returnValue.Data;
            Assert.Equal(items, idea);

            Assert.Equal(items.Count(), idea.Count());
            Assert.Equal(items.ToString(), idea.ToString());
            Assert.True(items.Equals(idea));
        }

        [Fact]
        public void getItem_Test()
        {
            // Arrange
            var items = new List<Model1>
            {
                new Model1 { Id = 1, DishName = "Item 1" },
                new Model1 { Id = 2, DishName = "Item 2" }
            };

            productService.Setup(x => x.getItem(2)).Returns(items[1]);
            var controller = new APIController(productService.Object);

            // Act
            var result = controller.getItem(2);


            // Assert
            var actionResult = Assert.IsType<ActionResult<Response<Model1>>>(result);
            var returnValue = Assert.IsType<Response<Model1>>(actionResult.Value);
            var idea = returnValue.Data;
            Assert.Equal(items[1], idea);
            Assert.Equal(items[1].ToString(), idea.ToString());
            Assert.True(items[1].Equals(idea));
        }

        [Fact]
        public void DeleteItem_Test()
        {
            // Arrange
            var items = new List<Model1>
            {
                new Model1 { Id = 1, DishName = "Item 1" },
                new Model1 { Id = 2, DishName = "Item 2" }
            };
            productService.Setup(x => x.DeleteItem(2)).Returns(items[1]);
            var controller = new APIController(productService.Object);

            // Act
            var result = controller.DeleteItem(2);


            // Assert
            var actionResult = Assert.IsType<ActionResult<Response<Model1>>>(result);
            var returnValue = Assert.IsType<Response<Model1>>(actionResult.Value);
            var idea = returnValue.Data;
            Assert.Equal(items[1].Id, idea.Id);
            Assert.Equal(items[1].ToString(), idea.ToString());
            Assert.True(items[1].Equals(idea));
        }

        [Fact]
        public void CreateItem_Test()
        {
            // Arrange
            var items = new List<Model1>
            {
                new Model1 { Id = 1, DishName = "Item 1" },
                new Model1 { Id = 2, DishName = "Item 2" }
            };
            productService.Setup(x => x.CreateItem(items[1])).Returns(items[1]);
            var controller = new APIController(productService.Object);

            // Act
            var result = controller.CreateItem(items[1]);


            // Assert
            var actionResult = Assert.IsType<ActionResult<Response<Model1>>>(result);
            var returnValue = Assert.IsType<Response<Model1>>(actionResult.Value);
            var idea = returnValue.Data;
            Assert.Equal(items[1].Id, idea.Id);
            Assert.Equal(items[1], idea);
            Assert.Equal(items[1].ToString(), idea.ToString());
            Assert.True(items[1].Equals(idea));
        }
    }
}
