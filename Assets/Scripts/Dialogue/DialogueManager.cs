using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    public List<string> sentences;
    private Typing typeManager;
    public Text uiText;
    private string currentDialogue;

    private void Start()
    {
        typeManager = new Typing();
        GetNextDialogue();
    }

    private void GetNextDialogue()
    {
        if (sentences.Count == 0)
        {
            currentDialogue = "";
            return;
        }
        currentDialogue = sentences[0];
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

        int indexOfError = typeManager.GetIndexOfError(currentDialogue);

        string formatOutput = FormatOutput(dialogueInput, indexOfError);
        uiText.text = formatOutput;
    }

    private string FormatOutput(string dialogueInput, int indexOfError)
    {
        string errorText = dialogueInput.Substring(indexOfError);
        
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
            typeManager.TypeLetter(letter);
        }
    }
}
