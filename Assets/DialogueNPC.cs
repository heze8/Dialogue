using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DialogueNPC : MonoBehaviour
{    
    [SerializeField]
    public List<string> sentences;
    private string currentDialogueChoices;
    public TextMeshPro choices;
    public TextMeshPro output;
    private List<Char> alphabet = new List<Char>{'a', 'b', 'c','d','e'};
    
    

    private void Start()
    {
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

    private void SayALoud(String dialogue)
    {
        output.text = dialogue;
    }

    private void GenerateResponse()
    {
        var text = new StringBuilder();
        for (var i = 0; i < Random.Range(5, 20); i++)
        {
            foreach (Char c in alphabet)
            {
                text.Append(c, Random.Range(1, 5));
            }
        }
        SayALoud(text.ToString());
    }
    

    public void Speak(String dialogueSpoken)
    {
        int indexOfError = DialogueManager.GetIndexOfError(dialogueSpoken, currentDialogueChoices);
        if (indexOfError == dialogueSpoken.Length && indexOfError == currentDialogueChoices.Length)
        {
            GenerateResponse();
            GetNextDialogueChoices();
        }
        else
        {
            SayALoud("?");
        }
    }
    
}
