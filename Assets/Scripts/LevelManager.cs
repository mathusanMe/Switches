using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public InputMaster controls;

    public GameObject gameManager;

    private GameManager gameManagerScript; 

    [SerializeField]
    private GameObject[] switches;

    public Switch.State[] startSwitchesState;

    private GameObject latestSwitch;

    private List<(int, int)> history = new List<(int, int)>();

    private int previousHistoryCount = 0;

    private bool isGameOver = true;

    //public TransitionBackwards transitionBackwards;

    void Awake() {
        controls = new InputMaster();
        controls.UI.MouseLeftClick.performed += _ => MouseLeftClickFunc();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject mySwitch in switches) {
            Switch switchScript = mySwitch.GetComponent<Switch>();
            isGameOver = isGameOver && switchScript.switchState == Switch.State.On;
        }

        if (isGameOver && history.Count != previousHistoryCount) {

            gameManagerScript.nextLevel();
            previousHistoryCount += 1;
        }

        isGameOver = true;
    }

    void MouseLeftClickFunc() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit) {
            if (hit.transform.name == "MenuButton") {
                SceneManager.LoadScene(0);
            } else if (hit.transform.name == "RedoButton") {
                redo();
            } else if (hit.transform.name == "RestartButton") {
                restartLevel();
            }
        }
    }

    void redo() {

        if (latestSwitch) {

            Animator latestSwitchAnim = latestSwitch.GetComponent<Animator>();

            if (!(latestSwitchAnim.GetCurrentAnimatorStateInfo(0).IsTag("0") || latestSwitchAnim.GetCurrentAnimatorStateInfo(0).IsTag("1"))) {

                if (history.Count != 0) {

                    (int, int) lastSwitchCoordinates = history[history.Count - 1];

                    GameObject lastSwitch = GameObject.Find("Switch_" + lastSwitchCoordinates.Item1 + "x" + lastSwitchCoordinates.Item2);

                    Switch lastSwitchScript = lastSwitch.GetComponent<Switch>();
                    lastSwitchScript.changeState(lastSwitch);

                    foreach(GameObject neighborSwitchOfLastSwitch in lastSwitchScript.neighborSwitches) {

                        if (neighborSwitchOfLastSwitch != null) {
                            Switch neighborSwitchOfLastSwitchScript = neighborSwitchOfLastSwitch.GetComponent<Switch>();
                            neighborSwitchOfLastSwitchScript.changeState(neighborSwitchOfLastSwitch);
                        }
                    }

                    history.RemoveAt(history.Count - 1);

                    latestSwitch = lastSwitch;
                }
            }
        } else {

            if (history.Count != 0) {

                    (int, int) lastSwitchCoordinates = history[history.Count - 1];

                    GameObject lastSwitch = GameObject.Find("Switch_" + lastSwitchCoordinates.Item1 + "x" + lastSwitchCoordinates.Item2);

                    Switch lastSwitchScript = lastSwitch.GetComponent<Switch>();
                    lastSwitchScript.changeState(lastSwitch);

                    foreach(GameObject neighborSwitchOfLastSwitch in lastSwitchScript.neighborSwitches) {

                        if (neighborSwitchOfLastSwitch != null) {
                            Switch neighborSwitchOfLastSwitchScript = neighborSwitchOfLastSwitch.GetComponent<Switch>();
                            neighborSwitchOfLastSwitchScript.changeState(neighborSwitchOfLastSwitch);
                        }
                    }

                    history.RemoveAt(history.Count - 1);

                    latestSwitch = lastSwitch;
            }
        }
    }

    void restartLevel() {
        
        int i = 0;

        foreach (GameObject mySwitch in switches) {
            
            Switch switchScript = mySwitch.GetComponent<Switch>();
            
            if (switchScript.switchState != startSwitchesState[i]) {

                switchScript.changeState(mySwitch);
            }
            ++i;
        }
    }

    public void addToHistory((int, int) log) {

        history.Add(log);
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }
}
