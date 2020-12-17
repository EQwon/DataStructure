using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructure.MyDictionary
{
    // Equals       두 오브젝트가 같은지 확인할 때 사용
    // GetHashCode  오브젝트의 해쉬코드를 생성할 때 사용
    // MyDictionary 클래스에서 사용하기 위한 용도입니다.
    public interface IMyEqualityComparer<in T>
    {
        bool Equals(T x, T y);
        int GetHashCode(T obj);
    }
}
