using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinatioPuzzle : MonoBehaviour
{
    public ButtonsCombPuzzle[] buttons;
    public GameObject[] lights;  
    // Start is called before the first frame update

     [SerializeField] private GameObject vitral;

    public void Awake(){
        
        buttons = GetComponentsInChildren<ButtonsCombPuzzle>();        
        if(buttons.Length <= 0)
        {
            Debug.LogWarning("No buttons found: " + buttons.Length);
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].OnActivated += (buttonPuzzle) => SolvingTurn(buttonPuzzle, index);
        }
    }
    public void SolvingTurn(ButtonsCombPuzzle buttonPuzzle, int index){
        if(buttons.Length > 3){
        if(index == 0){
            buttons[0].activePart = !buttons[0].activePart;
            buttons[index+1].activePart = !buttons[index+1].activePart;
            buttons[buttons.Length-1].activePart = !buttons[buttons.Length-1].activePart;
            Debug.Log(buttons[0].activePart);
        }
        else if(index == buttons.Length-1){
            buttons[0].activePart = !buttons[0].activePart;
            buttons[index-1].activePart = !buttons[index-1].activePart;
            buttons[index].activePart = !buttons[index].activePart;
        }
        else{
            buttons[index].activePart = !buttons[index].activePart;
            buttons[index-1].activePart = !buttons[index-1].activePart;
            buttons[index+1].activePart = !buttons[index+1].activePart;
        }}
        else{
            if(index == 0){
                buttons[0].activePart = !buttons[0].activePart;
                buttons[1].activePart = !buttons[1].activePart;
            }
            else if(index == 1){
                buttons[1].activePart = !buttons[1].activePart;
            }
            else{
                buttons[0].activePart = !buttons[0].activePart;
                buttons[2].activePart = !buttons[2].activePart;                   
            }
        }
        LightsOnOff();
        CheckWin();
    }

    private void LightsOnOff(){
        for (int i = 0; i < buttons.Length; i++){
            if(buttons[i].activePart){
                lights[i].SetActive(true);
            }
            else{
                lights[i].SetActive(false);
            }
        }
    }

    private void CheckWin(){
        foreach (GameObject light in lights){
            if (light.activeSelf)
                continue;
            else
                return;
        }
        vitral.SetActive(true);
    }

}
