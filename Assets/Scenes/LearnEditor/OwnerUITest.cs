using System;
using UnityEngine;
using System.Collections;
 
public class OwnerUITest : MonoBehaviour
{
 
    public int IntVal;
 
    public float FlatVal;
 
    public string StrVal;
 
    public Type3 mType = new Type3();
 
    private void Start() {
    }
 
}
 
[Serializable]
public class Type3
{
    public int mInt;
 
    public int mInt2;
}
 