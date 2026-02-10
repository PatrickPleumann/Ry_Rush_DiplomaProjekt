using UnityEngine;

public class Utility
{
    /// <summary>
    /// Floors a float to two digits.
    /// </summary>
    /// <param name="_value">The value which gets floored</param>
    /// <returns></returns>
    public static float FloorFloat_TwoDigits(float _value)
    {
        if (_value > 0)
        {
            var temp = (Mathf.FloorToInt(_value * 100f)) * 0.01f;
            return temp;
        }
        return 0f;
    } 
}
