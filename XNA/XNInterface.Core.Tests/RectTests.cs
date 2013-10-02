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
    }
}
