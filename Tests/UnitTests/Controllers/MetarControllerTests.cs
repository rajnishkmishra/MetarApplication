using System.Collections.Generic;
using NUnit.Framework;
using MetarApplication.Controllers;
using MetarApplication.Models;
using MetarApplication.Services;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Tests
{
    public class MetarControllerTests
    {
        private IMetarService _metarService;
        private MetarController _metarController;

        [SetUp]
        public void Setup()
        {
            _metarService = Substitute.For<IMetarService>();
            _metarController = new MetarController(_metarService);
        }

        [Test]
        public void Test1()
        {
            //Arrange
            var scode = "ABCD";
            var headers = new Headers()
            {
                nocache = 1
            };
            _metarService.GetData(Arg.Any<Headers>(), Arg.Any<string>()).ReturnsNull();

            //Act
            var result = _metarController.Get(headers,scode) as ObjectResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404,result.StatusCode);
            Dictionary<string,string> errorMessage = new Dictionary<string,string>();
            errorMessage = (Dictionary<string,string>)result.Value;
            Assert.AreEqual("station code not found.", errorMessage["message"]);
        }

        [Test]
        public void Test2()
        {
            //Arrange
            var scode = "KSGS";
            var headers = new Headers()
            {
                nocache = 1
            };
            var mockResult = new Result()
            {
                data = new Data()
                {
                    station = "KSGS",
                    last_observation = "2021/08/06 at 14:15 GMT",
                    temperature = "22 C (71.6 F)",
                    wind = "NNW at 5 mph (4 knots)"    
                }
            };
            _metarService.GetData(Arg.Any<Headers>(), Arg.Any<string>()).Returns(mockResult);

            //Act
            var result = _metarController.Get(headers,scode) as OkObjectResult;

            //Assert
            var resultObject = result.Value as Result;
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(result.Value);
            Assert.IsNotNull(resultObject.data);
            Assert.AreEqual(mockResult.data.station, resultObject.data.station);
            Assert.AreEqual(mockResult.data.last_observation, resultObject.data.last_observation);
            Assert.AreEqual(mockResult.data.temperature, resultObject.data.temperature);
            Assert.AreEqual(mockResult.data.wind, resultObject.data.wind);
        }
    }
}