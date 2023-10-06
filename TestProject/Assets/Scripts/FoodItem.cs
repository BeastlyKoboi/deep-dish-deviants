using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public enum FoodId
{
    dough = 0,
    cheese = 1,
    sauce = 2,
    plate = 3,
}

public enum CookState
{
    raw = 0,
    cooked = 1,
    burnt = 2
}
public class FoodItem : MonoBehaviour
{
    public FoodId id;
    public CookState foodState;
}
