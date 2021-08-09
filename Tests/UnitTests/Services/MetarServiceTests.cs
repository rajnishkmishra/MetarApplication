using System.Threading.Tasks;
using NUnit.Framework;
using MetarApplication.Models;
using MetarApplication.Services;
using MetarApplication.Services.Helper;
using MetarApplication.Services.Mapper;
using NSubstitute;

namespace Tests
{
    public class MetarServiceTests
    {
        private IResponseMapper _responseMapper;
        private ICacheHelper _cacheHelper;
        private IMetarDataService _metarDataService;
        private IMetarService _metarService;
        

        [SetUp]
        public void Setup()
        {
            _responseMapper = Substitute.For<IResponseMapper>();
            _cacheHelper = Substitute.For<ICacheHelper>();
            _metarDataService = Substitute.For<IMetarDataService>();
            _metarService = new MetarService(_responseMapper, _cacheHelper, _metarDataService);
            
        }

        [Test]
        public void Test1()
        {
            //Arrange
            var scode = "KSGS";
            var headers = new Headers()
            {
                nocache = 0
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
            var taskSource = new TaskCompletionSource<Result>();
            taskSource.SetResult(mockResult);
            _cacheHelper.GetMetarData(Arg.Any<string>()).Returns(taskSource.Task);

            //Act
            var result = _metarService.GetData(headers, scode);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(mockResult.data.station, result.data.station);
            Assert.AreEqual(mockResult.data.last_observation, result.data.last_observation);
            Assert.AreEqual(mockResult.data.temperature, result.data.temperature);
            Assert.AreEqual(mockResult.data.wind, result.data.wind);
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
            var mockMetarData = "2021/08/06 14:15\nKSGS 061415Z AUTO 33004KT 7SM SCT012 SCT100 22/19 A2993 RMK AO2 T02170189";
            _metarDataService.GetMetarData(Arg.Any<string>()).Returns(mockMetarData);
            _responseMapper.MapResponse(Arg.Any<string>()).Returns(mockResult);
            _cacheHelper.SetMetarData(Arg.Any<Result>());

            //Act
            var result = _metarService.GetData(headers, scode);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(mockResult.data.station, result.data.station);
            Assert.AreEqual(mockResult.data.last_observation, result.data.last_observation);
            Assert.AreEqual(mockResult.data.temperature, result.data.temperature);
            Assert.AreEqual(mockResult.data.wind, result.data.wind);
        }

        [Test]
        public void Test3()
        {
            //Arrange
            var scode = "KSGS";
            var headers = new Headers()
            {
                nocache = 1
            };
            _metarDataService.GetMetarData(Arg.Any<string>());

            //Act
            var result = _metarService.GetData(headers, scode);

            //Assert
            Assert.IsNull(result);
        }
    }
}