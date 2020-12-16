using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructure.MyDictionary
{
    public partial class MyDictionary<TKey, TValue>
    {
        public IEnumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private MyDictionary<TKey, TValue> dictionary;
            private KeyValuePair<TKey, TValue> current;
            private int index;

            internal Enumerator(MyDictionary<TKey, TValue> dictionary)
            {
                this.dictionary = dictionary;
                current = new KeyValuePair<TKey, TValue>();
                index = 0;
            }

            public KeyValuePair<TKey, TValue> Current => current;

            object IEnumerator.Current
            {
                get
                {
                    if (index == 0 || index == dictionary.count + 1)
                    {
                        Console.WriteLine("Enumerator가 진행될 수 없습니다.");
                    }

                    return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
                }
            }

            public void Dispose() { }

            public bool MoveNext()
            {
                while ((uint)index < (uint)dictionary.count)
                {
                    if (dictionary.entries[index].hashCode >= 0)
                    {
                        current = new KeyValuePair<TKey, TValue>(dictionary.entries[index].key, dictionary.entries[index].value);
                        index++;
                        return true;
                    }
                    index++;
                }

                index = dictionary.count + 1;
                current = new KeyValuePair<TKey, TValue>();
                return false;
            }

            public void Reset()
            {
                index = 0;
                current = new KeyValuePair<TKey, TValue>();
            }
        }
    }
}
