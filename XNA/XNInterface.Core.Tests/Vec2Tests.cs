using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNInterface.Core.Tests
{
    [TestClass]
    public class Vec2Tests
    {
        [TestMethod]
        public void TestCreationEmptyCtor()
        {
            var v = new Vec2();
            Assert.AreEqual(0, v.X);
            Assert.AreEqual(0, v.Y);
        }

        [TestMethod]
        public void TestCreationFloatCtor()
        {
            var v = new Vec2(16, 9);
            Assert.AreEqual(16, v.X);
            Assert.AreEqual(9, v.Y);
        }

        [TestMethod]
        public void TestCreationFullCtor()
        {
            var v = new Vec2(2, Vec2.Vec2ValueType.Absolute, 0.5f, Vec2.Vec2ValueType.Percentage);
            Assert.AreEqual(2, v.X);
            Assert.AreEqual(Vec2.Vec2ValueType.Absolute, v.XType);
            Assert.AreEqual(0.5f, v.Y);
            Assert.AreEqual(Vec2.Vec2ValueType.Percentage, v.YType);
        }

        [TestMethod]
        public void TestCalculationAbsolute()
        {
            var v = new Vec2(10, Vec2.Vec2ValueType.Absolute, 35, Vec2.Vec2ValueType.Absolute);
            v.Calculate(500, 500);
            Assert.AreEqual(10, v.RealX);
            Assert.AreEqual(35, v.RealY);
        }

        [TestMethod]
        public void TestCalculationPercentage()
        {
            var v = new Vec2(1, Vec2.Vec2ValueType.Percentage, 0.5f, Vec2.Vec2ValueType.Percentage);
            v.Calculate(500, 500);
            Assert.AreEqual(500, v.RealX);
            Assert.AreEqual(250, v.RealY);
        }

        [TestMethod]
        public void TestCalculationMixed()
        {
            var v = new Vec2(10, Vec2.Vec2ValueType.Absolute, 0.5f, Vec2.Vec2ValueType.Percentage);
            v.Calculate(500, 500);
            Assert.AreEqual(10, v.RealX);
            Assert.AreEqual(250, v.RealY);
        }

        [TestMethod]
        public void TestPercentageBounds()
        {
            var v = new Vec2(-0.6f, Vec2.Vec2ValueType.Percentage, 1.2f, Vec2.Vec2ValueType.Percentage);
            v.Calculate(100, 100);
            Assert.AreEqual(0, v.RealX);
            Assert.AreEqual(100, v.RealY);
        }

        [TestMethod]
        public void TestBadParentSize()
        {
            var v = new Vec2(-0.6f, Vec2.Vec2ValueType.Percentage, 1.2f, Vec2.Vec2ValueType.Percentage);
            v.Calculate(-50, 0);
            Assert.AreEqual(0, v.RealX);
            Assert.AreEqual(1, v.RealY);
        }

        [TestMethod]
        public void TestEquality()
        {
            var v1 = new Vec2(5, 7);
            var v2 = new Vec2(5, 7);
            Assert.IsTrue(v1.Equals(v2));
        }

        [TestMethod]
        public void TestEqualityPercentage()
        {
            var v1 = new Vec2(5, Vec2.Vec2ValueType.Absolute, 0.5f, Vec2.Vec2ValueType.Percentage);
            var v2 = new Vec2(5, Vec2.Vec2ValueType.Absolute, 0.5f, Vec2.Vec2ValueType.Percentage);
            Assert.IsTrue(v1.Equals(v2));
        }

        [TestMethod]
        public void TestEqualityFalse()
        {
            var v1 = new Vec2(5, Vec2.Vec2ValueType.Absolute, 0.5f, Vec2.Vec2ValueType.Percentage);
            var v2 = new Vec2(5, Vec2.Vec2ValueType.Absolute, 0.5f, Vec2.Vec2ValueType.Absolute);
            Assert.IsFalse(v1.Equals(v2));
        }
    }
}
