using System;
using System.Collections.Generic;
using System.Text;
using TwoWayDictionary;
using Xunit;

namespace TwoWayDictionaryTests
{    
    public class TwoWayDictionaryTests
    {
        TwoWayDictionary<int, string> dict;
        int[] nums = new int[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 1, 2, 3, 4, 5, 6, 7, 8, 9, 11, 12, 13, 14, 15, 16, 17, 18, 19, 99, 100 };
        string[] strs = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "!", "?", ",", "." };


        #region Creation Tests

        [Fact]
        public void CreateTwoWayDictionary()
        {
            dict = new TwoWayDictionary<int, string>();
            Assert.NotNull(dict);
        }

        [Fact]
        public void FailOnSameGenericType()
        {
            TwoWayDictionary<int, int> invalidDict = null;
            try
            {
                invalidDict = new TwoWayDictionary<int, int>();
            }
            catch (System.TypeInitializationException)
            {
                Assert.Null(invalidDict);
                return;
            }

            Assert.True(false);
        }

        #endregion

        #region Adding-only Tests

        [Fact]
        public void AddElementsTest()
        {
            dict = new TwoWayDictionary<int, string>();

            for (int i = 0; i < 30; i++)
                dict.Add(nums[i], strs[i]);

            for (int i = 0; i < 30; i++)
                Assert.True(dict.Contains(nums[i]));

            for (int i = 0; i < 30; i++)
                Assert.True(dict.Contains(strs[i]));

            Assert.True(dict.Count == 30);
            VerifyIntegrity<int, string>(dict);
        }

        [Fact]
        public void AddDuplicateKeyTest()
        {
            dict = new TwoWayDictionary<int, string>();

            dict.Add(nums[0], strs[0]);

            bool caughtException = false;
            try
            {
                dict.Add(nums[0], strs[0]);
            }
            catch (System.ArgumentException)
            {
                caughtException = true;
            }

            Assert.True(caughtException);
            VerifyIntegrity<int, string>(dict);

            caughtException = false;
            try
            {
                dict.Add(nums[0], strs[1]);
            }
            catch (System.ArgumentException)
            {
                caughtException = true;
            }

            Assert.True(caughtException);
            VerifyIntegrity<int, string>(dict);
        }

        [Fact]
        public void AddDuplicateValueTest()
        {
            dict = new TwoWayDictionary<int, string>();

            dict.Add(nums[0], strs[0]);

            bool caughtException = false;
            try
            {
                dict.Add(nums[0], strs[0]);
            }
            catch
            {
                caughtException = true;
            }

            if (!caughtException) Assert.True(false);
            VerifyIntegrity<int, string>(dict);

            caughtException = false;
            try
            {
                dict.Add(nums[1], strs[0]);
            }
            catch (System.ArgumentException)
            {
                caughtException = true;
            }

            if (!caughtException) Assert.True(false);
            VerifyIntegrity<int, string>(dict);
        }

        [Fact]
        public void AddByDirectAccess()
        {
            dict = new TwoWayDictionary<int, string>();

            for (int i = 0; i < 30; i++)
                dict[nums[i]] = strs[i];

            for (int i = 0; i < 30; i++)
                Assert.True(dict.Contains(nums[i]));

            for (int i = 0; i < 30; i++)
                Assert.True(dict.Contains(strs[i]));

            Assert.True(dict.Count == 30);
            VerifyIntegrity<int, string>(dict);
        }
        #endregion

        #region Remove Tests

        [Fact]
        public void RemoveElementsByKeyTest()
        {
            dict = new TwoWayDictionary<int, string>();

            for (int i = 0; i < 30; i++)
                dict.Add(nums[i], strs[i]);

            for (int i = 0; i < 30; i += 2)
                dict.Remove(nums[i]);

            for (int i = 1; i < 30; i += 2)
                Assert.True(dict.Contains(nums[i]));

            for (int i = 1; i < 30; i += 2)
                Assert.True(dict.Contains(strs[i]));

            Assert.True(dict.Count == 15);
            VerifyIntegrity<int, string>(dict);
        }

