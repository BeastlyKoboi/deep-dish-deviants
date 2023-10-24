using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;

public enum FoodId
{
    dough = 0,
    cheese = 1,
    sauce = 2,
    plate = 3,
    onion = 4,
    mushroom = 5,
    olive = 6,
    pepper = 7, 
    pepperoni = 8,
    beef = 9,
    bacon = 10,
    pineapple = 11

}

public enum CookState
{
    raw = 0,
    cooked = 1,
    burnt = 2
}

// state for ingredients that need to be cut
public enum CutState
{
    na = 0, // not applicable. applied if topping is not meant to be cut
    uncut = 1,
    cut = 2
}

// state for ingredients that need to be kneaded
public enum KneadState
{
    na = 0, // not applicable. applied if topping is not meant to be kneaded
    unkneaded = 1,
    kneaded = 2
}

public class FoodItem : MonoBehaviour
{
    public FoodId id;
    public CookState foodState;
    public CutState cutState;
    public KneadState kneadState;

}
