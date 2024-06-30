using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1StoryLoader : MonoBehaviour
{
    // List to hold all game objects to be registered
    public List<GameObject> gameObjectsToRegister;

    // Start is called before the first frame update
    void Start()
    {
        // Register each game object to the StoryEventManager
        foreach (GameObject obj in gameObjectsToRegister)
        {
            StoryEventManager.Instance.RegisterGameObject(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}



