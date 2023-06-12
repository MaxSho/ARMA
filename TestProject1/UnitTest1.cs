using DesARMA.Log;
using DesARMA.Log.Data;
using System.Text.RegularExpressions;

namespace TestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };

            //Assert.NotNull(createRequestData);
            //Assert.Equal(createRequestData, new CreateRequestData(null, createRequestDat2));
            //.Equal(createRequestData, createRequestData);
            //Assert.Collection(list, _ => { Assert.NotEmpty(_.GetData()); }, _ => { Assert.NotEmpty(_.GetData()); });
            Assert.Contains("Access", createRequestData.GetData());
        }
        [Fact]
        public void TestAll()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };

            Assert.All(list, _ => { Assert.Equal(_.GetData(), createRequestData.GetData()); });
        }
        [Fact]
        public void TestCollection()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };

            Assert.Collection(list, _ => { Assert.NotEmpty(_.GetData()); }, _ => { Assert.NotEmpty(_.GetData()); });
            
        }
        [Fact]
        public void TestContains()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };

            Assert.Contains("Access", createRequestData.GetData());
            Assert.Contains(createRequestData, list);
            Assert.Contains("Access", createRequestData.GetData(), StringComparison.OrdinalIgnoreCase);
        }
        [Fact]
        public void TestContainsGeneric()
        {
            CreateRequestData createRequestData = new (null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new (null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };

            Assert.Contains<CreateRequestData>(createRequestData, list, EqualityComparer<CreateRequestData>.Default);
        }
        [Fact]
        public void TestDistinct()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };

            //list.Add(createRequestData);
            Assert.Distinct(list);
        }
        [Fact]
        public void TestDoesNotContain()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };

            Assert.DoesNotContain("Access1", createRequestData.GetData());
            Assert.DoesNotContain(new(null, DesARMA.Log.TypeLogData.Access), list);
            Assert.DoesNotContain("Access1", createRequestData.GetData(), StringComparison.OrdinalIgnoreCase);
        }
        [Fact]
        public void TestDoesNotContainGeneric()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };

            Assert.DoesNotContain<CreateRequestData>(new(null, DesARMA.Log.TypeLogData.Access), list);

        }
        [Fact]
        public void TestDoesNotMatch()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };

            Assert.DoesNotMatch(new Regex(@"^\d{3}-\d{3}-\d{4}$"), "123-456-7890!");

        }
        [Fact]
        public void TestEmpty()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };

            Assert.Empty(new List<CreateRequestData>());
        }
        [Fact]
        public void TestEndsWith()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };

            Assert.EndsWith("3", "123");
        }
        [Fact]
        public void TestEqual()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };

            Assert.Equal(list, list);
        }
        [Fact]
        public void TestEqualGeneric()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };

            Assert.Equal<CreateRequestData>(list, list);
            Assert.Equal<CreateRequestData>(createRequestData, createRequestData);
        }
        [Fact]
        public void TestEquivalent()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };

            Assert.Equivalent(createRequestData, createRequestDat2); // have equal public field
        }
        [Fact]
        public void TestFail()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };
            if (false)
            {
                Assert.Fail("My fail");
            }
            
        }
        [Fact]
        public void TestFalse()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };
            Assert.False(list is string);

        }
        [Fact]
        public void TestInRange()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };
            Assert.InRange(list.Count, 0, 100);


        }
        [Fact]
        public void TestIsAssignableFrom()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };
            Assert.IsAssignableFrom(typeof(IDataLog), createRequestData);
            
        }
        [Fact]
        public void TestIsAssignableFromGeneric()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };
            Assert.IsAssignableFrom<IDataLog>(createRequestData);
        }
        [Fact]
        public void TestIsNotType()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };
            Assert.IsNotType(typeof(string), createRequestData);
        }
        [Fact]
        public void TestIsNotTypeGeneric()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };
            Assert.IsNotType<string>(createRequestData);
        }
        [Fact]
        public void TestIsType()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };
            Assert.IsType(typeof(CreateRequestData), createRequestData);
            
        }
        [Fact]
        public void TestIsTypeGeneric()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };
            Assert.IsType<CreateRequestData>(createRequestData);
        }
        [Fact]
        public void TestMatches()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };
            Assert.Matches(new Regex(@"^\d{3}-\d{3}-\d{4}$"), "123-456-7890");

        }
        [Fact]
        public void TestMultiple()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };
            Assert.Multiple(() =>
            {
                Assert.Matches(new Regex(@"^\d{3}-\d{3}-\d{4}$"), "123-456-7890");
                //Assert.Matches(new Regex(@"^\d{3}-\d{3}-\d{4}$"), "123-456-7890!");
                //Assert.Matches(new Regex(@"^\d{3}-\d{3}-\d{4}$"), "123-456-7890S");
            });


        }
        [Fact]
        public void TestNotEmpty()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };
            Assert.NotEmpty(list);
        }
        [Fact]
        public void TestNotEqual()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };
            int expected = 0;
            Assert.NotEqual(list.Count, expected);
        }
        [Fact]
        public void TestNotEqualGeneric()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };
            Assert.NotEqual<CreateRequestData>(list, new List<CreateRequestData>());
        }
        [Fact]
        public void TestNotInRange()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };
            Assert.NotInRange(list.Count, 3, 7);
        }
        [Fact]
        public void TestNotNull()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };
            Assert.NotNull(list);
        }
        [Fact]
        public void TestNotSame()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };
            Assert.NotSame(createRequestData, createRequestDat2);
        }
        [Fact]
        public void TestNotStrictEqual()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };
            Assert.NotStrictEqual(createRequestData, createRequestDat2);
        }
        [Fact]
        public void TestNull()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            List<CreateRequestData> list = new()
            {
                createRequestData,
                createRequestDat2,
            };
            Assert.Null(null);
        }
        [Fact]
        public void TestProperSubset()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            HashSet<CreateRequestData> list = new()
            {
                createRequestData
            };
            HashSet<CreateRequestData> list2 = new()
            {
                createRequestData,
                createRequestDat2,
            };


            Assert.ProperSubset<CreateRequestData>(list2, list);
        }
        [Fact]
        public void TestProperSuperset()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            HashSet<CreateRequestData> list = new()
            {
                createRequestData
            };
            HashSet<CreateRequestData> list2 = new()
            {
                createRequestData,
                createRequestDat2,
            };


            Assert.ProperSuperset<CreateRequestData>(list, list2);
            //Assert.ProperSuperset<CreateRequestData>(list, list); faile
        }
        [Fact]
        public void TestSame()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            HashSet<CreateRequestData> list = new()
            {
                createRequestData
            };
            HashSet<CreateRequestData> list2 = new()
            {
                createRequestData,
                createRequestDat2,
            };
            Assert.Same(list, list);


        }
        [Fact]
        public void TestSingle()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            HashSet<CreateRequestData> list = new()
            {
                createRequestData
            };
            HashSet<CreateRequestData> list2 = new()
            {
                createRequestData,
                createRequestDat2,
            };
            Assert.Single(list);

        }
        [Fact]
        public void TestStartsWith()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);

            HashSet<CreateRequestData> list = new()
            {
                createRequestData
            };
            HashSet<CreateRequestData> list2 = new()
            {
                createRequestData,
                createRequestDat2,
            };
            Assert.StartsWith("1", "123");

        }
        [Fact]
        public void TestStrictEqual()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);
            var sfd = createRequestData;

            HashSet<CreateRequestData> list = new()
            {
                createRequestData
            };
            HashSet<CreateRequestData> list2 = new()
            {
                createRequestData,
                createRequestDat2,
            };
            Assert.StrictEqual(createRequestData, sfd);

        }
        [Fact]
        public void TestSubset()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);
            var sfd = createRequestData;

            HashSet<CreateRequestData> list = new()
            {
                createRequestData
            };
            HashSet<CreateRequestData> list2 = new()
            {
                createRequestData,
                createRequestDat2,
            };
            Assert.Subset(list2, list);

        }
        [Fact]
        public void TestSuperset()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);
            var sfd = createRequestData;

            HashSet<CreateRequestData> list = new()
            {
                createRequestData
            };
            HashSet<CreateRequestData> list2 = new()
            {
                createRequestData,
                createRequestDat2,
            };
            Assert.Superset(list, list2);
            Assert.Superset(list, list);

        }
        [Fact]
        public void TestThrows()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);
            var sfd = createRequestData;

            HashSet<CreateRequestData> list = new()
            {
                createRequestData
            };
            HashSet<CreateRequestData> list2 = new()
            {
                createRequestData,
                createRequestDat2,
            };
            //Assert.Throws<Exception>(() => { createRequestData.GetData(); });
            int numerator = 10;
            int denominator = 0;
            Assert.Throws<DivideByZeroException>(() =>
            {
                int result = numerator / denominator;
            });
        }
        [Fact]
        public void TestThrowsAny()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);
            var sfd = createRequestData;

            HashSet<CreateRequestData> list = new()
            {
                createRequestData
            };
            HashSet<CreateRequestData> list2 = new()
            {
                createRequestData,
                createRequestDat2,
            };
            int numerator = 10;
            int denominator = 0;
            Assert.ThrowsAny<Exception>(() =>
            {
                int result = numerator / denominator;
            });

        }
        [Fact]
        public void TestTrue()
        {
            CreateRequestData createRequestData = new(null, DesARMA.Log.TypeLogData.Access);
            CreateRequestData createRequestDat2 = new(null, DesARMA.Log.TypeLogData.Access);
            var sfd = createRequestData;

            HashSet<CreateRequestData> list = new()
            {
                createRequestData
            };
            HashSet<CreateRequestData> list2 = new()
            {
                createRequestData,
                createRequestDat2,
            };
            int numerator = 10;
            int denominator = 10;

            Assert.True(numerator == denominator);
        }
    }
}