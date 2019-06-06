namespace sdmap.Functional
{
    public static class Maybe
    {
        public static Maybe<T> Empty<T>() where T : class
        {
            return new Maybe<T>();
        }
    }
}
