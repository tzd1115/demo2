using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelBase : MonoBehaviour
{
   public  MenuPanel panel;
    public virtual void Show(MenuPanel p)
    {
        this.gameObject.SetActive(true);
        panel = p;
    }

    public virtual void Hide()
    {
        this.gameObject.SetActive(false);
    }
    public bool IsActive()
    {
       return this.gameObject.activeSelf;
    }
}
