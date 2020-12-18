using System;
using System.Collections.Generic;

namespace DataStructure.MyQueue
{
    public partial class MyQueue<T>
    {
        private T[] _array;
        private int _head;
        private int _tail;
        private int _size;

        private const int growFactor = 2;
        private const int minimumGrow = 4;

        #region 생성자 (Constructor)
        public MyQueue()
        {
            _array = new T[0];
        }

        public MyQueue(int capacity)
        {
            if (capacity < 0) Console.WriteLine("Queue의 크기는 0보다 작을 수 없습니다.");

            _array = new T[capacity];
            _head = 0;
            _tail = 0;
            _size = 0;
        }
        #endregion

        #region 속성 (Property)
        public int Count => _size;
        #endregion

        #region 메소드 (Method)
        // 쓸떼없이 메모리와 연산을 낭비를 하지 않기 위해 원소가 있는 곳만 청소
        public void Clear()
        {
            if (_head < _tail)
                Array.Clear(_array, _head, _tail);
            else
            {
                Array.Clear(_array, _head, _array.Length - _head);
                Array.Clear(_array, 0, _tail);
            }

            _head = 0;
            _tail = 0;
            _size = 0;
        }

        public bool Contains(T item)
        {
            int index = _head;
            int count = _size;

            EqualityComparer<T> c = EqualityComparer<T>.Default;
            while (count > 0)
            {
                if (_array[index] != null && c.Equals(item, _array[index]))
                    return true;
                index = (index + 1) % _array.Length;
                count--;
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                Console.WriteLine("array가 비었습니다!");
                return;
            }

            if (arrayIndex < 0 || arrayIndex >= array.Length)
            {
                Console.WriteLine("시작 위치가 잘못되었습니다.");
                return;
            }

            int arrayLen = array.Length;
            if (arrayLen - arrayIndex < _size)
            {
                Console.WriteLine("입력 받은 array의 길이가 모자랍니다.");
                return;
            }

            // 복사하려는 queue가 비어있다면 그냥 바로 return 합니다.
            int numToCopy = _size;
            if (numToCopy == 0) return;

            int firstPart = (_array.Length - _head < numToCopy) ? _array.Length - _head : numToCopy;
            Array.Copy(_array, _head, array, arrayIndex, firstPart);
            numToCopy -= firstPart;
            if (numToCopy > 0)
            {
                Array.Copy(_array, 0, array, arrayIndex + _array.Length - _head, numToCopy);
            }
        }

        public void Enqueue(T item)
        {
            if (_size == _array.Length)
            {
                int newCapacity = _size * growFactor;
                if (newCapacity < _size + minimumGrow) newCapacity = _size + minimumGrow;

                SetCapactiy(newCapacity);
            }

            _array[_tail] = item;
            _tail = (_tail + 1) % _array.Length;
            _size++;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public T Dequeue()
        {
            if (_size == 0)
            {
                Console.WriteLine("Queue의 크기가 0이라서 뺄 수 없어요!");
                return default(T);
            }

            T removed = _array[_head];
            _array[_head] = default(T);
            _head = (_head + 1) % _array.Length;
            _size--;

            return removed;
        }

        public T Peek()
        {
            if (_size == 0)
            {
                Console.WriteLine("Queue의 크기가 0이라서 볼 수 없어요!");
                return default(T);
            }

            return _array[_head];
        }

        public T[] ToArray()
        {
            T[] arr = new T[_size];
            if (_size == 0)
                return arr;

            if (_head < _tail)
                Array.Copy(_array, _head, arr, 0, _size);
            else
            {
                Array.Copy(_array, _head, arr, 0, _array.Length - _head);
                Array.Copy(_array, 0, arr, _array.Length - _head, _tail);
            }

            return arr;
        }
        #endregion

        private void SetCapactiy(int capacity)
        {
            T[] newArray = new T[capacity];
            
            if (_size > 0)
            {
                if (_head < _tail)
                {
                    Array.Copy(_array, _head, newArray, 0, _size);
                }
                else
                {
                    Array.Copy(_array, _head, newArray, 0, _array.Length - _head);
                    Array.Copy(_array, 0, newArray, _array.Length - _head, _tail);
                }
            }

            _array = newArray;
            _head = 0;
            _tail = (_size == capacity) ? 0 : _size; // ??
        }

        internal T GetElement(int i)
        {
            return _array[(_head + i) % _array.Length];
        }
    }
}
