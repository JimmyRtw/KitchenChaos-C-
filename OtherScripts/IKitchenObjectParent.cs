using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEngine;
#endif

public interface IKitchenObjectParent
{
    #if UNITY_EDITOR
    public Transform GetKitchenObjectFollowTransform();
    public void SetKitchenObject(KitchenObject kitchenObject);
    public KitchenObject GetKitchenObject();
    public void ClearKitchenObject();
    public bool HasKitchenObject();
    #endif
}
