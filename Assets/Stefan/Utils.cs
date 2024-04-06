using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils 
{
    public static T GetRandomItem<T>(this T[] arr)
    {
        return arr[UnityEngine.Random.Range(0, arr.Length)];
    }
    public static T GetRandomItem<T>(this List<T> arr)
    {
        return arr[UnityEngine.Random.Range(0, arr.Count)];
    }

    public static T GetRandomItemFromArray<T>(T[] arr)
    {
        return arr[UnityEngine.Random.Range(0, arr.Length)];
    }
}
