﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour // (21/10/26 주석 및 코드수정)
{
    public static bool TextboxActivated = false; // 텍스트창 온오프체크용

    public Text interText; // 상호작용 표시 텍스트
    public Text talkBoxText; // 텍스트창 내부 텍스트
    public Text questBoxNameText; // 퀘스트창 제목 텍스트
    public Text questBoxText; // 퀘스트창 내용 텍스트
    public GameObject scanObject; // 텍스트창 백그라운드
    public Image FadeInImage; // 페이드 인/아웃

    private float rayRange = 3f; //레이 거리조절
    [SerializeField]
    private float getRange; // 아이템 습득가능 거리

    private bool pickupActivated = false; // 습득 가능유무 체크

    private bool textActivated = false; // 대화창 가능유무 체크
    private bool isTextDone = false; // 대화창 대화종료 체크
    private bool ObjScan = true;

    public  bool quest1Check = false;
    private bool quest2Check = false;
    public bool quest3Start = false;
    public bool quest3Check = false;
    private bool questboxMoveCheck = false;
    private bool questboxReturnCheck = false;

    private RaycastHit hitinfo; // 충돌체 정보저장
    private RaycastHit hitinfo2; // 충돌체 정보저장

    public int textIndex;
    private int QuestDelayTime = 500;

    Vector3 QuestShowPos = new Vector3(0, 0, 0);
    Vector3 QuestHidePos = new Vector3(-423, 0, 0);

    // 필요 컴포넌트
    [SerializeField]
    private LayerMask itemlayerMask; // 아이템 레이어 설정
    [SerializeField]
    private LayerMask objlayerMask; // 오브젝트 레이어 설정
    [SerializeField]
    TextBoxManager txtManager; // 텍스트 관리자
    [SerializeField]
    QuestBoxManager questManager;
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private Image pickImage;
    [SerializeField]
    private Image pickItemImage;
    [SerializeField]
    private GameObject go_Quest;
    [SerializeField]
    private GameObject quest1;
    [SerializeField]
    private GameObject quest2;

    public Item item; // 아이템 정보
    // Update is called once per frame

    private void Awake()
    {
        go_Quest.SetActive(false);
    }

    private void Start()
    {
        questBoxNameText.text = questManager.QuestCheck();
        questBoxText.text = questManager.QuestContentsCheck();
    }

    void Update()
    {
        CheckItem();
        CheckObject();
        TryAction();
        TextBoxChange();
        QuestBoxAppear();
    }

    public void Quest1Action()
    {
        quest1Check = true;

        Time.timeScale = 0;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        TextBoxData txtData = quest1.transform.GetComponent<TextBoxData>();
        TextChange(txtData.id, txtData.isNpc);

        ObjScan = false;
        textActivated = true;
        TextboxActivated = true;
        interText.gameObject.SetActive(false);
        scanObject.SetActive(true);
    }

    public void Quest2Action()
    {
        quest2Check = true;

        Time.timeScale = 0;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        TextBoxData txtData = quest2.transform.GetComponent<TextBoxData>();
        TextChange(txtData.id, txtData.isNpc);

        ObjScan = false;
        textActivated = true;
        TextboxActivated = true;
        interText.gameObject.SetActive(false);
        scanObject.SetActive(true);
    }

    private void TryAction()
    {
        if(Time.timeScale == 1)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                CanPickUp();
                CanTalkUp();
            }
            else if (Input.GetKeyDown(KeyCode.Tab) && QuestDelayTime != 0)
            {
                go_Quest.SetActive(true);
                questboxMoveCheck = true;
            }
        }

    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitinfo.transform != null)
            {
                inventory.AcquireItem(hitinfo.transform.GetComponent<ItemPickup>().item);
                Destroy(hitinfo.transform.gameObject);
                ItemInfoDisappear();
            }
        }
    }

    private void CanTalkUp()
    {
        if (textActivated)
        {
            if (hitinfo2.transform != null)
            {
                if(hitinfo2.transform.name == "Robot_Dummy")
                {
                    quest3Start = true;
                    quest3Check = true;

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    TextBoxData txtData = hitinfo2.transform.GetComponent<TextBoxData>();
                    TextChange(txtData.id, txtData.isNpc);

                    ObjScan = false;
                    TextboxActivated = true;
                    interText.gameObject.SetActive(false);
                    scanObject.SetActive(true);

                }
                else
                {
                    Time.timeScale = 0;

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    TextBoxData txtData = hitinfo2.transform.GetComponent<TextBoxData>();
                    TextChange(txtData.id, txtData.isNpc);

                    ObjScan = false;
                    TextboxActivated = true;
                    interText.gameObject.SetActive(false);
                    scanObject.SetActive(true);
                }
            }
        }
    }

    private void CheckItem()
    {
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward)
            , out hitinfo, rayRange, itemlayerMask))
        {
            if(hitinfo.transform.tag == "item")
            {
                item = hitinfo.transform.GetComponent<ItemPickup>().item;
                ItemInfoAppear(item);
            }
        }
        else
        {
            ItemInfoDisappear();
        }

    }

    private void CheckObject()
    {
        if (ObjScan)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitinfo2, rayRange, objlayerMask))
            {
                if (hitinfo2.transform.tag == "Object")
                {
                    ObjectTextAppear();
                }
            }
            else
            {
                ObjectTextDisappear();
            }
        }
    }
    private void ItemInfoAppear(Item _item)
    {
        pickupActivated = true;
        pickImage.gameObject.SetActive(true);
        pickItemImage.gameObject.SetActive(true);
        pickItemImage.sprite = _item.itemImage;
    }

    private void ItemInfoDisappear()
    {
        pickupActivated = false;
        pickImage.gameObject.SetActive(false);
        pickItemImage.gameObject.SetActive(false);
        pickItemImage.sprite = null;
    }

    private void ObjectTextAppear()
    {
        interText.gameObject.SetActive(true);
        textActivated = true;
    }
    private void ObjectTextDisappear()
    {
        interText.gameObject.SetActive(false);
        textActivated = false;
    }

    private void QuestBoxAppear()
    {
        if (questboxMoveCheck)
        {
            go_Quest.transform.localPosition = Vector3.Lerp(go_Quest.transform.localPosition, QuestShowPos,0.02f);
            if(QuestDelayTime > 0)
            {
                QuestDelayTime--;
                if (QuestDelayTime == 0)
                {
                    questboxReturnCheck = true;
                    questboxMoveCheck = false;
                }
            }

            if (go_Quest.transform.localPosition.x >= -1f)
            {
                go_Quest.transform.localPosition = QuestShowPos;
            }
        }

        else if (questboxReturnCheck)
        {
            go_Quest.transform.localPosition = Vector3.Lerp(go_Quest.transform.localPosition, QuestHidePos, 0.02f);
            if (go_Quest.transform.localPosition.x <= -422f)
            {
                go_Quest.transform.localPosition = QuestHidePos;
                QuestDelayTime = 500;
                questboxReturnCheck = false;
            }
        }
    }

    public void TextChange(int id, bool isNpc)
    {
        int questIndex = questManager.GetQuestIndex(id);
        string txtData = txtManager.GetTalk(id + questIndex, textIndex);

        if(txtData == null)
        {
            isTextDone = true;
            textActivated = false;
            quest1Check = false;
            quest2Check = false;
            quest3Check = false;
            textIndex = 0;
            questBoxNameText.text = questManager.QuestCheck(id);
            questBoxText.text = questManager.QuestContentsCheck(id);

            if (!quest1Check)
            {
                if(quest1 != null)
                {
                    Destroy(quest1);
                }
                
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {

                if (quest3Start && !quest3Check)
                {
                    ObjScan = false;
                    MainSceneChanger.GameClearActivated = true;
                    scanObject.SetActive(false);
                }

                else
                {
                    Time.timeScale = 1;

                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;


                    isTextDone = false;
                    ObjScan = true;
                    TextboxActivated = false;
                    interText.gameObject.SetActive(false);
                    scanObject.SetActive(false);
                }

            }
            return;
        }

        if (isNpc)
        {
            talkBoxText.text = txtData;
        }
        else
        {
            talkBoxText.text = txtData;
        }

        textIndex++;
    }


    private void TextBoxChange()
    {
        if (!isTextDone && textActivated)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (quest1Check)
                {
                    TextBoxData txtData = quest1.transform.GetComponent<TextBoxData>();
                    TextChange(txtData.id, txtData.isNpc);
                }
                else if (quest2Check)
                {
                    TextBoxData txtData = quest2.transform.GetComponent<TextBoxData>();
                    TextChange(txtData.id, txtData.isNpc);
                }
                else if (quest3Start)
                {
                    Color fadecol = FadeInImage.color;
                    fadecol.a += 0.3f; 
                    FadeInImage.color = fadecol;
                    TextBoxData txtData = hitinfo2.transform.GetComponent<TextBoxData>();
                    TextChange(txtData.id, txtData.isNpc);
                }
                else
                {
                    TextBoxData txtData = hitinfo2.transform.GetComponent<TextBoxData>();
                    TextChange(txtData.id, txtData.isNpc);
                }

            }
        }
    }
}
