using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Checkout.PaymentGateway.Application.Handlers.Abstractions;
using Checkout.PaymentGateway.Domain.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.PaymentGateway.Application.Handlers
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            var assembly = typeof(IServiceCollectionExtensions).Assembly;
            return AddHandlers(services, assembly.GetTypes());
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services, Type[] types)
        {
            _ = types ?? throw new ArgumentNullException(nameof(types));

            var handlers = types
                .Where(type => type.GetInterfaces().Any(i => IsHandlerInterface(i)))
                .Where(type => !ImplementsIDecorator(type))
                .ToList();

            var decoratorImplementationMapping = types
                .Where(type => IsDecorator(type))
                .ToDictionary(type => type.BaseType);

            handlers.ForEach(h => AddHandler(services, h, decoratorImplementationMapping));

            return services;
        }

        private static void AddHandler(IServiceCollection services, Type handler, Dictionary<Type, Type> decoratorImplementationMapping)
        {
            var attributes = handler.GetCustomAttributes(true);

            var decorators = attributes
                .Where(a => a.GetType().IsSubclassOf(typeof(HandlerAttribute)))
                .Select(a => (a as HandlerAttribute).Decorator)
                .ToList();

            var handlerType = handler.GetInterfaces().Single(i => IsHandlerInterface(i));
            var factory = BuildPipeline(decorators, handler, decoratorImplementationMapping);

            services.AddTransient(handlerType, factory);
        }

        private static Func<IServiceProvider, object> BuildPipeline(List<Type> decorators,
                                                                    Type handler,
                                                                    Dictionary<Type, Type> decoratorImplementationMapping)
        {
            var ctors = decorators
                .Select(d => decoratorImplementationMapping[d])
                .Concat(new [] { handler })
                .Select(d =>
                {
                    var constructorInfos = d.GetConstructors();
                    return constructorInfos.Single();
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
            if (!ImplementsIDecorator(type))
                return false;

            if (type.IsGenericType || type.IsAbstract)
                return false;

            for (var current = type; current.BaseType != typeof(object); current = type.BaseType)
            {
                if (!current.BaseType.IsGenericType)
                    continue;

                var typeDefinition = current.BaseType.GetGenericTypeDefinition();

                if (typeDefinition == typeof(CommandHandlerDecorator<>)
                    || typeDefinition == typeof(PreQueryHandlerDecorator<,>)
                    || typeDefinition == typeof(PostQueryHandlerDecorator<,>))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ImplementsIDecorator(Type type)
        {
            return type.GetInterfaces().Any(i => i == typeof(IDecorator));
        }
    }
}
