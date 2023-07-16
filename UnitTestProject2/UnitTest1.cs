using CURD_APP.Controllers;
using CURD_APP.Data;
using CURD_APP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace CURD_APP.UnitTestProject2
{
    public class APIControllerTests
    {
        [Fact]
        public void GetItems_ReturnsListOfItems()
        {
            // Arrange
            var mockDb = new Mock<ApplicationDbContext>();
            var controller = new APIController(mockDb.Object);
            var items = new List<Model1>
            {
                new Model1 { Id = 1, DishName = "Item 1" },
                new Model1 { Id = 2, DishName = "Item 2" }
            };
            mockDb.Setup(db => db.Dish.ToList()).Returns(items);

            // Act
            var result = controller.getItems();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<Response<List<Model1>>>(okResult.Value);
            Assert.Equal("Items retrieved successfully.", response.Message);
            Assert.Equal(items, response.Data);
        }

        [Fact]
        public void GetItem_WithValidId_ReturnsItem()
        {
            // Arrange
            var mockDb = new Mock<ApplicationDbContext>();
            var controller = new APIController(mockDb.Object);
            var item = new Model1 { Id = 1, DishName = "Item 1" };
            mockDb.Setup(db => db.Dish.FirstOrDefault(u => u.Id == 1)).Returns(item);

            // Act
            var result = controller.getItem(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<Response<Model1>>(okResult.Value);
            Assert.Equal("Item retrieved successfully.", response.Message);
            Assert.Equal(item, response.Data);
        }

        [Fact]
        public void GetItem_WithInvalidId_ReturnsBadRequest()
        {
            // Arrange
            var mockDb = new Mock<ApplicationDbContext>();
            var controller = new APIController(mockDb.Object);

            // Act
            var result = controller.getItem(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        // Similarly, you can write tests for other actions like DeleteItem, CreateItem, UpdateItem, etc.
    }
}
