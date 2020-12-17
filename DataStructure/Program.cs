using System;
using System.Collections.Generic;
using System.Diagnostics;
using DataStructure.MyDictionary;

class Program
{
    static void Main(string[] args)
    {
        int i = int.MaxValue;
        Console.WriteLine($"{i}의 두 배를 long으로 표현하면 {2 * (long)i} 인데 이것을 다시 int로 캐스팅하면 {(int)(2 * (long)i)}");
    }
}
