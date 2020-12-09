using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Checkout.PaymentGateway.Domain.Framework;

namespace Checkout.PaymentGateway.Application.Handlers.Abstractions
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public abstract class HandlerAttribute : Attribute
    {
        public HandlerAttribute(Type decoratorType, Type commandType)
        {
            _ = decoratorType ?? throw new ArgumentNullException(nameof(decoratorType));
            _ = commandType ?? throw new ArgumentNullException(nameof(commandType));

            CheckDecorator(decoratorType);

            if (decoratorType.BaseType == typeof(CommandHandlerDecorator<>))
            {
                throw new ArgumentException($"The parameter {nameof(decoratorType)} must be a type that inherits from {typeof(CommandHandlerDecorator<>)}", nameof(decoratorType));
            }

            if (!commandType.GetInterfaces().Any(i => i == typeof(ICommand)))
            {
                throw new ArgumentException($"The parameter {nameof(commandType)} must be a type that implements {typeof(ICommand)}.", nameof(commandType));
            }

            Decorator = decoratorType.MakeGenericType(commandType);
        }

        public HandlerAttribute(Type decoratorType, Type queryType, Type queryResultType, DecoratorExecutionTime executionTime)
        {
            _ = decoratorType ?? throw new ArgumentNullException(nameof(decoratorType));
            _ = queryType ?? throw new ArgumentNullException(nameof(queryType));
            _ = queryResultType ?? throw new ArgumentNullException(nameof(queryResultType));

            CheckQueryDecorator(decoratorType, executionTime);

            if (!queryType.GetInterfaces().Any(i => i == typeof(IQuery)))
            {
                throw new ArgumentException($"The parameter {nameof(queryType)} must be a type that implements {typeof(IQuery)}.", nameof(queryType));
            }

            if (!queryResultType.IsClass)
            {
                throw new ArgumentException($"The parameter {nameof(queryResultType)} must be a class type.", nameof(queryResultType));
            }


            Decorator = decoratorType.MakeGenericType(queryType, queryResultType);
        }

        private static void CheckDecorator(Type decoratorType)
        {
            if (!decoratorType.IsGenericType || !decoratorType.BaseType.IsGenericType)
            {
                throw new ArgumentException($"The parameter {nameof(decoratorType)} must be a generic type.", nameof(decoratorType));
            }
        }

        private static void CheckQueryDecorator(Type decoratorType, DecoratorExecutionTime executionTime)
        {
            CheckDecorator(decoratorType);

            if (executionTime == DecoratorExecutionTime.Pre && decoratorType.BaseType.GetGenericTypeDefinition() != typeof(PreQueryHandlerDecorator<,>))
            {
                throw new ArgumentException($"The parameter {nameof(decoratorType)} must be a type that inherits from {typeof(PreQueryHandlerDecorator<,>)}.", nameof(decoratorType));
            }
            else if (executionTime == DecoratorExecutionTime.Post && decoratorType.BaseType.GetGenericTypeDefinition() != typeof(PostQueryHandlerDecorator<,>))
            {
                throw new ArgumentException($"The parameter {nameof(decoratorType)} must be a type that inherits from {typeof(PostQueryHandlerDecorator<,>)}.", nameof(decoratorType));
            }
        }

        public Type Decorator { get; }
    }
}