        [Fact]
        public void RemoveElementsByValueTest()
        {
            dict = new TwoWayDictionary<int, string>();

            for (int i = 0; i < 30; i++)
                dict.Add(nums[i], strs[i]);

            for (int i = 0; i < 30; i += 2)
                dict.Remove(strs[i]);

            for (int i = 1; i < 30; i += 2)
                Assert.True(dict.Contains(nums[i]));

            for (int i = 1; i < 30; i += 2)
                Assert.True(dict.Contains(strs[i]));

            Assert.True(dict.Count == 15);
            VerifyIntegrity<int, string>(dict);
        }

        #endregion

        #region Modify Test

        [Fact]
        public void ChangeValueTest()
        {
            dict = new TwoWayDictionary<int, string>();

            for (int i = 0; i < 30; i++)
                dict.Add(nums[i], strs[i]);

            dict[nums[0]] = "test";

            Assert.False(dict.Contains(strs[0]));
            Assert.Equal(dict[nums[0]], "test");
            VerifyIntegrity<int, string>(dict);
        }

        [Fact]
        public void FailChangeValueTest()
        {
            dict = new TwoWayDictionary<int, string>();

            for (int i = 0; i < 30; i++)
                dict.Add(nums[i], strs[i]);

            bool caughtException = false;
            try
            {
                dict[nums[0]] = "b";
            }
            catch (System.Exception)
            {
                caughtException = true;
            }

            Assert.True(caughtException);
            Assert.Equal(dict[nums[0]], strs[0]);
            VerifyIntegrity<int, string>(dict);

            caughtException = false;
            try
            {
                dict[nums[0]] = null;
            }
            catch (System.Exception)
            {
                caughtException = true;
            }

            Assert.True(caughtException);
            Assert.Equal(dict[nums[0]], strs[0]);
            VerifyIntegrity<int, string>(dict);
        }

        [Fact]
        public void ChangeKeyTest()
        {
            dict = new TwoWayDictionary<int, string>();

            for (int i = 0; i < 30; i++)
                dict.Add(nums[i], strs[i]);

            dict[strs[0]] = 9999;

            Assert.False(dict.Contains(nums[0]));
            Assert.Equal(dict[strs[0]], 9999);
            VerifyIntegrity<int, string>(dict);
        }

        [Fact]
        public void FailChangeKeyTest()
        {
            dict = new TwoWayDictionary<int, string>();

            for (int i = 0; i < 30; i++)
                dict.Add(nums[i], strs[i]);

            bool caughtException = false;
            try
            {
                dict[strs[0]] = 10;
            }
            catch (System.Exception)
            {
                caughtException = true;
            }

            Assert.True(caughtException);
            Assert.Equal(dict[strs[0]], nums[0]);
            VerifyIntegrity<int, string>(dict);
        }


        [Fact]
        public void OverwriteKeyTest()
        {
            dict = new TwoWayDictionary<int, string>();

            for (int i = 0; i < 30; i++)
                dict.Add(nums[i], strs[i]);

            dict.Overwrite(nums[5], "test", dict[nums[5]]);
            VerifyIntegrity<int, string>(dict);
        }

        [Fact]
        public void OverwriteValueTest()
        {
            dict = new TwoWayDictionary<int, string>();

            for (int i = 0; i < 30; i++)
                dict.Add(nums[i], strs[i]);

            dict.Overwrite(dict[nums[5]], 9999, nums[5]);
            VerifyIntegrity<int, string>(dict);
        }

        #endregion

        [Fact]
        public void ContainsTest()
        {
            dict = new TwoWayDictionary<int, string>();

            dict.Add(nums[0], strs[0]);
            Assert.True(dict.Contains(nums[0]) && dict.Contains(strs[0]));

            try
            {
                dict.Contains(null);
            }
            catch (System.ArgumentException)
            {
                return;
            }

            Assert.True(false);
        }

