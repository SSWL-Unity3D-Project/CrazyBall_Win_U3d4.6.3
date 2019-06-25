using UnityEngine;
using System.Collections;
using System;

public class SSDaoJuManage : SSGameMono
{
    [Serializable]
    public class ManageData
    {
        //道具预制，道具产生点，道具生存时间。
        /// <summary>
        /// 道具预制的最大数量
        /// </summary>
        public int MaxDaoJuPrefab = 5;
        /// <summary>
        /// 道具生存时间
        /// </summary>
        public int LifeTime = 10;
        /// <summary>
        /// 道具间隔产生时间
        /// </summary>
        public SSGlobalData.MinMaxDataFloat TimeCreateDaoJu;
        /// <summary>
        /// 随机产生道具的数量
        /// </summary>
        public SSGlobalData.MinMaxDataInt RandomCreateDaoJu;
        /// <summary>
        /// 道具产生点组父级
        /// </summary>
        public Transform TrPointParent;
        /// <summary>
        /// 道具产生点数组
        /// </summary>
        internal Transform[] TrPointArray;
        internal void Init()
        {
            if (TrPointParent != null)
            {
                TrPointArray = TrPointParent.GetComponentsInChildren<Transform>();
                if (TrPointParent.childCount > 0)
                {
                    TrPointArray = new Transform[TrPointParent.childCount];
                    for (int i = 0; i < TrPointArray.Length; i++)
                    {
                        TrPointArray[i] = TrPointParent.GetChild(i);
                    }
                }
            }

            if (RandomCreateDaoJu.Max > TrPointArray.Length)
            {
                RandomCreateDaoJu.Max = TrPointArray.Length;
            }

            for (int i = 0; i < TrPointArray.Length; i++)
            {
                if (TrPointArray[i] != null && TrPointArray[i].gameObject.activeInHierarchy == true)
                {
                    TrPointArray[i].gameObject.SetActive(false);
                }
            }
        }
    }
    public ManageData m_ManageData;

    internal void Init()
    {
        if (m_ManageData != null)
        {
            m_ManageData.Init();
        }
    }

    internal void StartCreateDaoJu()
    {
        if (IsDelayStartCreateDaoJu == false)
        {
            //SSDebug.LogWarning("StartCreateDaoJu...................................");
            IsDelayStartCreateDaoJu = true;
            StartCoroutine(DelayStartCreateDaoJu());
        }
    }

    bool IsDelayStartCreateDaoJu = false;
    IEnumerator DelayStartCreateDaoJu()
    {
        if (m_ManageData == null)
        {
            yield break;
        }

        do
        {
            if (IsDelayStartCreateDaoJu == false)
            {
                yield break;
            }

            float time = m_ManageData.TimeCreateDaoJu.GetRandom();
            yield return new WaitForSeconds(time);
            if (IsDelayStartCreateDaoJu == false)
            {
                yield break;
            }
            //产生道具
            RandomCreateDaoJu();

            time = m_ManageData.LifeTime;
            bool isEmpty = false;
            for (int i = 0; i < time; i++)
            {
                if (IsDelayStartCreateDaoJu == false)
                {
                    yield break;
                }

                yield return new WaitForSeconds(1);
                isEmpty = m_ListDaoJuData.GetIsEmpty();
                if (isEmpty == true)
                {
                    //开始下一轮的道具产生
                    break;
                }
            }

            if (isEmpty == false)
            {
                CleanAllDaoJu();
            }
        }
        while (IsDelayStartCreateDaoJu == true);
        //SSDebug.LogWarning("End DelayStartCreateDaoJu....................");
    }

    internal void StopCreateDaoJu()
    {
        if (IsDelayStartCreateDaoJu == true)
        {
            IsDelayStartCreateDaoJu = false;
            StopCoroutine(DelayStartCreateDaoJu());
        }
        CleanAllDaoJu();
    }

    /// <summary>
    /// 道具数据链表
    /// </summary>
    SSGlobalData.ListObjData m_ListDaoJuData = new SSGlobalData.ListObjData();
    /// <summary>
    /// 添加道具
    /// </summary>
    void AddDaoJuToList(GameObject obj)
    {
        if (m_ListDaoJuData != null)
        {
            m_ListDaoJuData.AddObject(obj);
        }
    }

    /// <summary>
    /// 删除道具
    /// </summary>
    internal void RemoveDaoJuFromList(GameObject obj)
    {
        if (m_ListDaoJuData != null)
        {
            m_ListDaoJuData.RemoveObject(obj);
        }
    }

    /// <summary>
    /// 清除所有道具
    /// </summary>
    void CleanAllDaoJu()
    {
        if (m_ListDaoJuData != null)
        {
            GameObject[] objArray = m_ListDaoJuData.ToArray();
            m_ListDaoJuData.CleanList();
            for (int i = 0; i < objArray.Length; i++)
            {
                Destroy(objArray[i]);
            }
        }
    }

    /// <summary>
    /// 创建道具
    /// </summary>
    void RandomCreateDaoJu()
    {
        if (m_ManageData == null)
        {
            return;
        }

        int pointLength = m_ManageData.TrPointArray.Length;
        if (pointLength <= 0)
        {
            return;
        }

        int pointIndex = UnityEngine.Random.Range(0, m_ManageData.TrPointArray.Length);
        int max = m_ManageData.RandomCreateDaoJu.GetRandom();
        if (max < 1 || max > pointLength)
        {
            max = Mathf.Clamp(max, 1, pointLength);
        }

        for (int i = 0; i < max; i++)
        {
            int daoJuIndex = (UnityEngine.Random.Range(0, 100) % m_ManageData.MaxDaoJuPrefab) + 1;
            Transform tr = m_ManageData.TrPointArray[pointIndex % pointLength];
            pointIndex++;
            GameObject obj = CreateDaoJu(daoJuIndex, tr);
            AddDaoJuToList(obj);
        }
    }

    GameObject CreateDaoJu(int index, Transform tr)
    {
        GameObject obj = null;
        Transform parent = SSGameMange.GetInstance().m_CleanupData.DaoJuParent;
        string prefabPath = "DaoJu/DeFenDaoJu/DaoJu_" + index;
        GameObject gmDataPrefab = (GameObject)Resources.Load(prefabPath);
        if (gmDataPrefab != null)
        {
            //SSDebug.Log("CreateDaoJu.............. index == " + index);
            obj = (GameObject)Instantiate(gmDataPrefab, parent, tr);
        }
        else
        {
            SSDebug.LogWarning("CreateDaoJu -> gmDataPrefab was null! prefabPath == " + prefabPath);
        }
        return obj;
    }
}
