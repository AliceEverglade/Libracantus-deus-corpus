using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IntSO", menuName = "SriptableObjects/Int")]
public class IntSO : ScriptableObject
{
    [SerializeField] private bool isConstant;
    [SerializeField] private int constantValue;
    public IntSO variable;

    public int Value
    {
        get { return isConstant ? constantValue : variable.Value; }
    }
}
