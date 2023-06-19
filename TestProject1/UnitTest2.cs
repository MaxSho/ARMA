using DesARMA.Log;
using DesARMA.Log.Data;
using System.Text.RegularExpressions;

namespace TestProject1
{

    public class UnitTest2
    {
        readonly CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access, "");
        readonly CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access, "");
        readonly List<CreateRequestData> listTest = new()
            {
                new(null, DesARMA.Log.TypeLogData.Access, ""),
                new(null, DesARMA.Log.TypeLogData.Access, "")
            };

        public UnitTest2()
        {
            List<CreateRequestData> listTest = new()
            {
                createRequestData,
                createRequestDat2,
            };
        }


        [Theory]
        [InlineData("Access")]
        [InlineData("Access2")]
        public void Test1(string value)
        {
            //CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access, "");
            //CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access, "");

            //List<CreateRequestData> list = new()
            //{
            //    createRequestData,
            //    createRequestDat2,
            //};

            Assert.Contains(value, createRequestData.GetData());
        }
        [Fact]
        public void TestAll()
        {
            //CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access, "");
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access, "");

            //List<CreateRequestData> list = new()
            //{
            //    createRequestData,
            //    createRequestDat2,
            //};

            Assert.All(listTest, _ => { Assert.Equal(_.GetData(), createRequestData.GetData()); });
        }

    }


}