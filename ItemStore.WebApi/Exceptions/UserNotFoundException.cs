namespace ItemStore.WebApi.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("User was not found")
        {

        }
    }
}
