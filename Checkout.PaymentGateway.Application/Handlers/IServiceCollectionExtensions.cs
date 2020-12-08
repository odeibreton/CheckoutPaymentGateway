using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Checkout.PaymentGateway.Domain.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.PaymentGateway.Application.Handlers
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            var assembly = typeof(IServiceCollectionExtensions).Assembly;
            return AddHandlers(services, assembly);
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services, Assembly assembly)
        {
            _ = assembly ?? throw new ArgumentNullException(nameof(assembly));

            var handlers = assembly.GetTypes()
                .Where(type => type.GetInterfaces().Any(i => IsHandlerInterface(i)))
                .Where(type => !IsDecorator(type))
                .ToList();

            handlers.ForEach(h => AddHandler(services, h));

            return services;
        }

        private static void AddHandler(IServiceCollection services, Type handler)
        {
            var attributes = handler.GetCustomAttributes(false);

            var decorators = attributes
                .Where(a => a.GetType().IsSubclassOf(typeof(CommandHandlerAttribute)))
                .Select(a => (a as CommandHandlerAttribute).Decorator)
                .ToList();

            var handlerType = handler.GetInterfaces().Single(i => IsHandlerInterface(i));
            var factory = BuildPipeline(decorators, handler, handlerType);

            services.AddTransient(handlerType, factory);
        }

        private static Func<IServiceProvider, object> BuildPipeline(List<Type> decorators, Type handler, Type handlerType)
        {
            var ctors = decorators
                .Concat(new [] { handler })
                .Select(d =>
                {
                    var decorator = d.IsGenericType ? d.MakeGenericType(handlerType.GenericTypeArguments) : d;
                    return decorator.GetConstructors().Single();
                })
                .Reverse()
                .ToList();

            object factory(IServiceProvider provider)
            {
                object current = null;

                foreach (ConstructorInfo ctor in ctors)
                {
                    List<ParameterInfo> parameterInfos = ctor.GetParameters().ToList();

                    object[] parameters = GetParameters(parameterInfos, current, provider);

                    current = ctor.Invoke(parameters);
                }

                return current;
            }

            return factory;
        }

        private static object[] GetParameters(List<ParameterInfo> parameterInfos, object current, IServiceProvider provider)
        {
            var result = new object[parameterInfos.Count];

            for (int i = 0; i < parameterInfos.Count; i++)
            {
                result[i] = GetParameter(parameterInfos[i], current, provider);
            }

            return result;
        }

        private static object GetParameter(ParameterInfo parameterInfo, object current, IServiceProvider provider)
        {
            Type parameterType = parameterInfo.ParameterType;

            if (IsHandlerInterface(parameterType))
                return current;

            object service = provider.GetService(parameterType);
            if (service != null)
                return service;

            throw new ArgumentException($"Type {parameterType} not found.");
        }

        private static bool IsHandlerInterface(Type type)
        {
            if (!type.IsGenericType)
                return false;

            var typeDefinition = type.GetGenericTypeDefinition();

            return typeDefinition == typeof(ICommandHandler<>) || typeDefinition == typeof(IQueryHandler<,>);
        }

        private static bool IsDecorator(Type type)
        {
            if (!type.IsGenericType)
                return false;

            var typeDefinition = type.GetGenericTypeDefinition();

            return typeDefinition == typeof(CommandHandlerDecorator<>) || typeDefinition.IsSubclassOf(typeof(CommandHandlerDecorator<>));
        }
    }
}
