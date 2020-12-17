using System;

namespace DataStructure.MyQueue
{
    public class MyQueue<T>
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
    }
}
