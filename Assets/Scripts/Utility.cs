using UnityEngine;

public class Utility
{
    /// <summary>
    /// Floors a float to two digits.
    /// </summary>
    /// <param name="_value">The value which gets floored</param>
    /// <returns></returns>
    public static float FloorFloatToTwoDigits(float _value)
    {
        if (_value > 0)
        {
            var temp = _value * 100;
            temp = Mathf.FloorToInt(temp);
            temp = temp * 0.01f;
            //var temp = (Mathf.FloorToInt(_value * 100f)) * 0.01f;
            Debug.Log("Value from parameter: " + _value);
            Debug.Log("Return value " + temp);
            return temp;
        }
        return 0f;
    }
}
