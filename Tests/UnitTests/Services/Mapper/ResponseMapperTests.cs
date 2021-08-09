using NUnit.Framework;
using MetarApplication.Services.Mapper;
using NSubstitute;

namespace Tests
{
    public class ResponseMapperTests
    {
        private IResponseMapper _responseMapper;

        [SetUp]
        public void Setup()
        {
            _responseMapper = Substitute.For<ResponseMapper>();
        }

        [Test]
        public void Test1()
        {
            //Arrange
            string sampleResponse = "2021/08/06 14:15\nKSGS 061415Z AUTO 33004KT 7SM SCT012 SCT100 22/19 A2993 RMK AO2 T02170189";

            //Act
            var mappedResponse = _responseMapper.MapResponse(sampleResponse);

            //Assert
            Assert.IsNotNull(mappedResponse);
            Assert.AreEqual("KSGS", mappedResponse.data.station);
            Assert.AreEqual("2021/08/06 at 14:15 GMT", mappedResponse.data.last_observation);
            Assert.AreEqual("22 C (71.6 F)", mappedResponse.data.temperature);
            Assert.AreEqual("NNW at 5 mph (4 knots)", mappedResponse.data.wind);
        }

        [Test]
        public void Test2()
        {
            //Arrange
            string sampleResponse = "2021/08/07 17:54\nKPHL 071754Z 19009G16KT 10SM BKN140 BKN250 31/16 A3006 RMK AO2 SLP179 T03060161 10306 20222 58008 $";

            //Act
            var mappedResponse = _responseMapper.MapResponse(sampleResponse);

            //Assert
            Assert.IsNotNull(mappedResponse);
            Assert.AreEqual("KPHL", mappedResponse.data.station);
            Assert.AreEqual("2021/08/07 at 17:54 GMT", mappedResponse.data.last_observation);
            Assert.AreEqual("31 C (87.8 F)", mappedResponse.data.temperature);
            Assert.AreEqual("S at 18 mph (16 knots)", mappedResponse.data.wind);
        }

        [Test]
        public void Test3()
        {
            //Arrange
            string sampleResponse = "2021/08/07 18:15\nPAUT 071815Z AUTO 00000KT 10SM SCT004 OVC021 10/M05 A2951 RMK AO2 T00951055 PWINO";

            //Act
            var mappedResponse = _responseMapper.MapResponse(sampleResponse);

            //Assert
            Assert.IsNotNull(mappedResponse);
            Assert.AreEqual("PAUT", mappedResponse.data.station);
            Assert.AreEqual("2021/08/07 at 18:15 GMT", mappedResponse.data.last_observation);
            Assert.AreEqual("10 C (50 F)", mappedResponse.data.temperature);
            Assert.AreEqual("CALM", mappedResponse.data.wind);
        }

        [Test]
        public void Test4()
        {
            //Arrange
            string sampleResponse = "2021/08/07 17:54\nKJAN 071754Z VRB04KT 10SM FEW047 31/18 A3008 RMK AO2 SLP182 T03060178 10311 20206 58003";

            //Act
            var mappedResponse = _responseMapper.MapResponse(sampleResponse);

            //Assert
            Assert.IsNotNull(mappedResponse);
            Assert.AreEqual("KJAN", mappedResponse.data.station);
            Assert.AreEqual("2021/08/07 at 17:54 GMT", mappedResponse.data.last_observation);
            Assert.AreEqual("31 C (87.8 F)", mappedResponse.data.temperature);
            Assert.AreEqual("5 mph (4 knots)", mappedResponse.data.wind);
        }
    }
}