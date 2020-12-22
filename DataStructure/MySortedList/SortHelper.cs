using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructure.MySortedList
{
    internal class SortHelper<T>
    {
        // array에서 index 부터 length 길이만큼 중에 value가 존재하는 위치를 반환
        // 만약 없다면 음수가 반환된다. 다만 NOT 연산자를 통해 반환되었으므로
        // 반환값에 다시 NOT 연산을 하면 value가 있어야 하는 위치를 얻을 수 있다.
        public static int BinarySearch(T[] array, int index, int length, T value, IComparer<T> comparer)
        {
            int lo = index;
            int hi = index + length - 1;
            
            while(lo <= hi)
            {
                int mid = lo + ((hi - lo) >> 1);
                int order = comparer.Compare(array[mid], value);

                if (order == 0) return mid;
                if (order < 0)
                    lo = mid + 1;
                else
                    hi = mid - 1;
            }

            return ~lo;
        }
    }
}
