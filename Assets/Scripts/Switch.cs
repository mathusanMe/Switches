using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Switch : MonoBehaviour
{
    public InputMaster controls;
    //public Transition transition;

    [SerializeField]
    private Sprite switchSelectedStateOn;
    [SerializeField]
    private Sprite switchSelectedStateOff;
    [SerializeField]
    private Sprite switchUnSelectedStateOn;
    [SerializeField]
    private Sprite switchUnSelectedStateOff;
    
    private SpriteRenderer spriteRenderer;
    private Animator switchAnim;

    private bool hovering = false;

    [SerializeField]    
    private State switchState;

    private enum State {On, Off}

    void Awake() {

        controls = new InputMaster();
        controls.UI.MouseLeftClick.performed += _ => MouseLeftClickFunc();
    }

    // Start is called before the first frame update
    void Start()
    {
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

                if (switchState == State.On) {

                    switchAnim.SetBool("isIdle_On", false);
                    switchState = State.Off;

                } else if (switchState == State.Off) {

                    switchAnim.SetBool("isIdle_Off", false);
                    switchState = State.On;
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
