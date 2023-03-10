using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using BNG;

public class Chef_PickUp : MonoBehaviour
{
    public GameObject slotItem;
    public Chef_Inventory inven;
    public bool isDelay;
    
    private void Start()
    {
        isDelay = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            Chef_UIManager._Instance.activeinteraction();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Player")
        {
            Chef_UIManager._Instance.inactiveinteraction();
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        // VR -> Player 태그는 Locomation에 적용
        if (collision.tag == "Player" && InputBridge.Instance.RightTriggerUp)
        {
            if (!isDelay)
            {
                inven = collision.transform.root.GetComponent<Chef_Inventory>();
                for (int i = 0; i < inven.slots.Count; i++)
                {
                    if (inven.slots[i].isEmpty) 
                    {
                        isDelay = true;
                        Instantiate(slotItem, inven.slots[i].slotObj.transform, false);
                        //inven.slots[i].foodname = inven.GetComponent<Image>().sprite.name;
                        inven.slots[i].isEmpty = false;
                        StartCoroutine(pickUpDelay());
                        break;
                    }
                } 
            }
        }

        //inven.slots[i].slotObj.transform.GetChild(0).gameObject.GetComponent<Image>().sprite.name == ""
        IEnumerator pickUpDelay()
        { 
            yield return new WaitForSeconds(2f);
            isDelay = false;
        }
    }
}
