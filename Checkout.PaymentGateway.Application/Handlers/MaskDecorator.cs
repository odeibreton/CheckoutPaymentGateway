using System;
using System.Collections.Generic;
using System.Text;
using Checkout.PaymentGateway.Application.Handlers.Abstractions;
using Checkout.PaymentGateway.Domain.Framework;

namespace Checkout.PaymentGateway.Application.Handlers
{
    public abstract class MaskDecorator<TQuery, TResult> : PostQueryHandlerDecorator<TQuery, TResult>
        where TQuery : IQuery
        where TResult : class
    {
        protected MaskDecorator(IQueryHandler<TQuery, TResult> internalHandler) : base(internalHandler)
        {
        }

        protected static string Mask(string value, int visibleCharacters)
        {
            var formattedNumber = value.Replace(" ", "");
            var last4digits = formattedNumber.Remove(0, formattedNumber.Length - visibleCharacters);
            return new string('*', formattedNumber.Length - visibleCharacters) + last4digits;
        }
    }

    public class MaskAttribute : HandlerAttribute
    {
        public MaskAttribute(Type queryType, Type queryResultType) : base(typeof(MaskDecorator<,>), queryType, queryResultType, DecoratorExecutionTime.Post)
        {
        }
    }
}
