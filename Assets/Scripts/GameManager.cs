using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] levels;

    private int nextLevelIndex = 0;
    private bool changeLvl = false;

    public float speed;
    public Vector3 Direction;

    public AnimationCurve AnimCurve;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (changeLvl) {
            
            Transform previousLvlTransform = levels[nextLevelIndex - 1].GetComponent<Transform>();

            if (previousLvlTransform.position.x > -12f) {
                
                previousLvlTransform.position += Direction.normalized * speed * AnimCurve.Evaluate(Time.time) * Time.deltaTime;
            
            } else {

                levels[nextLevelIndex - 1].SetActive(false);
                levels[nextLevelIndex].SetActive(true);

                Transform currentLvlTransform = levels[nextLevelIndex].GetComponent<Transform>();
            
                if (currentLvlTransform.position.x > 0f) {
                    
                    currentLvlTransform.position += Direction.normalized * speed * AnimCurve.Evaluate(Time.time) * Time.deltaTime;
                
                }
            }
        }
    }

    IEnumerator Wait() {
        yield return new WaitForSeconds(1.0f);

        changeLvl = true;
    }

    public void nextLevel() {
        nextLevelIndex += 1;

        if (nextLevelIndex < levels.Length) {

            StartCoroutine(Wait());

        } else {

        }
    }
}
