using UnityEngine;
using System.Collections;

public class SSDengDaiDuiFang : SSGameMono
{
    GameObject m_DengDaiObj;
    /// <summary>
    /// 创建等待对方开始游戏UI
    /// </summary>
    internal void CreateDengDaiDuiFangUI(SSGlobalData.PlayerEnum indexPlayer, Transform par)
    {
        if (m_DengDaiObj != null)
        {
            return;
        }

        int index = (int)indexPlayer + 1;
        string prefabPath = "GUI/DengDai/DengDaiDuiFangP" + index;
        GameObject gmDataPrefab = (GameObject)Resources.Load(prefabPath);
        if (gmDataPrefab != null)
        {
            SSDebug.Log("CreateDengDaiDuiFangUI......................................................");
            m_DengDaiObj = (GameObject)Instantiate(gmDataPrefab, par);
        }
        else
        {
            SSDebug.LogWarning("CreateDengDaiDuiFangUI -> gmDataPrefab was null! prefabPath == " + prefabPath);
        }
    }

    internal void RemoveDengDaiDuiFang()
    {
        if (m_DengDaiObj != null)
        {
            Destroy(m_DengDaiObj);
        }
    }
}
