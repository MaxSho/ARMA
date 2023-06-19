using DesARMA.Log;
using DesARMA.Log.Data;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Diagnostics;

namespace MSTestProject
{
    [TestClass]
    public class UnitTest1
    {
        public TestContext testContext { get; set; }
        CreateRequestData createRequestData;
         CreateRequestData createRequestDat2;
         List<CreateRequestData> listTest;
        [TestInitialize]
        public void TestInitialize()
        {
            createRequestData = new(null, DesARMA.Log.TypeLogData.Access, "");
            createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access, "");
            listTest = new()
            {
                createRequestData,
                createRequestDat2
            };

            //Debug.WriteLine("" + createRequestDat2.GetHashCode());
            //.WriteLine("" + listTest.GetHashCode());
        }
        [TestCleanup]
        public void TestCleanUp()
        {
            Debug.WriteLine("Test CleanUp");
        }
        [TestMethod]
        public void Test1()
        {
            //CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            //CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            //List<CreateRequestData> list = new()
            //{
            //    createRequestData,
            //    createRequestDat2,
            //};
            //Debug.WriteLine("\n" + createRequestDat2.GetHashCode());
            //Debug.WriteLine("" + listTest.GetHashCode());
            CollectionAssert.Contains(listTest, createRequestDat2);
        }
        // DataSource - визначення джерела даних.
        // 1 параметр - ім'я провайдера
        // 2 параметр - рядок підключення або шлях до файлу
        // 3 параметр - ім'я таблиці або елемента в XML
        // 4 параметр - як відбувається доступ до записів з джерела даних
        //[DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
        //    "testData.xml",
        //    "User",
        //    DataAccessMethod.Sequential)]
        //[TestMethod]
        //public void UserManager_Add_FromXML()
        //{
        //    Debug.WriteLine("+");
        //    //string userId = Convert.ToString(testContext.Properties["userid"]);
        //    //string telephone = Convert.ToString(testContext.Properties["telephone"]);
        //    //string email = Convert.ToString(testContext.Properties["email"]);


        //}
        [TestMethod]
        [DynamicData(nameof(GetUserData), DynamicDataSourceType.Method)]
        public void UserManager_Add_FromXML(string userId, string telephone, string email)
        {
            Debug.WriteLine($"{userId} {telephone} {email}");
            //string userId = Convert.ToString(testContext.Properties["userid"]);
            //string telephone = Convert.ToString(testContext.Properties["telephone"]);
            //string email = Convert.ToString(testContext.Properties["email"]);


        }
       
        public static IEnumerable<object[]> GetUserData()
        {
            // Код для отримання даних з джерела даних (наприклад, з файлу XML)

            yield return new object[] { "userId1", "telephone1", "email1" };
            yield return new object[] { "userId2", "telephone2", "email2" };
            // І так далі...
        }
        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void Div_10_0_ExceptionExpected()
        {
            int x = 10, y = 0;

            int actual = x / y;
        }
    }
}