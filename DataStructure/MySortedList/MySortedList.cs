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
        #endregion
    }
}
