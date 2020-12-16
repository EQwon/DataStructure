using System;

namespace DataStructure
{
    public class MyDictionary<TKey, TValue>
    {
        private struct Entry
        {
            public int hashCode;    // 31비트 이하의 해시코드, 사용안하면 -1
            public int next;        // 엔트리의 번호, 마지막이면 -1
            public TKey key;
            public TValue value;
        }

        private int[] buckets;
        private Entry[] entries;
        private int count;
        private int freeList;
        private int freeCount;
        private IMyEqualityComparer<TKey> comparer;

        #region 생성자 (Constructor)
        public MyDictionary() : this(0, null) { }

        public MyDictionary(int capacity) : this(capacity, null) { }

        public MyDictionary(IMyEqualityComparer<TKey> comparer) : this(0, comparer) { }

        public MyDictionary(int capacity, IMyEqualityComparer<TKey> comparer)
        {
            if (capacity < 0) Console.WriteLine("Dictionary는 크기가 0 미만이 될 수 없습니다.");
            if (capacity > 0) Initialize(capacity);
            this.comparer = comparer ?? MyEqualityComparer<TKey>.Default;
        }
        #endregion

        #region 속성 (Property)
        public TValue this[TKey key]
        {
            get
            {
                int i = FindEntry(key);
                if (i >= 0) return entries[i].value;

                Console.WriteLine("해당하는 Key에 대한 값을 찾지 못했습니다.");
                return default;
            }
            set
            {
                Insert(key, value, false);
            }
        }

        public int Count => count - freeCount;
        #endregion

        #region 메소드 (Method)
        public void Add(TKey key, TValue value)
        {
            Insert(key, value, true);
        }

        public void Clear()
        {
            if (count > 0)
            {
                for (int i = 0; i < buckets.Length; i++) buckets[i] = -1;
                Array.Clear(entries, 0, entries.Length);
                count = 0;
                freeList = -1;
                freeCount = 0;
            }
        }

        public bool ContainsKey(TKey key)
        {
            return FindEntry(key) >= 0;
        }

        public bool ContainsValue(TValue value)
        {
            if (value == null)
            {
                foreach (Entry entry in entries)
                {
                    if (entry.hashCode >= 0 && entry.value == null)
                        return true;
                }
            }
            else
            {
                MyEqualityComparer<TValue> c = MyEqualityComparer<TValue>.Default;

                foreach (Entry entry in entries)
                {
                    if (entry.hashCode >=0 && c.Equals(value, entry.value))
                        return true;
                }
            }

            return false;
        }

        public bool Remove(TKey key)
        {
            if (key == null) Console.WriteLine("Key가 null입니다.");

            if (buckets != null)
            {
                int hashCode = comparer.GetHashCode(key) & 0x7FFFFFFF;
                int targetBucket = buckets[hashCode] % buckets.Length;
                int last = -1;

                for (int i = buckets[targetBucket]; i >= 0; last = i, i = entries[i].next)
                {
                    if (entries[i].hashCode == hashCode && comparer.Equals(entries[i].key, key))
                    {
                        if (last < 0)   // 이것이 bucket의 첫번째 였다면
                            buckets[targetBucket] = entries[i].next;
                        else            // 이전의 다른 entry가 존재했다면
                            entries[last].next = entries[i].next;

                        // 해당 entry를 초기화시킵니다.
                        entries[i].hashCode = -1;
                        entries[i].next = freeList;     // 기존에 빈공간이 있었다면 빈공간에 대한 정보를 연결합니다.
                        entries[i].key = default;
                        entries[i].value = default;

                        // 빈공간에 대한 정보를 수정합니다.
                        freeList = i;
                        freeCount++;

                        return true;
                    }
                }
            }

            return false;
        }
        #endregion

        private void Initialize(int capacity)
        {
            int size = HashHelper.GetPrime(capacity);
            buckets = new int[size];
            for (int i = 0; i < buckets.Length; i++) buckets[i] = -1;
            entries = new Entry[size];
            freeList = -1;
        }

        private int FindEntry(TKey key)
        {
            if (key == null) Console.WriteLine("입력된 Key가 null입니다.");

            if (buckets != null)
            {
                int hashCode = comparer.GetHashCode(key) & 0x7FFFFFFF;
                for (int i = buckets[hashCode % buckets.Length]; i >= 0; i = entries[i].next)
                {
                    if (entries[i].hashCode == hashCode && comparer.Equals(entries[i].key, key))
                        return i;
                }
            }

            return -1;
        }

