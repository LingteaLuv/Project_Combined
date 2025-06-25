using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    public int ItemId;
    public int ResultItemId;
    public int ResultQuantity;
    public int MaterialItemId1;
    public int MaterialItemQuantity1;
    public int MaterialItemId2;
    public int MaterialItemQuantity2;
    public int MaterialItemId3;
    public int MaterialItemQuantity3;
    public int MaterialItemId4;
    public int MaterialItemQuantity4;

    public void Init(int[] array)
    {
        ItemId = array[0];
        ResultItemId = array[1];
        ResultQuantity = array[2];
        MaterialItemId1 = array[3];
        MaterialItemQuantity1 = array[4];
        MaterialItemId2 = array[5];
        MaterialItemQuantity2 = array[6];
        MaterialItemId3 = array[7];
        MaterialItemQuantity3 = array[8];
        MaterialItemId4 = array[9];
        MaterialItemQuantity4 = array[10];
    }
}
