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
    }
}
