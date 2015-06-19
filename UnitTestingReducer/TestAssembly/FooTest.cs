namespace TestAssembly
{
    using NUnit.Framework;

    using TestedAssembly;

    [TestFixture]
    public class FooTest
    {
        [Test]
        public void BarPositive()
        {
            var foo = new Foo();
            Assert.AreEqual(1, foo.Bar(1));
        }

        [Test]
        public void BarNegative([Values(-1, -2, -10)] int val)
        {
            var foo = new Foo();
            Assert.AreEqual(-val, foo.Bar(val));
        }

        [Test]
        public void Baz()
        {
            var foo = new Foo();
            Assert.AreEqual(5, foo.Baz(2, 3));
        }
    }
}