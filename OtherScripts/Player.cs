

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.OpenVR;
using UnityEditor;
using UnityEngine;
using System;

public class Player : MonoBehaviour,IKitchenObjectParent
{
    public event EventHandler OnPickUp;
    public static Player Instance { get ; private set ;}

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7.0f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float interactionDist = 2f;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private BaseCounter selectedCounter = null;
    private bool isWalking = false;
    private Vector3 lastInteractionDir;
    private KitchenObject kitchenObject;

    private void Awake()
    {
        if(Instance!=null)
        {
            Debug.Log("There are more than one Instance of player");
        }

        Instance = this;       
    }
    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnAlternateInteractAction;
    }

    private void GameInput_OnAlternateInteractAction(object sender, EventArgs e)
    {
        if(!KitchenGameManager.Instance.IsGamePlaying()) return;
        // if we have any active selected Counter then we can interact with it
        if(selectedCounter) selectedCounter.InteractAlternate(this);        
    }

    private void GameInput_OnInteractAction(object sender,System.EventArgs e)
    {
        if(!KitchenGameManager.Instance.IsGamePlaying()) return;
        // if we have any active selected Counter then we can interact with it
        if(selectedCounter) selectedCounter.Interact(this);
    }
    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x,0f,inputVector.y);

        isWalking = moveDir != Vector3.zero;

        // setting last movement direction so we could check for current interaction
        if(isWalking) lastInteractionDir = moveDir;

        float moveDist = Time.deltaTime*moveSpeed;
        float playerHeight = 2f;
        float playerRadius = 0.7f;

        // casting capsulecast with first point as transform position and second point as height offseted transform position in moveDir
        bool canMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up*playerHeight,playerRadius,moveDir,moveDist);

        if(!canMove)
        {
            Vector3 moveDirx = new Vector3(moveDir.x,0f,0f);
            Vector3 moveDiry = new Vector3(0f,0f,moveDir.z);

            bool canMoveX = moveDir.x !=0 && !Physics.CapsuleCast(transform.position,transform.position + Vector3.up*playerHeight,playerRadius,moveDirx,moveDist);
            bool canMovey = moveDir.y !=0 && !Physics.CapsuleCast(transform.position,transform.position + Vector3.up*playerHeight,playerRadius,moveDiry,moveDist);

            // checking can move in x direction
            if(canMoveX)
            {
                moveDir = moveDirx.normalized;
                canMove = true;
            }

            // checking can move in y direction
            if(canMovey)
            {
                moveDir = moveDiry.normalized;
                canMove = true;
            }
        }


        if(canMove)
        {
            // setting position of transform with given movespeed;
            transform.position += moveDir*moveDist;
        }

        // setting rotation of transform with given inputdirection
        // lerp : for a position based smoothness
        // slerp : for a rotation based smoothness
        transform.forward = Vector3.Slerp(transform.forward,moveDir,Time.deltaTime*rotateSpeed);
    }

    private void HandleInteractions()
    {
        bool selecetedCounterTempVar = false;
        // if ray hit something that means we hit something is function returns true
        bool isInteracting = Physics.Raycast(transform.position,lastInteractionDir,out RaycastHit raycastHit,interactionDist,countersLayerMask);

        if(isInteracting)
        {
            bool isInteractingClearCounter = raycastHit.transform.TryGetComponent(out BaseCounter baseCounter);

            if(isInteractingClearCounter)
            {
                // assigning tempvar a true value so we have an idea of selecting counter and not set it to NULL
                selecetedCounterTempVar = true;
                SetSelectedCounter(baseCounter);
            }
        }

        // setting selected Counter to null because no interaction has been done
        if(!selecetedCounterTempVar) SetSelectedCounter(null);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if(kitchenObject)
        {
            OnPickUp ? .Invoke(this,EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return this.kitchenObject;
    }

    public void ClearKitchenObject()
    {
        this.kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return this.kitchenObject != null;
    }   
}

