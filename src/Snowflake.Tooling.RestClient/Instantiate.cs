using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Tooling.RestClient
{
    internal static class Instantiate
    {
        public static T CreateInstance<T>() where T : new ()
        {
            return new T();
        }

        private static ConstructorInfo GetConstructor<T>(BindingFlags flags, Type[] constructorParams)
        {
            return Instantiate.GetConstructor(flags, typeof(T), constructorParams);
       /*  from constructor in typeof(T).GetConstructors()
            where !constructor.GetParameters().Select(p => p.GetType()).Except(constructorParams).Any()
            select constructor).FirstOrDefault() */
        }

        private static ConstructorInfo GetConstructor(BindingFlags flags, Type type, Type[] constructorParams)
        {
            var constructors = from constructor in type.GetConstructors(flags)
                let parameters = constructor.GetParameters().Select(p => p.ParameterType)
                where !parameters.Except(constructorParams).Any()
                where parameters.Count() == constructorParams.Count()
                select constructor;
            return constructors.First();
            
        }
        public static T CreateInstance<T>(Type type)
        {
            return Instantiate.CreateInstance<T>(new[] {type});
        }

        public static T CreateInstance<T>(Type[] constructorParams)
        {
            Func<T> instanceCreator = Expression.Lambda<Func<T>>(
                Expression.New(Instantiate.GetConstructor<T>
                (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, constructorParams)))
                .Compile();
            return instanceCreator();
        }

        public static object CreateInstance(Type createType)
        {
            return Instantiate.CreateInstance(createType, Type.EmptyTypes);
        }

        public static object CreateInstance(Type createType, Type[] constructorParams)
        {
            Func<object> instanceCreator = Expression.Lambda<Func<object>>(
                 Expression.New(Instantiate.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, createType, constructorParams)))
                .Compile();
            return instanceCreator();
        }

        public static object CreateInstance(Type createType, Type[] constructorParams, params Expression[] args)
        {
            Func<object> instanceCreator = Expression.Lambda<Func<object>>(
               Expression.New(Instantiate.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, createType, constructorParams), args)).Compile();
            return instanceCreator();
        }
    }
}
