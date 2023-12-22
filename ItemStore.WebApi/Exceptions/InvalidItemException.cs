namespace ItemStore.WebApi.Exceptions
{
    public class InvalidItemException : Exception
    {
        public InvalidItemException() : base("Item already exists")
        { }
    }
}
