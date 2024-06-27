using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIType
{
    Signup,
    Login,
    Lobby,
    SetPlayerNumPopup,
    Room,
    Battle
}

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject UIRoot;

    private static UIManager _instance = null;

    public static UIManager Instance
    {
        get
        {
            return _instance;
        }
    }

    //UI Resources.Load로 불러내도록 개선하기
    //_createdUIDic : 생성된 UI들을 저장
    private Dictionary<UIType, GameObject> _createdUIDic = new Dictionary<UIType, GameObject>();

    //_openedUIDic : 활성화된 UI들을 저장
    private HashSet<UIType> _openedUIDic = new HashSet<UIType>();

    private void Awake()
    {
       _instance = this;
    }

    private void CreateUI(UIType uiType)
    {
        if(_createdUIDic.ContainsKey(uiType) == false) 
        {
            string path = GetUIPath(uiType);
            GameObject loadedUIPrefab = (GameObject)Resources.Load(path);    //UI 프리펩
            GameObject createdUI = Instantiate(loadedUIPrefab, UIRoot.transform);
            if(createdUI != null)
            {
                _createdUIDic.Add(uiType, createdUI);
            }
        }
    }

    private string GetUIPath(UIType uiType)
    {
        string path = string.Empty;
        switch (uiType)
        {
            case UIType.Signup:
                path = "Prefabs/UI/UI_Signup";
                break;
            case UIType.Login:
                path = "Prefabs/UI/UI_Login";
                break;
            case UIType.Lobby:
                path = "Prefabs/UI/UI_Lobby";
                break;
            case UIType.SetPlayerNumPopup:
                path = "Prefabs/UI/Popup_SetPlayerNum";
                break;
            case UIType.Room:
                path = "Prefabs/UI/UI_Room";
                break;
            case UIType.Battle:
                path = "Prefabs/UI/Battle/UI_Battle";
                break;
        }
        return path;
    }

    public GameObject GetCreatedUI(UIType uiType)
    {
        if(_createdUIDic.ContainsKey(uiType) == false)
        {
            CreateUI(uiType);
        }
        return _createdUIDic[uiType];
    }

    private void OpenUI(UIType uiType, GameObject uiObject)
    {
        if(_openedUIDic.Contains(uiType) == false)
        {
            uiObject.SetActive(true);
            _openedUIDic.Add(uiType);
        }
    }

    private void CloseUI(UIType uiType)
    {
        if( _openedUIDic.Contains(uiType) )
        {
            var uiObject = _createdUIDic[uiType];
            uiObject.SetActive(false);
            _openedUIDic.Remove(uiType);
        }
    }

    public void OpenSpecificUI(UIType uiType)
    {
        var uiObj = GetCreatedUI(uiType);

        if (uiObj != null)
        {
            OpenUI(uiType, uiObj);
        }
    }

    public void CloseSpecificUI(UIType uiType)
    {
        CloseUI(uiType);
    }
}