        [Fact]
        public void UseCaseTest()
        {
            TwoWayDictionary<TestClass1, TestClass2> myDict = new TwoWayDictionary<TestClass1, TestClass2>();

            List<TestClass1> classes1 = new List<TestClass1>();
            List<TestClass2> classes2 = new List<TestClass2>();

            for (int i = 0; i < 80; i++)
            {
                classes1.Add(new TestClass1());
                classes2.Add(new TestClass2());

                myDict.Add(classes1[i], classes2[i]);
            }

            VerifyIntegrity<TestClass1, TestClass2>(myDict);

            bool exceptionCaught = false;
            try
            {
                myDict[classes1[23]] = classes2[9];
            }
            catch (System.ArgumentException)
            {
                exceptionCaught = true;
            }

            Assert.True(exceptionCaught);
            VerifyIntegrity<TestClass1, TestClass2>(myDict);


            exceptionCaught = false;
            try
            {
                myDict[classes2[12]] = classes1[33];
            }
            catch (System.ArgumentException)
            {
                exceptionCaught = true;
            }

            Assert.True(exceptionCaught);
            VerifyIntegrity<TestClass1, TestClass2>(myDict);

            myDict.Remove(classes1[50]);
            Assert.False(myDict.Contains(classes1[50]));
            Assert.True(myDict.Count == 79);

            myDict.Remove(classes2[20]);
            Assert.False(myDict.Contains(classes2[20]));
            Assert.True(myDict.Count == 78);

            exceptionCaught = false;
            try
            {
                myDict.Remove(classes2[20]);
            }
            catch (System.Exception)
            {
                exceptionCaught = true;
            }
            Assert.True(exceptionCaught);
            Assert.False(myDict.Contains(classes2[20]));
            Assert.True(myDict.Count == 78);

            exceptionCaught = false;
            try
            {
                myDict.Remove(new TestClass1());
            }
            catch (System.Exception)
            {
                exceptionCaught = true;
            }
            Assert.True(exceptionCaught);
            Assert.True(myDict.Count == 78);

            myDict.Add(classes1[50], classes2[20]);
            Assert.True(myDict.Contains(classes1[50]) && myDict.Contains(classes2[20]) && myDict[classes1[50]] == classes2[20] && myDict.Count == 79);
            VerifyIntegrity<TestClass1, TestClass2>(myDict);

            myDict.Overwrite(classes1[12], new TestClass2(), myDict[classes1[12]]);
            VerifyIntegrity<TestClass1, TestClass2>(myDict);

            myDict.Overwrite(classes2[40], new TestClass1(), myDict[classes2[40]]);
            VerifyIntegrity<TestClass1, TestClass2>(myDict);
        }

        private class TestClass1
        {
            public int a;
            public int b;
        }
        private class TestClass2 : TestClass1
        {
            public string s;
        }

        /// <summary>
        /// Verifies whether the dictionary is integer at the moment this function is called.
        /// See the class invariant in TwoWayDictionary.cs
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dict"></param>
        void VerifyIntegrity<T1, T2>(TwoWayDictionary<T1, T2> dict)
        {
            // Check if dictionary is initialized
            Assert.NotNull(dict);

            // Check if the count if the dictionary, forward and backward hold the same amount of keys
            int count = dict.Count;
            Assert.Equal(count, dict.Forward.Count);
            Assert.Equal(count, dict.Backward.Count);

            IReadOnlyCollection<T1> forwKeys = dict.Forward.Keys;
            IReadOnlyCollection<T2> backwKeys = dict.Backward.Keys;
            IReadOnlyCollection<T2> forwValues = dict.Forward.Values;
            IReadOnlyCollection<T1> backWValues = dict.Backward.Values;

            // Check if the forward keys are equal to the backwards values, and vice versa
            Assert.Equal(forwKeys, backWValues);
            Assert.Equal(backwKeys, forwValues);

            foreach (var elem in forwKeys) Assert.True(dict.Contains(elem));
            foreach (var elem in backwKeys) Assert.True(dict.Contains(elem));
            foreach (var elem in forwValues) Assert.True(dict.Contains(elem));
            foreach (var elem in backWValues) Assert.True(dict.Contains(elem));
        }
    }
}
