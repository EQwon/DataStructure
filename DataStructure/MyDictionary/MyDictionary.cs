using System;


namespace DataStructure.MyDictionary
{
    public partial class MyDictionary<TKey, TValue>
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
}