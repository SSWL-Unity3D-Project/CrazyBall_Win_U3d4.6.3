using UnityEngine;

/// <summary>
/// 游戏总控制组件
/// </summary>
public class SSGameMange : SSGameMono
{
    public class CleanupData
    {
        /// <summary>
        /// 道具父级
        /// </summary>
        internal Transform DaoJuParent;
        internal void Init()
        {
            GameObject objMission = new GameObject("MissionCleanup");
            GameObject objDaoJu = new GameObject("DaoJuParent");
            DaoJuParent = objDaoJu.transform;
            DaoJuParent.SetParent(objMission.transform);
        }
    }
    internal CleanupData m_CleanupData;

    static SSGameMange _Instance;
    public static SSGameMange GetInstance()
    {
        return _Instance;
    }

    // Use this for initialization
    void Start()
    {
        if (_Instance == null)
        {
            _Instance = this;
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    void Init()
    {
        SetGameResolution();
        InitCleanupData();
        CreateGameScene();
        CreateGameUI();
    }

    void SetGameResolution()
    {
        Screen.showCursor = false;
        Screen.SetResolution(1360, 768, true);
    }

    void InitCleanupData()
    {
        if (m_CleanupData == null)
        {
            m_CleanupData = new CleanupData();
        }

        if (m_CleanupData != null)
        {
            m_CleanupData.Init();
        }
    }

    /// <summary>
    /// 游戏场景控制组件
    /// </summary>
    internal SSGameScene m_SSGameScene;
    /// <summary>
    /// 创建游戏场景
    /// </summary>
    void CreateGameScene()
    {
        string prefabPath = "GameScence/Scene01";
        GameObject gmDataPrefab = (GameObject)Resources.Load(prefabPath);
        if (gmDataPrefab != null)
        {
            //SSDebug.Log("CreateGameScene......................................................");
            GameObject obj = (GameObject)Instantiate(gmDataPrefab);
            SSGameScene com = obj.GetComponent<SSGameScene>();
            if (com != null)
            {
                m_SSGameScene = com;
                com.Init();
            }
        }
        else
        {
            SSDebug.LogWarning("CreateGameScene -> gmDataPrefab was null! prefabPath == " + prefabPath);
        }
    }

    /// <summary>
    /// 游戏UI管理组件
    /// </summary>
    internal SSGameUI m_SSGameUI;
    /// <summary>
    /// 创建游戏UI
    /// </summary>
    void CreateGameUI()
    {
        string prefabPath = "GUI/GameUI/GameUI";
        GameObject gmDataPrefab = (GameObject)Resources.Load(prefabPath);
        if (gmDataPrefab != null)
        {
            //SSDebug.Log("CreateGameUI......................................................");
            GameObject obj = (GameObject)Instantiate(gmDataPrefab);
            SSGameUI com = obj.GetComponent<SSGameUI>();
            if (com != null)
            {
                m_SSGameUI = com;
                com.Init();
            }
        }
        else
        {
            SSDebug.LogWarning("CreateGameUI -> gmDataPrefab was null! prefabPath == " + prefabPath);
        }
    }
}
