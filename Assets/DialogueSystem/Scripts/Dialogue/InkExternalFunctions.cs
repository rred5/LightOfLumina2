using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class InkExternalFunctions
{
    private BusinessManager businessManager;

    public InkExternalFunctions()
    {
        // Find the BusinessManager instance in the scene
        businessManager = GameObject.FindObjectOfType<BusinessManager>();
        if (businessManager == null)
        {
            Debug.LogError("BusinessManager not found in the scene.");
        }
    }

    public void Bind(Story story, Animator emoteAnimator)
    {
        story.BindExternalFunction("playEmote", (string emoteName) => PlayEmote(emoteName, emoteAnimator));
        story.BindExternalFunction("businessFunction", (string argument) => CallBusinessFunction(argument));
    }

    public void Unbind(Story story)
    {
        story.UnbindExternalFunction("playEmote");
        story.UnbindExternalFunction("businessFunction");
    }

    private void PlayEmote(string emoteName, Animator emoteAnimator)
    {
        if (emoteAnimator != null)
        {
            emoteAnimator.Play(emoteName);
        }
        else
        {
            Debug.LogWarning("Tried to play emote, but emote animator was not initialized when entering dialogue mode.");
        }
    }

    private void CallBusinessFunction(string argument)
    {
        if (businessManager != null)
        {
            if (argument == "FBLAmoney")
            {
                businessManager.changeMoney(10000); // Modify this to call the actual method and pass the argument
            }
        }
        else
        {
            Debug.LogWarning("BusinessManager reference is not set.");
        }
    }
}
