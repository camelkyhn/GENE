using System;

namespace Gene.Middleware.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class PlaceHolderAttribute : Attribute
    {
        public string PlaceHolder { get; set; }

        public PlaceHolderAttribute(string placeHolder)
        {
            PlaceHolder = placeHolder;
        }
    }
}