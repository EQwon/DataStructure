using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructure.MyQueue
{
    public partial class MyQueue<T>
    {
        public struct Enumerator : IEnumerator<T>
        {
            private MyQueue<T> _q;
            private int _index;         // -1이면 아직 시작안함, -2이면 끝나거나 dispose
            private T _currentElement;

            internal Enumerator(MyQueue<T> q)
            {
                _q = q;
                _index = -1;
                _currentElement = default(T);
            }

            public T Current
            {
                get
                {
                    if (_index < 0)
                    {
                        if (_index == -1)
                        {
                            Console.WriteLine("Enumerator가 아직 시작되지 않았습니다.");
                            return default(T);
                        }
                        else
                        {
                            Console.WriteLine("Enumerator가 이미 종료되었습니다.");
                            return default(T);
                        }
                    }
                    return _currentElement;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    if (_index < 0)
                    {
                        if (_index == -1)
                        {
                            Console.WriteLine("Enumerator가 아직 시작되지 않았습니다.");
                            return default(T);
                        }
                        else
                        {
                            Console.WriteLine("Enumerator가 이미 종료되었습니다.");
                            return default(T);
                        }
                    }
                    return _currentElement;
                }
            }

            public void Dispose()
            {
                _index = -2;
                _currentElement = default(T);
            }

            public bool MoveNext()
            {
                if (_index == -2)
                    return false;

                _index++;
                if (_index == _q.Count)
                {
                    Dispose();
                    return false;
                }

                _currentElement = _q.GetElement(_index);
                return true;
            }

            public void Reset()
            {
                _index = -1;
                _currentElement = default(T);
            }
        }
    }
}
