using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNInterface.Core.Tests
{
    [TestClass]
    public class RectTests
    {
        [TestMethod]
        public void TestEmptyCtor()
        {
            var r = new Rect();
            Assert.AreEqual(r.Position, new Vec2());
            Assert.AreEqual(r.Size, new Vec2());
        }

        [TestMethod]
        public void TestAllFloatCtor()
        {
            var r = new Rect(1, 2, 3, 4);
            Assert.AreEqual(r.Position, new Vec2(1, 2));
            Assert.AreEqual(r.Size, new Vec2(3, 4));
        }

        [TestMethod]
        public void TestVec2Ctor()
        {
            var r = new Rect(new Vec2(1, 2), new Vec2(3, 4));
            Assert.AreEqual(r.Position, new Vec2(1, 2));
            Assert.AreEqual(r.Size, new Vec2(3, 4));
        }

        [TestMethod]
        public void TestFloatParent()
        {
            var r = new Rect(new Vec2(1, 0.5f, Vec2.Vec2ValueType.Percentage),
                new Vec2(0.5f, 1, Vec2.Vec2ValueType.Percentage));
            r.Calculate(100, 100);
            Assert.AreEqual(100, r.Position.RealX);
            Assert.AreEqual(50, r.Position.RealY);
            Assert.AreEqual(50, r.Size.RealX);
            Assert.AreEqual(100, r.Size.RealY);
        }

        [TestMethod]
        public void TestVec2Parent()
        {
            var r = new Rect(new Vec2(1, 0.5f, Vec2.Vec2ValueType.Percentage),
                new Vec2(0.5f, 1, Vec2.Vec2ValueType.Percentage));
            r.Calculate(new Vec2(100, 100));
            Assert.AreEqual(100, r.Position.RealX);
            Assert.AreEqual(50, r.Position.RealY);
            Assert.AreEqual(50, r.Size.RealX);
            Assert.AreEqual(100, r.Size.RealY);
        }

        [TestMethod]
        public void TestRectParent()
        {
            var r = new Rect(new Vec2(1, 0.5f, Vec2.Vec2ValueType.Percentage),
                new Vec2(0.5f, 1, Vec2.Vec2ValueType.Percentage));
            r.Calculate(new Rect(0, 0, 100, 100));
            Assert.AreEqual(100, r.Position.RealX);
            Assert.AreEqual(50, r.Position.RealY);
            Assert.AreEqual(50, r.Size.RealX);
            Assert.AreEqual(100, r.Size.RealY);
        }
    }
}
