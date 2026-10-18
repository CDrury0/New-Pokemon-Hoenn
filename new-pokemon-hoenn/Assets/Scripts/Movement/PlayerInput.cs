using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static Transform playerTransform;
    /// <summary>
    /// This may be set independently to account for menu toggling
    /// </summary>
    public static bool allowMovementInput = true;
    private static bool _allowMenuToggle = true;
    /// <summary>
    /// Setting this false automatically sets allowMovementInput to false
    /// </summary>
    public static bool AllowMenuToggle {
        get { return _allowMenuToggle; }
        set { if(value == false){
                allowMovementInput = false;
            }
            _allowMenuToggle = value;
        }
    }
    public static int StepCount;
    protected static List<Tuple<int, Func<bool>>> AfterStep;
    public GameObject interactPointPrefab;
    public static Transform followPoint;
    public MenuAnimation menuAnimation;
    public LayerMask interruptsMovement;
    public Animator animator;
    public PlayerMovementHandler movementHandler;
    public float CurrentMoveSpeed { get; set; }
    public bool IsSprinting { get; set; }
    [SerializeField] private float movementInputDelaySeconds;
    public static Vector3 Direction { get; set; }
    public static Vector3 PlayerHeightOffset {get => new(0, -0.5f, 0);}
    public static MovementMode MovementMode { get; private set; }

    public static void UpdateMovementMode(MovementMode newMode) {

    }

    public static void SetPlayerPosition(Vector3 newPosition) {
        followPoint.position = newPosition - PlayerHeightOffset;
        playerTransform.position = newPosition - PlayerHeightOffset;
    }

    public Collider2D GetColliderAtNextStep() {
        return Physics2D.OverlapCircle(followPoint.position + PlayerHeightOffset + Direction, 0.4f, interruptsMovement);
    }

    void Update() {
        GetPlayerInput();
        transform.position = Vector3.MoveTowards(transform.position, followPoint.position, CurrentMoveSpeed * Time.deltaTime);
    }

    private void GetPlayerInput() {
        if (Vector3.Distance(transform.position, followPoint.position) != 0){
            return;
        }
        if (!AllowMenuToggle){
            return;
        }
        if (GetMenuInput() || !allowMovementInput) {
            return;
        }
        if (GetInteractInput()) {
            return;
        }
        IsSprinting = Input.GetKey(KeyCode.LeftShift);
        GetMovementInput();
    }
            

    private bool GetInteractInput() {
        if(Input.GetKeyDown(KeyCode.E)){
            ActivateInteractPoint();
            return true;
        }
        return false;
    }

    private bool GetMenuInput() {
        if(Input.GetKeyDown(KeyCode.Q)){
            allowMovementInput = !allowMovementInput;
            menuAnimation.ToggleMenu();
            return true;
        }
        return false;
    }

    private void GetMovementInput() {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if(horizontal == 0 && vertical == 0){
            movementHandler.AnimateMovement(this, Direction);
            return;
        }
        Vector3 newDirection = horizontal != 0 ? new(horizontal, 0, 0) : new(0, vertical, 0);
        movementHandler.HandleMovement(this, newDirection);        
    }

    public static void RegisterAfterStep(Func<bool> func, int priority) {
        for(int i = 0; i < AfterStep.Count; i++){
            if(priority < AfterStep[i].Item1){
                AfterStep.Insert(i, new(priority, func));
                return;
            }
        }

        AfterStep.Add(new(priority, func));
    }

    public static void UnregisterAfterStep(Func<bool> func) {
        AfterStep.Remove(AfterStep.Find((entry) => entry.Item2 == func));
    }

    public void DoAfterStep() {
        var temp = new List<Tuple<int, Func<bool>>>(AfterStep);
        foreach(var val in temp)
            if(!val.Item2())
                break;
    }

    public void ActivateInteractPoint() {
        StartCoroutine(ActivateInteractPointCoroutine());
    }

    protected IEnumerator ActivateInteractPointCoroutine() {
        GameObject pointObj = Instantiate(interactPointPrefab, transform.position + PlayerHeightOffset + Direction, Quaternion.identity, transform);
        yield return new WaitForFixedUpdate();
        Destroy(pointObj);
    }

    public void DelayMovementInput() {
        StartCoroutine(DelayMovementInputCoroutine());
    }

    protected IEnumerator DelayMovementInputCoroutine() {
        movementHandler.AnimateMovement(this, Direction);
        allowMovementInput = false;
        yield return new WaitForSeconds(movementInputDelaySeconds);
        allowMovementInput = true;
    }

    void Awake(){
        playerTransform = transform;
        followPoint = GameObject.FindGameObjectWithTag("MovePoint").transform;
        AfterStep = new();
    }

    void Start(){
        if(SaveManager.LoadedSave?.currentPosition != null){
            transform.position = SaveManager.LoadedSave.currentPosition.value;
        }
        
        followPoint.position = transform.position;
        Direction = Vector3.down;
    }
}
