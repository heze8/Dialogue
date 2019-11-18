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
    public List<string> sentences;
    private Typing typeManager;
    public TextMeshPro outputText;
    public TextMeshPro choices;
    public float speakingRadius = 10f;
    public LayerMask speakingLayer;
    private string currentDialogueChoices;
    private bool madeChoice;

    private void Start()
    {
        typeManager = new Typing();
        GetNextDialogueChoices();
    }

    private void GetNextDialogueChoices()
    {
        if (sentences.Count == 0)
        {
            currentDialogueChoices = "No more choices";
            choices.text = currentDialogueChoices;
            return;
        }
        currentDialogueChoices = sentences[0];
        choices.text = currentDialogueChoices;
        sentences.RemoveAt(0);
    }

    private void Update()
    {
        Type();
        
        UpdateOutput();
    }
    
    private void UpdateOutput()
    {
        string dialogueInput = typeManager.GetCurrentDialogueInput();

        int indexOfError = typeManager.GetIndexOfError(currentDialogueChoices);

        string formatOutput = FormatOutput(dialogueInput, indexOfError);
        outputText.text = formatOutput;
    }

    /// <summary>
    /// Causes the written input from the user to be coloured red based on the index of error.
    /// </summary>
    /// <param name="dialogueInput"></param>
    /// <param name="indexOfError"></param>
    /// <returns></returns>
    private string FormatOutput(string dialogueInput, int indexOfError)
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
    }

    private void Type()
    {
        foreach (char letter in Input.inputString)
        {
            if (letter.Equals('\r') || letter.Equals('\n'))
            {
                onEnter();
            }
            typeManager.TypeLetter(letter);
        }
    }

    private void onEnter()
    {
        if (madeChoice)
        {
            SpeakToNearby();
            GetNextDialogueChoices();
        }
    }
    
    void SpeakToNearby()
    {
        Collider2D[] peopleToSpeakTo = Physics2D.OverlapCircleAll(gameObject.transform.position, speakingRadius, speakingLayer);
        foreach (var people in peopleToSpeakTo)
        {
            var dialogueManager = people.GetComponent<DialogueManager>();
            if (!dialogueManager.Equals(this))
            {
                dialogueManager.Speak(typeManager.GetCurrentDialogueInput());
            }
        }
    }

    public void Speak(String message)
    {
        outputText.text = "Shut up!";
    }
}
