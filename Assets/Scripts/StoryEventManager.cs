using UnityEngine;
using System.Collections.Generic;

public class StoryEventManager : MonoBehaviour
{
    // Singleton instance
    public static StoryEventManager Instance;

    private void Awake()
    {
        // Ensure that there's only one instance of StoryEventManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Dictionary to hold game objects and their initial states
    private Dictionary<GameObject, bool> gameObjectStates = new Dictionary<GameObject, bool>();

    // Function to add a game object to the dictionary
    public void RegisterGameObject(GameObject obj)
    {
        if (!gameObjectStates.ContainsKey(obj))
        {
            gameObjectStates.Add(obj, obj.activeSelf);
        }
    }

    // Function to enable a game object
    public void EnableGameObject(GameObject obj)
    {
        if (gameObjectStates.ContainsKey(obj))
        {
            obj.SetActive(true);
        }
    }

    // Function to disable a game object
    public void DisableGameObject(GameObject obj)
    {
        if (gameObjectStates.ContainsKey(obj))
        {
            obj.SetActive(false);
        }
    }

    // Function to reset game objects to their initial states
    public void ResetGameObjects()
    {
        foreach (var item in gameObjectStates)
        {
            item.Key.SetActive(item.Value);
        }
    }

    // Function to enable multiple game objects
    public void EnableGameObjects(List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            EnableGameObject(obj);
        }
    }

    // Function to disable multiple game objects
    public void DisableGameObjects(List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            DisableGameObject(obj);
        }
    }

    // Function to enable game objects by name
    public void EnableGameObjectByName(string name)
    {
        foreach (var obj in gameObjectStates.Keys)
        {
            if (obj.name == name)
            {
                EnableGameObject(obj);
            }
        }
    }

    // Function to disable game objects by name
    public void DisableGameObjectByName(string name)
    {
        foreach (var obj in gameObjectStates.Keys)
        {
            if (obj.name == name)
            {
                DisableGameObject(obj);
            }
        }
    }
}



