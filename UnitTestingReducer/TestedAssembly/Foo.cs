namespace TestedAssembly
{
    public class Foo
    {
        public int Bar(int x)
        {
            if (x < 0)
            {
                return -x;
            }

            return x;
        }

        public int Baz(int x, int y)
        {
            return x + y;
        }
    }

    public class FooInt1
    {
        private int foo;
        public FooInt1(int i)
        {
            foo = i;
        }

        public static implicit operator int(FooInt1 foo)
        {
            return foo.foo;
        }

        public static implicit operator FooInt1(int i)
        {
            return new FooInt1(i);
        }
    }

    public class FooInt2
    {
        private int foo;
        public FooInt2(int i)
        {
            foo = i;
        }

        public static implicit operator int(FooInt2 foo)
        {
            return foo.foo;
        }

        public static implicit operator FooInt2(int i)
        {
            return new FooInt2(i);
        }
    }

    public class FooSimple1
    {
        public bool Return(bool b)
        { return b; }
    }

    public class FooSimple2
    {
        public bool Return(bool b)
        { return b; }
    }

}
