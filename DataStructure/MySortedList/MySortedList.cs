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
        #endregion
    }
}
