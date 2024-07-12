using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinatioPuzzle : MonoBehaviour
{
    public ButtonsCombPuzzle[] buttons;
    // Start is called before the first frame update

    public void Awake(){
        
        buttons = GetComponentsInChildren<ButtonsCombPuzzle>();        
        if(buttons.Length != 4)
        {
            Debug.LogWarning("Expected exactly 4 ButtonCombPuzzles, but found " + buttons.Length);
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; 
            buttons[i].OnActivated += (buttonPuzzle) => SolvingTurn(buttonPuzzle, index);
        }
    }
    public void SolvingTurn(ButtonsCombPuzzle buttonPuzzle, int index){
        if(index == 0){
            buttons[0].activePart = !buttons[0].activePart;
            buttons[index+1].activePart = !buttons[index+1].activePart;
            buttons[buttons.Length-1].activePart = !buttons[buttons.Length-1].activePart;
        }
        else if(index == buttons.Length-1){
            buttons[0].activePart = !buttons[index+1].activePart;
            buttons[index-1].activePart = !buttons[index-1].activePart;
            buttons[index].activePart = !buttons[index].activePart;
        }
        else{
            buttons[index].activePart = !buttons[index].activePart;
            buttons[index-1].activePart = !buttons[index-1].activePart;
            buttons[index+1].activePart = !buttons[index+1].activePart;
        }
    }


}
