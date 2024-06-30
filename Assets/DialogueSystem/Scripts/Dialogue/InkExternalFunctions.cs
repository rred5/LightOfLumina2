using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class InkExternalFunctions
{
    private BusinessManager businessManager;
    private StoryEventManager storyEventManager;
    public InkExternalFunctions()
    {
        // Find the BusinessManager instance in the scene
        businessManager = GameObject.FindObjectOfType<BusinessManager>();
        if (businessManager == null)
        {
            Debug.LogError("BusinessManager not found in the scene.");
        }

        storyEventManager = GameObject.FindObjectOfType<StoryEventManager>();
        if (businessManager == null)
        {
            Debug.LogError("StoryEventManager not found in the scene.");
        }
    }

    public void Bind(Story story, Animator emoteAnimator)
    {
        story.BindExternalFunction("playEmote", (string emoteName) => PlayEmote(emoteName, emoteAnimator));
        story.BindExternalFunction("businessFunction", (string argument) => CallBusinessFunction(argument));
        story.BindExternalFunction("storyEvent", (string argumentS) => CallStoryFunction(argumentS));
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
    private void CallStoryFunction(string argumentS) {
        if (storyEventManager != null)
        {
            #region Level1
            if (argumentS == "disableWorkerInit")
            {
                storyEventManager.DisableGameObjectByName("WorkerInit"); // Modify this to call the actual method and pass the argument
            }

            if (argumentS == "enableWorkerHint")
            {
                storyEventManager.EnableGameObjectByName("WorkerHint"); // Modify this to call the actual method and pass the argument
            }

            if (argumentS == "enableL1FirstClue"){
                storyEventManager.EnableGameObjectByName("FirstClue");
            }

            if (argumentS == "enableCustomer")
            {
                storyEventManager.EnableGameObjectByName("Customer");
            }


            #endregion

            #region Level2
            if (argumentS == "PI1") {
                storyEventManager.EnableGameObjectByName("PI_1");
            }
            if (argumentS == "PI2")
            {
                storyEventManager.EnableGameObjectByName("PI_2");

            }

            if (argumentS == "EdisonHintL2") {
                storyEventManager.EnableGameObjectByName("EdisonHintL2");
                    }

          
            if (argumentS == "PI3")
            {
                storyEventManager.EnableGameObjectByName("PI_3");
            }

            if (argumentS == "BankHint") {
                storyEventManager.EnableGameObjectByName("Bank_clue");

            }

            if (argumentS == "Bank_disable")
            {
                storyEventManager.DisableGameObjectByName("NPC");

            }
            if (argumentS == "PI4")
            {
                storyEventManager.EnableGameObjectByName("PI_4");
            }



            if (argumentS == "PI1_disable")
            {
                storyEventManager.DisableGameObjectByName("PI_1");
            }
            if (argumentS == "PI2_disable")
            {
                storyEventManager.DisableGameObjectByName("PI_2");
            }
            if (argumentS == "PI3_disable")
            {
                storyEventManager.DisableGameObjectByName("PI_3");
            }

            #endregion
        }
        else
        {
            Debug.LogWarning("BusinessManager reference is not set.");
        }
    }
    private void CallBusinessFunction(string argument)
    {
        if (businessManager != null)
        {
            #region Level1
            if (argument == "FBLAmoney")
            {
                businessManager.changeMoney(10000); // Modify this to call the actual method and pass the argument
            }
            if (argument == "hireFirst") {
                businessManager.HireWorker();
                businessManager.HireWorker();
            }
            if (argument == "allowFinish") {
                GameManager.Instance.allowFinish = true;
            }
            #endregion

            #region Level2
            if (argument == "Banker1") {
                businessManager.setInterest(0.05f);
                businessManager.TakeLoan(100000f);
            }

            if (argument == "Banker2") {
                businessManager.setInterest(0.15f);
                businessManager.TakeLoan(200000f);
            }

            if (argument == "Banker3") {
                businessManager.setInterest(0.10f);
                businessManager.TakeLoan(50000f);
            }
            #endregion

            #region Level3
            if (argument == "L3Banker1")
            {
                businessManager.setInterest(0.05f);
                businessManager.TakeLoan(1000000f);
            }

            if (argument == "L3Banker2")
            {
                businessManager.setInterest(0.07f);
                businessManager.TakeLoan(1000000f);
            }

            if (argument == "L3Banker3")
            {
                businessManager.setInterest(0.03f);
                businessManager.TakeLoan(1500000f);
            }

            #endregion
        }
        else
        {
            Debug.LogWarning("BusinessManager reference is not set.");
        }
    }
}
