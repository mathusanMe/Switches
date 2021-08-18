using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public InputMaster controls;
    //public Transition transition;

    [SerializeField]
    private Sprite playButtonUnSelectedState;
    
    private SpriteRenderer spriteRenderer;
    private Animator playButtonAnim;
    private bool hovering = false;

    void Awake() {
        controls = new InputMaster();
        controls.UI.MouseLeftClick.performed += _ => MouseLeftClickFunc();
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        playButtonAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hovering) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit) {
                if (hit.transform.name == "PlayButton") {
                    playButtonAnim.SetBool("isHovered", true);
                }
            }
        } else {
            spriteRenderer.sprite = playButtonUnSelectedState;
            playButtonAnim.SetBool("isHovered", false);
        }
    }

    void MouseLeftClickFunc() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit) {
            if (hit.transform.name == "PlayButton") {
                playButtonAnim.SetBool("isClicked", true);
            }
        }
    }

    void LoadLevel() {
        SceneManager.LoadScene(1);
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
