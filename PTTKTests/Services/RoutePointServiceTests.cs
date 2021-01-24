using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PTTK.Models;
using PTTK.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PTTK.Services.Tests
{
    [TestClass()]
    public class RoutePointServiceTests
    {

        private void MockDbContext(out Mock<PTTKContext> pttkContextMock, out Mock<DbSet<RoutePoint>> routePointsMock)
        {
            var routePointsInDb = new List<RoutePoint>
            {
                new RoutePoint() { Id = 1, Name = "Przyklad", Cordinates = "32° 65' 34\" N 32° 53' 12\" W", Height = 239 },
                new RoutePoint() { Id = 2, Name = "Gora", Cordinates = "32° 65' 32\" S 32° 53' 54\" W", Height = 239 }
            }.AsQueryable();

            routePointsMock = new Mock<DbSet<RoutePoint>>();
            routePointsMock.As<IQueryable<RoutePoint>>().Setup(m => m.Provider).Returns(routePointsInDb.Provider);
            routePointsMock.As<IQueryable<RoutePoint>>().Setup(m => m.Expression).Returns(routePointsInDb.Expression);
            routePointsMock.As<IQueryable<RoutePoint>>().Setup(m => m.ElementType).Returns(routePointsInDb.ElementType);
            routePointsMock.As<IQueryable<RoutePoint>>().Setup(m => m.GetEnumerator()).Returns(routePointsInDb.GetEnumerator());

            pttkContextMock = new Mock<PTTKContext>();
            pttkContextMock.Setup(x => x.RoutePoints).Returns(routePointsMock.Object);
        }

        [TestMethod()]
        public void CreateRoutePoint_create_userCreated()
        {
            //Arrange
            MockDbContext(out Mock<PTTKContext> pttkContextMock, out Mock<DbSet<RoutePoint>> routePointsMock);
            RoutePointService service = new RoutePointService(pttkContextMock.Object);

            string expectedName = "Testowy";
            string expectedDescription = "Opis";
            string expectedCordinates = "54° 65' 34\" N 32° 73' 12\" W";
            int expectedHeight = 1235;

            RoutePoint routePoint = new RoutePoint();
            routePoint.Name = expectedName;
            routePoint.Description = expectedDescription;
            routePoint.Cordinates = expectedCordinates;
            routePoint.Height = expectedHeight;

            //Act
            service.CreateRoutePoint(routePoint);

            //Assert
            routePointsMock.Verify(
                x => x.Add(It.Is<RoutePoint>(r => r.Name == expectedName 
                       && r.Description == expectedDescription
                       && r.Cordinates == expectedCordinates
                       && r.Height == expectedHeight)),
                Times.Once);

            pttkContextMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [TestMethod()]
        public void CreateRoutePoint_duplicate_exception()
        {
            //Arrange
            MockDbContext(out Mock<PTTKContext> pttkContextMock, out Mock<DbSet<RoutePoint>> routePointsMock);
            RoutePointService service = new RoutePointService(pttkContextMock.Object);

            RoutePoint routePoint = new RoutePoint();
            routePoint.Name = "Przyklad";
            routePoint.Description = "Opis";
            routePoint.Cordinates = "54° 65' 34\" N 32° 73' 12\" W";
            routePoint.Height = 1235;

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => service.CreateRoutePoint(routePoint));
            pttkContextMock.Verify(x => x.SaveChanges(), Times.Never);
        }

        [TestMethod()]
        public void CreateRoutePoint_nameNull_exception()
        {
            //Arrange
            MockDbContext(out Mock<PTTKContext> pttkContextMock, out Mock<DbSet<RoutePoint>> routePointsMock);
            RoutePointService service = new RoutePointService(pttkContextMock.Object);

            RoutePoint routePoint = new RoutePoint();
            routePoint.Name = null;
            routePoint.Description = "Opis";
            routePoint.Cordinates = "54° 65' 34\" N 32° 73' 12\" W";
            routePoint.Height = 1235;

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => service.CreateRoutePoint(routePoint));
            pttkContextMock.Verify(x => x.SaveChanges(), Times.Never);
        }

        [TestMethod()]
        public void CreateRoutePoint_cordinatesNull_exception()
        {
            //Arrange
            MockDbContext(out Mock<PTTKContext> pttkContextMock, out Mock<DbSet<RoutePoint>> routePointsMock);
            RoutePointService service = new RoutePointService(pttkContextMock.Object);

            RoutePoint routePoint = new RoutePoint();
            routePoint.Name = "Testowy";
            routePoint.Description = "Opis";
            routePoint.Cordinates = null;
            routePoint.Height = 1235;

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => service.CreateRoutePoint(routePoint));
            pttkContextMock.Verify(x => x.SaveChanges(), Times.Never);
        }

        [TestMethod()]
        public void CreateRoutePoint_heightNegative_exception()
        {
            //Arrange
            MockDbContext(out Mock<PTTKContext> pttkContextMock, out Mock<DbSet<RoutePoint>> routePointsMock);
            RoutePointService service = new RoutePointService(pttkContextMock.Object);

            RoutePoint routePoint = new RoutePoint();
            routePoint.Name = "Testowy";
            routePoint.Description = "Opis";
            routePoint.Cordinates = "54° 65' 34\" N 32° 73' 12\" W";
            routePoint.Height = -12;

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => service.CreateRoutePoint(routePoint));
            pttkContextMock.Verify(x => x.SaveChanges(), Times.Never);
        }

        [TestMethod()]
        public void CreateRoutePoint_nameToLong_exception()
        {
            //Arrange
            MockDbContext(out Mock<PTTKContext> pttkContextMock, out Mock<DbSet<RoutePoint>> routePointsMock);
            RoutePointService service = new RoutePointService(pttkContextMock.Object);

            RoutePoint routePoint = new RoutePoint();
            routePoint.Name = new string('a', 51);
            routePoint.Description = "Opis";
            routePoint.Cordinates = "54° 65' 34\" N 32° 73' 12\" W";
            routePoint.Height = 1235;

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => service.CreateRoutePoint(routePoint));
            pttkContextMock.Verify(x => x.SaveChanges(), Times.Never);
        }

        [TestMethod()]
        public void CreateRoutePoint_descriptionToLong_exception()
        {
            //Arrange
            MockDbContext(out Mock<PTTKContext> pttkContextMock, out Mock<DbSet<RoutePoint>> routePointsMock);
            RoutePointService service = new RoutePointService(pttkContextMock.Object);

            RoutePoint routePoint = new RoutePoint();
            routePoint.Name = "Testowy";
            routePoint.Description = new string('h', 501);
            routePoint.Cordinates = "54° 65' 34\" N 32° 73' 12\" W";
            routePoint.Height = 1235;

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => service.CreateRoutePoint(routePoint));
            pttkContextMock.Verify(x => x.SaveChanges(), Times.Never);
        }

        [TestMethod()]
        public void CreateRoutePoint_cordinatesImproperFormat1_exception()
        {
            //Arrange
            MockDbContext(out Mock<PTTKContext> pttkContextMock, out Mock<DbSet<RoutePoint>> routePointsMock);
            RoutePointService service = new RoutePointService(pttkContextMock.Object);

            RoutePoint routePoint = new RoutePoint();
            routePoint.Name = "Testowy";
            routePoint.Description = null;
            routePoint.Cordinates = "54° 65' 34\" A 32° 73' 12\" W";
            routePoint.Height = 1235;

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => service.CreateRoutePoint(routePoint));
            pttkContextMock.Verify(x => x.SaveChanges(), Times.Never);
        }

        [TestMethod()]
        public void CreateRoutePoint_cordinatesImproperFormat2_exception()
        {
            //Arrange
            MockDbContext(out Mock<PTTKContext> pttkContextMock, out Mock<DbSet<RoutePoint>> routePointsMock);
            RoutePointService service = new RoutePointService(pttkContextMock.Object);

            RoutePoint routePoint = new RoutePoint();
            routePoint.Name = "Testowy";
            routePoint.Description = null;
            routePoint.Cordinates = "54° 65' 34\" N 32° 73' 12\" B";
            routePoint.Height = 1235;

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => service.CreateRoutePoint(routePoint));
            pttkContextMock.Verify(x => x.SaveChanges(), Times.Never);
        }

        [TestMethod()]
        public void CreateRoutePoint_cordinatesImproperFormat3_exception()
        {
            //Arrange
            MockDbContext(out Mock<PTTKContext> pttkContextMock, out Mock<DbSet<RoutePoint>> routePointsMock);
            RoutePointService service = new RoutePointService(pttkContextMock.Object);

            RoutePoint routePoint = new RoutePoint();
            routePoint.Name = "Testowy";
            routePoint.Description = null;
            routePoint.Cordinates = "54° 65' 4\" N 32° 73' 12\" W";
            routePoint.Height = 1235;

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => service.CreateRoutePoint(routePoint));
            pttkContextMock.Verify(x => x.SaveChanges(), Times.Never);
        }





        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////






        [TestMethod()]
        public void EditRoutePoint_edit_userEdited()
        {
            //Arrange
            MockDbContext(out Mock<PTTKContext> pttkContextMock, out Mock<DbSet<RoutePoint>> routePointsMock);
            RoutePointService service = new RoutePointService(pttkContextMock.Object);

            int id = 1;
            string expectedName = "Testowy";
            string expectedDescription = "Opis";
            string expectedCordinates = "54° 65' 34\" N 32° 73' 12\" W";
            int expectedHeight = 1235;

            RoutePoint routePoint = new RoutePoint();
            routePoint.Id = id;
            routePoint.Name = expectedName;
            routePoint.Description = expectedDescription;
            routePoint.Cordinates = expectedCordinates;
            routePoint.Height = expectedHeight;

            //Act
            service.EditRoutePoint(routePoint);

            //Assert
            var routePointFromMock = routePointsMock.Object.Where(r => r.Id == id).First();
            Assert.AreEqual(expectedName, routePointFromMock.Name);
            Assert.AreEqual(expectedDescription, routePointFromMock.Description);
            Assert.AreEqual(expectedCordinates, routePointFromMock.Cordinates);
            Assert.AreEqual(expectedHeight, routePointFromMock.Height);

            pttkContextMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [TestMethod()]
        public void EditRoutePoint_editWithoutChangingName_userEdited()
        {
            //Arrange
            MockDbContext(out Mock<PTTKContext> pttkContextMock, out Mock<DbSet<RoutePoint>> routePointsMock);
            RoutePointService service = new RoutePointService(pttkContextMock.Object);

            int id = 1;
            string expectedName = "Przyklad";
            string expectedDescription = "Opis";
            string expectedCordinates = "54° 65' 34\" N 32° 73' 12\" W";
            int expectedHeight = 1235;

            RoutePoint routePoint = new RoutePoint();
            routePoint.Id = id;
            routePoint.Name = expectedName;
            routePoint.Description = expectedDescription;
            routePoint.Cordinates = expectedCordinates;
            routePoint.Height = expectedHeight;

            //Act
            service.EditRoutePoint(routePoint);

            //Assert
            var routePointFromMock = routePointsMock.Object.Where(r => r.Id == id).First();
            Assert.AreEqual(expectedName, routePointFromMock.Name);
            Assert.AreEqual(expectedDescription, routePointFromMock.Description);
            Assert.AreEqual(expectedCordinates, routePointFromMock.Cordinates);
            Assert.AreEqual(expectedHeight, routePointFromMock.Height);

            pttkContextMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [TestMethod()]
        public void EditRoutePoint_duplicate_exception()
        {
            //Arrange
            MockDbContext(out Mock<PTTKContext> pttkContextMock, out Mock<DbSet<RoutePoint>> routePointsMock);
            RoutePointService service = new RoutePointService(pttkContextMock.Object);

            RoutePoint routePoint = new RoutePoint();
            routePoint.Id = 2;
            routePoint.Name = "Przyklad";
            routePoint.Description = "Opis";
            routePoint.Cordinates = "54° 65' 34\" N 32° 73' 12\" W";
            routePoint.Height = 1235;

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => service.EditRoutePoint(routePoint));
            pttkContextMock.Verify(x => x.SaveChanges(), Times.Never);
        }

        [TestMethod()]
        public void EditRoutePoint_nameNull_exception()
        {
            //Arrange
            MockDbContext(out Mock<PTTKContext> pttkContextMock, out Mock<DbSet<RoutePoint>> routePointsMock);
            RoutePointService service = new RoutePointService(pttkContextMock.Object);

            RoutePoint routePoint = new RoutePoint();
            routePoint.Id = 1;
            routePoint.Name = null;
            routePoint.Description = "Opis";
            routePoint.Cordinates = "54° 65' 34\" N 32° 73' 12\" W";
            routePoint.Height = 1235;

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => service.EditRoutePoint(routePoint));
            pttkContextMock.Verify(x => x.SaveChanges(), Times.Never);
        }

        [TestMethod()]
        public void EditRoutePoint_cordinatesNull_exception()
        {
            //Arrange
            MockDbContext(out Mock<PTTKContext> pttkContextMock, out Mock<DbSet<RoutePoint>> routePointsMock);
            RoutePointService service = new RoutePointService(pttkContextMock.Object);

            RoutePoint routePoint = new RoutePoint();
            routePoint.Id = 1;
            routePoint.Name = "Testowy";
            routePoint.Description = "Opis";
            routePoint.Cordinates = null;
            routePoint.Height = 1235;

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => service.EditRoutePoint(routePoint));
            pttkContextMock.Verify(x => x.SaveChanges(), Times.Never);
        }

        [TestMethod()]
        public void EditRoutePoint_heightNegative_exception()
        {
            //Arrange
            MockDbContext(out Mock<PTTKContext> pttkContextMock, out Mock<DbSet<RoutePoint>> routePointsMock);
            RoutePointService service = new RoutePointService(pttkContextMock.Object);

            RoutePoint routePoint = new RoutePoint();
            routePoint.Id = 1;
            routePoint.Name = "Testowy";
            routePoint.Description = "Opis";
            routePoint.Cordinates = "54° 65' 34\" N 32° 73' 12\" W";
            routePoint.Height = -12;

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => service.EditRoutePoint(routePoint));
            pttkContextMock.Verify(x => x.SaveChanges(), Times.Never);
        }

        [TestMethod()]
        public void EditRoutePoint_nameToLong_exception()
        {
            //Arrange
            MockDbContext(out Mock<PTTKContext> pttkContextMock, out Mock<DbSet<RoutePoint>> routePointsMock);
            RoutePointService service = new RoutePointService(pttkContextMock.Object);

            RoutePoint routePoint = new RoutePoint();
            routePoint.Id = 1;
            routePoint.Name = new string('a', 51);
            routePoint.Description = "Opis";
            routePoint.Cordinates = "54° 65' 34\" N 32° 73' 12\" W";
            routePoint.Height = 1235;

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => service.EditRoutePoint(routePoint));
            pttkContextMock.Verify(x => x.SaveChanges(), Times.Never);
        }

        [TestMethod()]
        public void EditRoutePoint_descriptionToLong_exception()
        {
            //Arrange
            MockDbContext(out Mock<PTTKContext> pttkContextMock, out Mock<DbSet<RoutePoint>> routePointsMock);
            RoutePointService service = new RoutePointService(pttkContextMock.Object);

            RoutePoint routePoint = new RoutePoint();
            routePoint.Id = 1;
            routePoint.Name = "Testowy";
            routePoint.Description = new string('h', 501);
            routePoint.Cordinates = "54° 65' 34\" N 32° 73' 12\" W";
            routePoint.Height = 1235;

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => service.EditRoutePoint(routePoint));
            pttkContextMock.Verify(x => x.SaveChanges(), Times.Never);
        }

        [TestMethod()]
        public void EditRoutePoint_cordinatesImproperFormat1_exception()
        {
            //Arrange
            MockDbContext(out Mock<PTTKContext> pttkContextMock, out Mock<DbSet<RoutePoint>> routePointsMock);
            RoutePointService service = new RoutePointService(pttkContextMock.Object);

            RoutePoint routePoint = new RoutePoint();
            routePoint.Id = 1;
            routePoint.Name = "Testowy";
            routePoint.Description = null;
            routePoint.Cordinates = "54° 65' 34\" A 32° 73' 12\" W";
            routePoint.Height = 1235;

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => service.EditRoutePoint(routePoint));
            pttkContextMock.Verify(x => x.SaveChanges(), Times.Never);
        }

        [TestMethod()]
        public void EditRoutePoint_cordinatesImproperFormat2_exception()
        {
            //Arrange
            MockDbContext(out Mock<PTTKContext> pttkContextMock, out Mock<DbSet<RoutePoint>> routePointsMock);
            RoutePointService service = new RoutePointService(pttkContextMock.Object);

            RoutePoint routePoint = new RoutePoint();
            routePoint.Id = 1;
            routePoint.Name = "Testowy";
            routePoint.Description = null;
            routePoint.Cordinates = "54° 65' 34\" N 32° 73' 12\" B";
            routePoint.Height = 1235;

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => service.EditRoutePoint(routePoint));
            pttkContextMock.Verify(x => x.SaveChanges(), Times.Never);
        }

        [TestMethod()]
        public void EditRoutePoint_cordinatesImproperFormat3_exception()
        {
            //Arrange
            MockDbContext(out Mock<PTTKContext> pttkContextMock, out Mock<DbSet<RoutePoint>> routePointsMock);
            RoutePointService service = new RoutePointService(pttkContextMock.Object);

            RoutePoint routePoint = new RoutePoint();
            routePoint.Id = 1;
            routePoint.Name = "Testowy";
            routePoint.Description = null;
            routePoint.Cordinates = "54° 65' 4\" N 32° 73' 12\" W";
            routePoint.Height = 1235;

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => service.EditRoutePoint(routePoint));
            pttkContextMock.Verify(x => x.SaveChanges(), Times.Never);
        }
    }
}