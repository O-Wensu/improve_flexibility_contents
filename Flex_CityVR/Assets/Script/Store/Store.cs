﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    // Store을 열거나 종료할 경우 InfromType 변경은 버튼 onClick에서 수행
    //public GameObject content;
    //public GameObject petItemPrefab;

    [HideInInspector]
    public int UserMoney; // DB 연동 필요
    public GameObject PetShop;
    public Text UserMoneyText;
    public Transform petItemRoot;
    bool canBuy = false;
    
    private int getItemCost;
    private string getItemName;
    private int itemIndex;

    [HideInInspector]
    public UnityEngine.UI.Button button;

    public static Store instance;
    private void Awake()
    {
        Store.instance = this;
        UserMoney = 1000000;
        // UserMoney 연동
        getItemCost = 0;
        itemIndex = 0;
        UserMoneyText.text = GetThousandComma(UserMoney).ToString();
    }

    void Start()
    {
        /// 동적 Item 추가///
/*        for (int i = 0; i < 3; i++)
        {
            Instantiate<GameObject>(this.petItemPrefab, content.transform);
        }*/

    }

    void Update()
    {
        
    }

    // 1000단위 comma
    public string GetThousandComma(int data)
    {
        return string.Format("{0:#,###}",data);
    }

    public void ItemClick(int index)
    {
        button = EventSystem.current.currentSelectedGameObject.transform.GetComponent<Button>();
        button.enabled = false;
        UIManager.instance.setInformType(2); // Shop
        UIManager.instance.informPanel.SetActive(true);
        UIManager.instance.informText.text = "아이템을 구매하시겠습니까?"; //informtype shop일때
        itemIndex = index;
        getItemName = petItemRoot.GetChild(index).GetComponent<ItemInfo>().itemName;
        getItemCost = petItemRoot.GetChild(index).GetComponent<ItemInfo>().itemCost;
        if (UserMoney >= getItemCost)
        {
            canBuy = true;
        }
        else
            canBuy = false;
    }

    public void BuyItem()
    {
        StartCoroutine(UIManager.instance.SimpleInform());
        if (canBuy)
        {
            UIManager.instance.informText_simple.text = "구매를 완료했습니다.";
            UserMoney -= getItemCost;
            UserMoneyText.text = GetThousandComma(UserMoney).ToString();

            // 구매한 아이템의 ItemInfo 전달
            var itemInfo = petItemRoot.GetChild(itemIndex).GetComponent<ItemInfo>();
            Inventory.instance.UpdateItem(itemInfo);
        }
        else
        {
            UIManager.instance.informText_simple.text = "다이아가 부족합니다.";
        }
        button.enabled = true;
        button = null;
    }
}