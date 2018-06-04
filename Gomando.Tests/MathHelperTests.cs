
using Gomando.Logic.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gomando.Tests
{
    [TestClass]
    public class MathHelperTests
    {


        [TestMethod]
        public void ToRadianTest1()
        {
            double actual = MathHelper.ToRadian(0);
            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public void ToRadianTest2()
        {
            double actual = MathHelper.ToRadian(90);
            Assert.AreEqual(Math.PI / 2, actual);
        }

        [TestMethod]
        public void ToRadianTest3()
        {
            double actual = MathHelper.ToRadian(135);
            Assert.AreEqual(Math.PI * 0.75, actual);
        }

        [TestMethod]
        public void ToRadianTest4()
        {
            double actual = MathHelper.ToRadian(720);
            Assert.AreEqual(4 * Math.PI, actual);
        }

        [TestMethod]
        public void ToRadianTest5()
        {
            double actual = MathHelper.ToRadian(-90);
            Assert.AreEqual(-Math.PI / 2, actual);
        }

        [TestMethod]
        public void HaversineDistanceTest1()
        {
            double actual = (int)MathHelper.HaversineDistance(0, 0, 0, 0);
            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public void HaversineDistanceTest2()
        {
            double actual = (int)MathHelper.HaversineDistance(0, 0, 45, 45);
            Assert.AreEqual(6679, actual);
        }

        [TestMethod]
        public void HaversineDistanceTest3()
        {
            double actual = (int)MathHelper.HaversineDistance(25, 25, 26, 26);
            Assert.AreEqual(149, actual);
        }

        [TestMethod]
        public void HaversineDistanceTest4()
        {
            double actual = (int)MathHelper.HaversineDistance(11.125, 30.5, 11.125, 30.55);
            Assert.AreEqual(5, actual);
        }

        [TestMethod]
        public void HaversineDistanceTest5()
        {
            double actual = (int)MathHelper.HaversineDistance(20, 0, 0, 20);
            Assert.AreEqual(3115, actual);
        }

        [TestMethod]
        public void HaversineDistanceTest6()
        {
            double actual = (int)MathHelper.HaversineDistance(22, 20, 0, 0);
            Assert.AreEqual(3272, actual);
        }
        
        [TestMethod]
        public void HaversineDistanceTest7()
        {
            double actual = (int)MathHelper.HaversineDistance(11.125, 30.5, 0, -10);
            Assert.AreEqual(4647, actual);
        }

        [TestMethod]
        public void ConvertSecondsToTimeStringTest1()
        {
            string actual = MathHelper.ConvertSecondsToTimeString(0);
            Assert.AreEqual("0", actual);
        }

        [TestMethod]
        public void ConvertSecondsToTimeStringTest2()
        {
            string actual = MathHelper.ConvertSecondsToTimeString(30);
            Assert.AreEqual("30", actual);
        }

        [TestMethod]
        public void ConvertSecondsToTimeStringTest3()
        {
            string actual = MathHelper.ConvertSecondsToTimeString(125);
            Assert.AreEqual("2:05", actual);
        }

        [TestMethod]
        public void ConvertSecondsToTimeStringTest4()
        {
            string actual = MathHelper.ConvertSecondsToTimeString(1125);
            Assert.AreEqual("18:45", actual);
        }

        [TestMethod]
        public void ConvertSecondsToTimeStringTest5()
        {
            string actual = MathHelper.ConvertSecondsToTimeString(3888);
            Assert.AreEqual("1:04:48", actual);
        }

        [TestMethod]
        public void ConvertSecondsToTimeStringTest6()
        {
            string actual = MathHelper.ConvertSecondsToTimeString(70000);
            Assert.AreEqual("19:26:40", actual);
        }
    }
}
