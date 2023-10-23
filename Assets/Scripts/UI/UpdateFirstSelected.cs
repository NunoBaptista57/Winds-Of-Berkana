using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpdateFirstSelected : MonoBehaviour
{

    [SerializeField] public GameObject firstSelected;
    
    void OnEnable()
    {
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(gameObject, new BaseEventData(eventSystem));    
        firstSelected.GetComponent<Button>().Select();
    }


}
