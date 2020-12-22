using System;
using System.Collections.Generic;

namespace DataStructure.MySortedList
{
    public class MySortedList<TKey, TValue>
    {
        private TKey[] keys;
        private TValue[] values;
        private int _size;
        private IComparer<TKey> comparer;

        static TKey[] emptyKeys = new TKey[0];
        static TValue[] emptyValues = new TValue[0];

        private readonly int _defaultCapacity = 4;
        private readonly int MaxArrayLength = 0x7FEFFFFF;

        #region 생성자 (Constructor)
        public MySortedList()
        {
            keys = emptyKeys;
            values = emptyValues;
            _size = 0;
            comparer = Comparer<TKey>.Default;
        }

        public MySortedList(int capacity)
        {
            if (capacity < 0)
            {
                Console.WriteLine("SortedList는 크기가 0보다 작을 수 없습니다.");
                return;
            }

            keys = new TKey[capacity];
            values = new TValue[capacity];
            comparer = Comparer<TKey>.Default;
        }

        public MySortedList(IComparer<TKey> comparer) : this()
        {
            if (comparer != null) this.comparer = comparer;
        }

        public MySortedList(int capacity, IComparer<TKey> comparer) : this(comparer)
        {
            Capacity = capacity;
        }
        #endregion

        #region 속성 (Property)
        // SortedList의 최대 용량을 가져오거나 설정합니다.
        public int Capacity
        {
            get => keys.Length;
            set
            {
                if (value != keys.Length)
                {
                    if (value < _size)
                    {
                        Console.WriteLine("기존의 크기보다 작아질 수는 없습니다.");
                        return;
                    }
                    if (value > 0)
                    {
                        TKey[] newKeys = new TKey[value];
                        TValue[] newValues = new TValue[value];
                        if (_size > 0)
                        {
                            Array.Copy(keys, 0, newKeys, 0, _size);
                            Array.Copy(values, 0, newValues, 0, _size);
                        }
                        keys = newKeys;
                        values = newValues;
                    }
                    else
                    {
                        keys = emptyKeys;
                        values = emptyValues;
                    }
                }
            }
        }

        // SortedList에 포함된 요소의 수를 가져옵니다.
        public int Count => _size;

        public TValue this[TKey key]
        {
            get
            {
                int i = IndexOfKey(key);
                if (i >= 0)
                    return values[i];

                Console.WriteLine(key + "에 해당하는 값을 찾을 수 없습니다.");
                return default(TValue);
            }

            set
            {
                int i = SortHelper<TKey>.BinarySearch(keys, 0, _size, key, comparer);
                if (i >= 0)
                    values[i] = value;
                else
                    Insert(~i, key, value);
            }
        }
        #endregion

        #region 메소드 (Method)
        public void Add(TKey key, TValue value)
        {
            if (key == null)
            {
                Console.WriteLine("Key가 null입니다!");
                return;
            }

            // SortHelper를 통해 key에 해당하는 위치를 찾습니다.
            // 만약 양수라면 key가 이미 존재하는, 중복된 key라는 뜻이고
            // 음수라면 NOT 연산을 통해 삽입되어야하는 위치를 얻을 수 있습니다.
            int index = SortHelper<TKey>.BinarySearch(keys, 0, _size, key, comparer);
            if (index >= 0)
            {
                Console.WriteLine("중복된 key를 추가하려고 하고 있습니다.");
                return;
            }

            Insert(~index, key, value);
        }

        public void Clear()
        {
            Array.Clear(keys, 0, _size);
            Array.Clear(values, 0, _size);
            _size = 0;
        }
        #endregion

        // index의 위치에 key와 value를 삽입합니다.
        private void Insert(int index, TKey key, TValue value)
        {
            if (_size == keys.Length) EnsureCapacity(_size + 1);
            if (index < _size)
            {
                Array.Copy(keys, index, keys, index + 1, _size - index);
                Array.Copy(values, index, values, index + 1, _size - index);
            }

            keys[index] = key;
            values[index] = value;
            _size++;
        }

        // SortedList가 min 이상의 용량을 가지는 것을 보장합니다.
        private void EnsureCapacity(int min)
        {
            int newCapacity = keys.Length == 0 ? _defaultCapacity : keys.Length * 2;
            if ((uint)newCapacity > MaxArrayLength) newCapacity = MaxArrayLength;
            if (newCapacity < min) newCapacity = min;
            Capacity = newCapacity;
        }

        private int IndexOfKey(TKey key)
        {
            if (key == null)
            {
                Console.WriteLine("keys는 null이 될 수 없습니다.");
                return -1;
            }
            int i = SortHelper<TKey>.BinarySearch(keys, 0, _size, key, comparer);
            return i >= 0 ? i : -1;
        }
    }
}
