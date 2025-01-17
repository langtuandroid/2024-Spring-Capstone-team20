using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Util 
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static T ExtractValue<T>(string input, string pattern) where T : struct, IConvertible
    {
        var match = Regex.Match(input, pattern);
        if (match.Success)
        {
            string value = match.Groups[1].Value;

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (FormatException) 
            { 
                return default(T);//if error, returns 0
            }
        }
        return default;
    }
}
