using System;
using System.Collections.Generic;
using System.Diagnostics;
using DataStructure.MyDictionary;

class Program
{
    static void Main(string[] args)
    {
        MyDictionary<string, int> myDict = new MyDictionary<string, int>();
        Dictionary<string, int> dict = new Dictionary<string, int>();

        Stopwatch watch = new Stopwatch();

        watch.Start();
        for (int i = 0; i < 100000; i++)
        {
            myDict[i + "번째"] = i;
        }
        watch.Stop();
        Console.WriteLine("MyDictionary에 100000개 추가하기 : " + watch.ElapsedMilliseconds.ToString());

        watch.Reset();

        watch.Start();
        for (int i = 0; i < 100000; i++)
        {
            dict[i + "번째"] = i;
        }
        watch.Stop();
        Console.WriteLine("Dictionary에 100000개 추가하기 : " + watch.ElapsedMilliseconds.ToString());
    }
}
