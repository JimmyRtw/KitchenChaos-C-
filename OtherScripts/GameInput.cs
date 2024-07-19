
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";
    public static GameInput Instance {get;private set;}
    public event EventHandler OnInteractAction; 
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPause;
    public event EventHandler OnBindingRebind;
    private PlayerInputActions playerInputActions;

    public enum Bindings
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        Interact_Alternate,
        Pause,
    }
    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();

        if(PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;  

        playerInputActions.Dispose();      
    }
    private void Pause_performed(InputAction.CallbackContext context)
    {
        OnPause ? .Invoke(this,EventArgs.Empty);
    }

    private void InteractAlternate_performed(InputAction.CallbackContext context)
    {
        OnInteractAlternateAction ? .Invoke(this,EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction ? .Invoke(this,EventArgs.Empty);
    }
    public Vector2 GetMovementVectorNormalized()
    {
        // //taking input from keyboard
        // bool inputVarUp = Input.GetKey(KeyCode.W);
        // bool inputVarDown = Input.GetKey(KeyCode.S);
        // bool inputVarLeft = Input.GetKey(KeyCode.A);
        // bool inputVarRight = Input.GetKey(KeyCode.D);

        // Vector2 inputVector = new Vector2(0,0);

        // //setting up inputvector
        // if(inputVarUp) inputVector.y += 1;
        // if(inputVarDown) inputVector.y -= 1;
        // if(inputVarLeft) inputVector.x -= 1;
        // if(inputVarRight) inputVector.x += 1;

        // normalizing so each movement have constant speed

        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }    

    public string GetBindingText(Bindings bindings)
    {
        switch(bindings)
        {
            default :

            case Bindings.Move_Up : return playerInputActions.Player.Move.bindings[1].ToDisplayString();   
            case Bindings.Move_Down : return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Bindings.Move_Left : return playerInputActions.Player.Move.bindings[3].ToDisplayString();     
            case Bindings.Move_Right : return playerInputActions.Player.Move.bindings[4].ToDisplayString();        
            case Bindings.Interact : return playerInputActions.Player.Interact.bindings[0].ToDisplayString();       
            case Bindings.Interact_Alternate : return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();    
            case Bindings.Pause : return playerInputActions.Player.Pause.bindings[0].ToDisplayString();            
        }
    }

    public void RebindBinding(Bindings bindings,Action onActionRebound)
    {
        playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch(bindings)
        {
            default:

            case Bindings.Move_Up : inputAction = playerInputActions.Player.Move; bindingIndex = 1; break;
            case Bindings.Move_Down : inputAction = playerInputActions.Player.Move; bindingIndex = 2; break;
            case Bindings.Move_Left : inputAction = playerInputActions.Player.Move; bindingIndex = 3; break;
            case Bindings.Move_Right : inputAction = playerInputActions.Player.Move; bindingIndex = 4; break;
            case Bindings.Interact : inputAction = playerInputActions.Player.Interact; bindingIndex = 0; break;
            case Bindings.Interact_Alternate : inputAction = playerInputActions.Player.InteractAlternate; bindingIndex = 0; break;
            case Bindings.Pause : inputAction = playerInputActions.Player.Pause; bindingIndex = 0; break;
        }

          inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback=>
          {
            callback.Dispose();
            playerInputActions.Player.Enable();
            onActionRebound();
            PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();

            OnBindingRebind ? .Invoke(this,EventArgs.Empty);
        }).Start();
    }
};
