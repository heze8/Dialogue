using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private Typing typeManager;
    public TextMeshPro outputText;
    public float speakingRadius = 10f;
    public LayerMask speakingLayer;
    private bool isSpeaking;

    private void Start()
    {
        typeManager = new Typing();
        isSpeaking = false;
    }

    

    private void Update()
    {
        Type();
        
        UpdateOutput();
    }
    
    private void UpdateOutput()
    {
        string dialogueInput = typeManager.GetCurrentDialogueInput();

        //int indexOfError = typeManager.GetIndexOfError(currentDialogueChoices);

        //string formatOutput = FormatOutput(dialogueInput, indexOfError);
        outputText.text = dialogueInput;//formatOutput;
    }

    /// <summary>
    /// Causes the written input from the user to be coloured red based on the index of error.
    /// </summary>
    /// <param name="dialogueInput"></param>
    /// <param name="indexOfError"></param>
    /// <returns></returns>
    /*private string FormatOutput(string dialogueInput, int indexOfError)
    {
        if (dialogueInput.Length == indexOfError)
        {
            if (currentDialogueChoices.Length == indexOfError)
            {
                madeChoice = true;
            }
            return dialogueInput;
        }

        madeChoice = false;
        
        string errorText = null;
            
        errorText = dialogueInput.Substring(indexOfError);


        if (!errorText.Any())
        {
            return dialogueInput;
        }
        
        string correctText = dialogueInput.Substring(0, indexOfError);
        
        var formattedText = new StringBuilder(correctText);
        formattedText.Append("<color=red>");
        formattedText.Append(errorText);
        formattedText.Append("</color>");

        return formattedText.ToString();
    }*/

    private void Type()
    {
        foreach (char letter in Input.inputString)
        {
            if (letter.Equals('\r') || letter.Equals('\n'))
            {
                onEnter();
            }
            
            if (isSpeaking)
                typeManager.TypeLetter(letter);
        }
    }

    private void onEnter()
    {
        if (isSpeaking)
        {
            SpeakToNearby();
            isSpeaking = false;
            typeManager.ClearDialogue();
        }
        else 
            isSpeaking = true;
        //GetNextDialogueChoices();

    }
    
    void SpeakToNearby()
    {
        Collider2D[] peopleToSpeakTo = Physics2D.OverlapCircleAll(gameObject.transform.position, speakingRadius, speakingLayer);
        foreach (var people in peopleToSpeakTo)
        {
            var dialogueManager = people.gameObject.GetComponent<DialogueNPC>();
            if (dialogueManager != null) 
                dialogueManager.Speak(typeManager.GetCurrentDialogueInput());
        }
    }

    public void Speak(String message)
    {
        outputText.text = "Shut up!";
    }
    
    /// <summary>
    /// Method that returns the first index on which the error occurs in the first sentence compared to the second sentence.
    /// </summary>
    /// <param name="firstSentence"></param> sentence is the one where the error is found.
    /// <param name="secondSentence"></param> sentence is the crosschecked sentence.
    /// <returns></returns> the index by char of the error, returns the length of the string if no error.
    public static int GetIndexOfError (string firstSentence, string secondSentence)
    {
        if (!secondSentence.Any()) return 0;
        
        var sentenceChars = secondSentence.ToCharArray();
        int dialogueLength = firstSentence.Length;
        for (var i = 0; i < dialogueLength; i++)
        {
            if (i >= sentenceChars.Length || !sentenceChars[i].Equals(firstSentence[i]))
            {
                return i;
            }
        }

        return dialogueLength;
    }
}
