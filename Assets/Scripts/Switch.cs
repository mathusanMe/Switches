using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Switch : MonoBehaviour
{
    public InputMaster controls;
    //public Transition transition;

    public GameObject gameManager;
    
    public GameObject[] neighborSwitches = new GameObject[4];

    private GameManager gameManagerScript;  
    
    [SerializeField]
    private Sprite switchSelectedStateOn;
    [SerializeField]
    private Sprite switchSelectedStateOff;
    [SerializeField]
    private Sprite switchUnSelectedStateOn;
    [SerializeField]
    private Sprite switchUnSelectedStateOff;

    [SerializeField]
    private int[] coordinates = new int[2];
    
    private SpriteRenderer spriteRenderer;
    private Animator switchAnim;

    private bool hovering = false;
  
    public State switchState;

    public enum State {On, Off}

    void Awake() {

        controls = new InputMaster();
        controls.UI.MouseLeftClick.performed += _ => MouseLeftClickFunc();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        switchAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hovering && !(switchAnim.GetCurrentAnimatorStateInfo(0).IsTag("0") || switchAnim.GetCurrentAnimatorStateInfo(0).IsTag("1"))) {

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit) {

                if (hit.transform.name == gameObject.name) {
                    if (switchState == State.On) {
                        
                        switchAnim.SetBool("isHovered_On", true);

                    } else if (switchState == State.Off) {

                        switchAnim.SetBool("isHovered_Off", true);
                    }
                }
            }
        } else if (!hovering && !(switchAnim.GetCurrentAnimatorStateInfo(0).IsTag("0") || switchAnim.GetCurrentAnimatorStateInfo(0).IsTag("1"))) {

            if (switchState == State.On) {

                switchAnim.SetBool("isHovered_On", false);            

            } else if (switchState == State.Off) {

                switchAnim.SetBool("isHovered_Off", false);
            }
        }


    }

    void MouseLeftClickFunc() {

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit) {

            if (hit.transform.name == gameObject.name) {

                if (switchState == State.On && !(switchAnim.GetCurrentAnimatorStateInfo(0).IsTag("0") || switchAnim.GetCurrentAnimatorStateInfo(0).IsTag("1"))) {

                    switchAnim.SetBool("isIdle_On", false);
                    switchState = State.Off;

                    foreach (GameObject neighborSwitch in neighborSwitches){
                        if (neighborSwitch != null) {
                            neighborSwitch.GetComponent<Switch>().changeState(neighborSwitch);
                        }
                    }

                    gameManagerScript.addToHistory((coordinates[0], coordinates[1]));

                } else if (switchState == State.Off && !(switchAnim.GetCurrentAnimatorStateInfo(0).IsTag("0") || switchAnim.GetCurrentAnimatorStateInfo(0).IsTag("1"))) {

                    switchAnim.SetBool("isIdle_Off", false);
                    switchState = State.On;

                    foreach (GameObject neighborSwitch in neighborSwitches){
                        if (neighborSwitch != null) {
                            neighborSwitch.GetComponent<Switch>().changeState(neighborSwitch);
                        }
                    }

                    gameManagerScript.addToHistory((coordinates[0], coordinates[1]));
                }
            }
        }
    }

    void goToIdle() {
        if (switchState == State.On) {

            switchAnim.SetBool("isIdle_On", true);
            
        } else if (switchState == State.Off) {

            switchAnim.SetBool("isIdle_Off", true);
        }
    }

    public void changeState(GameObject neighborSwitch) {
        Animator neighborSwitchAnim = neighborSwitch.GetComponent<Animator>();
        Switch neighborSwitchScript = neighborSwitch.GetComponent<Switch>();

        if (neighborSwitchScript.switchState == State.On && !(neighborSwitchAnim.GetCurrentAnimatorStateInfo(0).IsTag("0") || neighborSwitchAnim.GetCurrentAnimatorStateInfo(0).IsTag("1"))) {

            neighborSwitchAnim.SetBool("isIdle_On", false);
            neighborSwitchScript.switchState = State.Off;

        } else if (neighborSwitchScript.switchState == State.Off && !(neighborSwitchAnim.GetCurrentAnimatorStateInfo(0).IsTag("0") || neighborSwitchAnim.GetCurrentAnimatorStateInfo(0).IsTag("1"))) {

            neighborSwitchAnim.SetBool("isIdle_Off", false);
            neighborSwitchScript.switchState = State.On;
        }
    }

    private void OnEnable() {

        controls.Enable();
    }

    private void OnDisable() {

        controls.Disable();
    }

    private void OnMouseOver() {

        hovering = true;
    }

    private void OnMouseExit() {

        hovering = false;
    }
}
