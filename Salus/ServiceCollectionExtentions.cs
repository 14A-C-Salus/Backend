//using System;
//using System.Linq;
//using System.Reflection;
//using Microsoft.Extensions.DependencyInjection;

//public static class ServiceCollectionExtentions
//{
//    public static void AddAllGenericTypes(this IServiceCollection services
//        , Type t
//        , Assembly[] assemblies
//        , bool additionalRegisterTypesByThemself = false
//        , ServiceLifetime lifetime = ServiceLifetime.Transient
//    )
//    {
//        var genericType = t;
//        var typesFromAssemblies = assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces()
//            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType)));

//        foreach (var type in typesFromAssemblies)
//        {
//            services.Add(new ServiceDescriptor(t, type, lifetime));
//            if (additionalRegisterTypesByThemself)
//                services.Add(new ServiceDescriptor(type, type, lifetime));
//        }
//    }
//}
////source: https://gist.github.com/GetoXs/5caf0d8cfe6faa8a855c3ccef7c5a541