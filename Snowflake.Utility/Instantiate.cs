﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Utility
{
    public static class Instantiate
    {
        public static T CreateInstance<T>() where T : new ()
        {
            return new T();
        }

        public static T CreateInstance<T>(Type type)
        {
            return Instantiate.CreateInstance<T>(new[] {type});
        }

        public static T CreateInstance<T>(Type[] types)
        {
            Func<T> instanceCreator = Expression.Lambda<Func<T>>(
                Expression.New(typeof(T).GetConstructor(types))
              ).Compile();
            return instanceCreator();
        }

        public static object CreateInstance(Type createType)
        {
            return Instantiate.CreateInstance(createType, Type.EmptyTypes);
        }

        public static object CreateInstance(Type createType, Type[] constructorParams)
        {
            Func<object> instanceCreator = Expression.Lambda<Func<object>>(
                Expression.New(createType.GetConstructor(constructorParams))
                ).Compile();
            return instanceCreator();
        }

    }
}
