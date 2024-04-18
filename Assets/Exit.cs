using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [SerializeField] float nextLevelDelay = 2f;

    void Start()
    {
        
    }


    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Exit collided with: " + collision.transform.name + " on Layer: " + LayerMask.LayerToName(collision.gameObject.layer));
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(GoToNextLevel());
        }
    }

    private IEnumerator GoToNextLevel()
    {
        yield return new WaitForSeconds(nextLevelDelay);
        //load next scene
        int nextSceneIdx = SceneManager.GetActiveScene().buildIndex +1;
        if (nextSceneIdx >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("You Win!! Now Start Over");
            nextSceneIdx = 0;
        }
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(nextSceneIdx);
    }

}
