using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionBackwards : MonoBehaviour
{
    public Animator animator;
    public float transitionDelayTime = 1.0f;

    void Awake() {
        animator = GameObject.Find("Transition").GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadLevel() {
        
        StartCoroutine(DelayLoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
    }

    IEnumerator DelayLoadLevel(int index) {
        
        animator.SetTrigger("TriggerTransitionBackwards");
        yield return new WaitForSeconds(transitionDelayTime);
        SceneManager.LoadSceneAsync(index);
    }
}
