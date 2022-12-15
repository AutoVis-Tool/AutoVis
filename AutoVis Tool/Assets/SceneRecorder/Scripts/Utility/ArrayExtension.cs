using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayExtension
{
    /// <summary>
    /// Method that finds the value closest to the target in an array
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static double ClosestTo(this IEnumerable<double> collection, double target)
    {
        var closest = double.MaxValue;
        var minDifference = double.MaxValue;
        foreach (var element in collection)
        {
            var difference = Math.Abs((double)element - target);
            if (minDifference > difference)
            {
                minDifference = (double)difference;
                closest = element;
            }
        }

        return closest;
    }
}
