﻿namespace ItemStore.WebApi.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException() : base("Item was not found")
        {

        }
    }
}
