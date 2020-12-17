using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructure.MyDictionary
{
    public abstract class MyEqualityComparer<T> : IMyEqualityComparer<T>
    {
        static readonly MyEqualityComparer<T> defaultComparer = CreateComparer();

        public static MyEqualityComparer<T> Default
        {
            get
            {
                return defaultComparer;
            }
        }

        private static MyEqualityComparer<T> CreateComparer()
        {
            return (MyEqualityComparer<T>)(object)(new MyObjectEqualityComparer<T>());
        }

        public abstract bool Equals(T x, T y);

        public abstract int GetHashCode(T obj);
    }

    internal class MyObjectEqualityComparer<T> : MyEqualityComparer<T>
    {
        public override bool Equals(T x, T y)
        {
            if (x != null)
            {
                if (y != null) return x.Equals(y);
                return false;
            }
            if (y != null) return false;
            return true;
        }

        public override int GetHashCode(T obj)
        {
            if (obj == null) return 0;
            return obj.GetHashCode();
        }
    }
}
