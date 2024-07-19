using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject
{
    public ScriptableObject input;
    public ScriptableObject output;

    public int CuttingProgressMax = 0;
}
