namespace ItemStore.WebApi.Exceptions
{
    public class ItemListEmptyException : Exception
    {
        public ItemListEmptyException() : base("Item list is empty")
        { 
        
        }
    }
}
