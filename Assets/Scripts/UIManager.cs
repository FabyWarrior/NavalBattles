using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Image p1Bar;
    public Image p2Bar;

    void Start()
    {
        EventManager.AddEventListener(Events.ON_LIFE_UPDATE, OnLifeUpdate);
    }

    private void OnLifeUpdate(params object[] paramsContainer)
    {
        if ((string)paramsContainer[0] == "Char")
            p1Bar.fillAmount = (float)paramsContainer[1];
        else
            p2Bar.fillAmount = (float)paramsContainer[1];
    }
}
