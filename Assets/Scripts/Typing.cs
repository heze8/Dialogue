using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public class Typing
{
    private StringBuilder myDialogue;
    private int spaceBuffer = 0; //to prevent words from going out of screen.
    private int dialogueLength;
    private const int MaxWrongChars = 5;
    
    public Typing()
    {
        myDialogue = new StringBuilder();
    }
    
    public void TypeLetter (char letter)
    {
        dialogueLength = myDialogue.Length;
        switch (letter)
        {
            //backspace
            case '\b':
                if (dialogueLength > 0)
                {
                    myDialogue.Remove(dialogueLength - 1, 1);
                    spaceBuffer--;
                }

                break;
            
            //enter
            case '\n':
            case '\r':
                myDialogue.Clear();
                spaceBuffer = 0;
                break;
            
            case ' ':
                if (dialogueLength > 0 && spaceBuffer < MaxWrongChars)
                {
                    spaceBuffer++;
                    myDialogue.Append(letter);
                }
                break;
            
            default:
                myDialogue.Append(letter);
                spaceBuffer = 0;
                break;
        }
    }

    public string GetCurrentDialogueInput()
    {
        return myDialogue.ToString();
    }

    /// <summary>
    /// Method that returns the first index on which the error occurs in the sentence.
    /// </summary>
    /// <param name="sentence"></param> sentence is the DialogueSentence given.
    /// <returns></returns> the index by char of the error, returns the length of the string if no error.
    public int GetIndexOfError (string sentence)
    {
        if (!sentence.Any()) return 0;
        
        var sentenceChars = sentence.ToCharArray();
        dialogueLength = myDialogue.Length;
        for (var i = 0; i < dialogueLength; i++)
        {
            if (i >= sentenceChars.Length || !sentenceChars[i].Equals(myDialogue[i]))
            {
                return i;
            }
        }

        return dialogueLength;
    }
}
