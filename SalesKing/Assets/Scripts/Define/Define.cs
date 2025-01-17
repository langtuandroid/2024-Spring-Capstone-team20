using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    public enum SceneMode
    {
        Start,
        CityMap, 
        OfficeMap
    }

    public enum Emotion
    {
        Neutral,
        SlightlyPositive,
        Positive,
        VeryPositive,
        SlightlyNegative,
        Negative,
        VeryNegative
    }
}
