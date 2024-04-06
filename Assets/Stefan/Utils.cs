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

    public static int ClosestNumberInRange(int min, int max, int target)
    {
        // Ensure that the minimum value is less than or equal to the maximum value
        if (min > max)
            (max, min) = (min, max);

        // Handle the case when the target is outside the range
        if (target <= min)
            return min;
        else if (target >= max)
            return max;


        return target;
    }
}