        // add == True      새로운 pair를 추가
        // add == False     기존의 Key에 대응하는 value를 교체
        private void Insert(TKey key, TValue value, bool add)
        {
            if (key == null) Console.WriteLine("Key 값이 null입니다.");

            if (buckets == null) Initialize(0);
            int hashCode = comparer.GetHashCode(key) & 0x7FFFFFFF;
            int targetBucket = hashCode % buckets.Length;

            for (int i = buckets[targetBucket]; i >= 0; i = entries[i].next)
            {
                if (entries[i].hashCode == hashCode && comparer.Equals(entries[i].key, key))
                {
                    if (add) Console.WriteLine("중복된 Key 입니다.");

                    entries[i].value = value;
                    return;
                }
            }
            int index;
            if (freeCount > 0)
            {
                index = freeList;
                freeList = entries[index].next;
                freeCount--;
            }
            else
            {
                if (count == entries.Length)
                {
                    Resize();
                    targetBucket = hashCode % buckets.Length;
                }
                index = count;
                count++;
            }
            entries[index].hashCode = hashCode;
            entries[index].next = buckets[targetBucket];
            entries[index].key = key;
            entries[index].value = value;
            buckets[targetBucket] = index;
        }

        private void Resize()
        {
            Resize(HashHelper.ExpandPrime(count));
        }

        // 현재 Dictionary의 크기를 newSize 만큼으로 증가시킵니다.
        private void Resize(int newSize)
        {
            int[] newBuckets = new int[newSize];
            for (int i = 0; i < newBuckets.Length; i++) newBuckets[i] = -1;
            Entry[] newEntries = new Entry[newSize];
            Array.Copy(entries, 0, newEntries, 0, count);

            for (int i = 0; i < count; i++)
            {
                if (newEntries[i].hashCode >= 0)
                {
                    int targetBucket = newEntries[i].hashCode % newSize;
                    newEntries[i].next = newBuckets[targetBucket];
                    newBuckets[targetBucket] = i;
                }
            }

            entries = newEntries;
            buckets = newBuckets;
        }
    }

    public static class HashHelper
    {
        public static readonly int[] primes = {
            3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
            1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
            17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
            187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
            1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369};
        public const int MaxPrimeArrayLength = 0x7FEFFFFD;

        // 기존 소수보다 2배 초과의 소수를 찾기
        public static int ExpandPrime(int oldPrime)
        {
            int newPrime = 2 * oldPrime;

            // 단 int 범위의 최대 소수를 넘는지 체크 한 번 합니다.
            if ((uint)newPrime > MaxPrimeArrayLength && MaxPrimeArrayLength > oldPrime)
                return MaxPrimeArrayLength;

            return GetPrime(newPrime);
        }

        public static int GetPrime(int min)
        {
            if (min < 0) Console.WriteLine("0보다 작은 소수를 찾을 수 없습니다.");

            for (int i = 0; i < primes.Length; i++)
                if (primes[i] >= min) return primes[i];

            // 만약 미리 정의된 소수의 범위를 벗어난다면
            // 그냥 계산해야지 뭐...
            for (int i = (min | 1); i < Int32.MaxValue; i += 2)     // 여기서 min | 1 의 의미는 홀수로 만들기 위함
            {
                if (IsPrime(i)) return i;
            }

            return min;
        }

        public static bool IsPrime(int candidate)
        {
            if ((candidate & 1) != 0)    // 홀수냐고 물어보는 것
            {
                int limit = (int)Math.Sqrt(candidate);
                for (int divisor = 3; divisor <= limit; divisor += 2)
                {
                    if ((candidate % divisor) == 0) return false;
                }
                return true;
            }
            return (candidate == 2);
        }
    }

    // Equals       두 오브젝트가 같은지 확인할 때 사용
    // GetHashCode  오브젝트의 해쉬코드를 생성할 때 사용
    // MyDictionary 클래스에서 사용하기 위한 용도입니다.
    public interface IMyEqualityComparer<in T>
    {
        bool Equals(T x, T y);
        int GetHashCode(T obj);
    }

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