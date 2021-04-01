using System;

namespace Gene.Middleware.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TooltipAttribute : Attribute
    {
        public string Message { get; set; }

        public string Position { get; set; }

        public TooltipAttribute() { }

        public TooltipAttribute(string message, string position)
        {
            Message  = message;
            Position = position;
        }
    }
}