using System;
using System.Collections.Generic;
using DataStructure.MyDictionary;

class Program
{
    static void Main(string[] args)
    {
        MyDictionary<string, int> myDict = new MyDictionary<string, int>();
        Dictionary<string, int> dict = new Dictionary<string, int>();

        foreach (KeyValuePair<string, int> pair in dict)
        { }

        foreach (KeyValuePair<string, int> pair in myDict)
        { }
    }
}
