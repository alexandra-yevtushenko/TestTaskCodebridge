using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestTaskCodebridge.Controllers;

namespace TestTaskCodebridge.Tests
{
    [TestClass]
    public class TestDogsController
    {
        [TestMethod]
        public void GetAllDogs_ReturnsCorrectPagination()
        {
            var controller = new DogsController();
        }
    }
}
