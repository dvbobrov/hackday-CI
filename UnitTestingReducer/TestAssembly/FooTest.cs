//Tests for Foo (at all 8):
//    BarPositive
//    BarNegative
//    BarNegative
//    BarNegative
//    Baz
//    TestFoo_FooInt1_FooSimple1
//    TestFoo_FooInt2_FooSimple1
//    TestFoo_FooInt1_FooSimple2
//    TestFoo_FooInt2_FooSimple2
//Tests for FooInt1 (at all 3):
//    TestFooInt1
//    TestFoo_FooInt1_FooSimple1
//    TestFoo_FooInt1_FooSimple2
//Tests for FooInt2 (at all 3):
//    TestFooInt2
//    TestFoo_FooInt2_FooSimple1
//    TestFoo_FooInt2_FooSimple2
//Tests for FooSimple1 (at all 3):
//    TestFooSimple1
//    TestFoo_FooInt1_FooSimple1
//    TestFoo_FooInt2_FooSimple1
//Tests for FooSimple2 (at all 3):
//    TestFooSimple2
//    TestFoo_FooInt1_FooSimple2
//    TestFoo_FooInt2_FooSimple2


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
        public void TestFooInt1()
        {
            var fooint1 = new FooInt1(2);

            Assert.AreEqual(2, fooint1);
        }

        [Test]
        public void TestFooInt2()
        {
            var fooint2 = new FooInt2(10);

            Assert.AreNotEqual(fooint2, 20);
        }

        [Test]
        public void TestFooSimple1()
        {
            var foosimple1 = new FooSimple1();

            Assert.True(foosimple1.Return(true));
        }

        [Test]
        public void TestFooSimple2()
        {
            var foosimple2 = new FooSimple2();

            Assert.False(foosimple2.Return(false));
        }

        [Test]
        public void TestFoo_FooInt1_FooSimple1()
        {
            var foo = new Foo();

            var fooint1 = new FooInt1(15);

            var foosimple1 = new FooSimple1();

            Assert.AreEqual(15, foo.Bar(15));

            Assert.AreNotEqual(1231, fooint1);

            Assert.False(foosimple1.Return(false));
        }

        [Test]
        public void TestFoo_FooInt2_FooSimple1()
        {
            var foo = new Foo();

            var fooint2 = new FooInt2(15);

            var foosimple1 = new FooSimple1();

            Assert.AreEqual(15, foo.Bar(15));

            Assert.AreNotEqual(1231, fooint2);

            Assert.False(foosimple1.Return(false));
        }

        [Test]
        public void TestFoo_FooInt1_FooSimple2()
        {
            var foo = new Foo();

            var fooint1 = new FooInt1(15);

            var foosimple2 = new FooSimple2();

            Assert.AreEqual(15, foo.Bar(15));

            Assert.AreNotEqual(1231, fooint1);

            Assert.False(foosimple2.Return(false));
        }

        [Test]
        public void TestFoo_FooInt2_FooSimple2()
        {
            var foo = new Foo();

            var fooint2 = new FooInt2(15);

            var foosimple2 = new FooSimple2();

            Assert.AreEqual(15, foo.Bar(15));

            Assert.AreNotEqual(1231, fooint2);

            Assert.False(foosimple2.Return(false));
        }

        public void Baz()
        {
            var foo = new Foo();
            Assert.AreEqual(5, foo.Baz(2, 3));
        }
    }
}