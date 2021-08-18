using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public InputMaster controls;

    //public TransitionBackwards transitionBackwards;

    void Awake() {
        controls = new InputMaster();
        controls.UI.MouseLeftClick.performed += _ => MouseLeftClickFunc();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MouseLeftClickFunc() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit) {
            if (hit.transform.name == "MenuButton") {
                SceneManager.LoadScene(0);
            } else if (hit.transform.name == "RedoButton") {
                print("Redo");
            } else if (hit.transform.name == "RestartButton") {
                print("Restart");
            }
        }
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }
}
