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
                ClearDialogue();
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

    public void ClearDialogue()
    {
        myDialogue.Clear();
    }

    public string GetCurrentDialogueInput()
    {
        return myDialogue.ToString();
    }
}
