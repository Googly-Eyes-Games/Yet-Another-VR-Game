using UnityEngine;

public static class ObjectUtils
{
    public static T GetRandom<T>(this T[] array)
    {
        int randomIndex = Random.Range(0, array.Length);
        return array[randomIndex];
    }
}