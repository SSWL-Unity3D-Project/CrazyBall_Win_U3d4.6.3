using System.Collections;
using UnityEngine;

public class SSGameDaoJiShi : MonoBehaviour
{
    [System.Serializable]
    public class DaoJiShiData
    {
        /// <summary>
        /// 倒计时的分钟
        /// </summary>
        public SSGameNumUI m_TimeFen;
        /// <summary>
        /// 倒计时的秒数
        /// </summary>
        public SSGameNumUI m_TimeMiao;
        /// <summary>
        /// 10秒之后的倒计时UI图集
        /// </summary>
        public Sprite[] TeShuTimeNumArray = new Sprite[10];
        /// <summary>
        /// 原先倒计时UI图集
        /// </summary>
        Sprite[] OldTimeNumArray;
        /// <summary>
        /// 游戏倒计时
        /// </summary>
        public int Time = 180;
        int DaoJiShi = 0;
        /// <summary>
        /// 倒计时是否结束播放
        /// </summary>
        internal bool IsEndDaoJiShi = false;
        public DaoJiShiData()
        {
            DaoJiShi = Time;
        }

        internal void Init()
        {
            if (m_TimeFen != null)
            {
                OldTimeNumArray = m_TimeFen.GetNumSpriteArray();
            }
            ShowDaoJiShi();
        }

        internal void DownTime()
        {
            if (DaoJiShi <= 0)
            {
                IsEndDaoJiShi = true;
                //倒计时结束
                //重置曲棍球位置
                if (SSGameMange.GetInstance() != null
                    && SSGameMange.GetInstance().m_SSGameScene != null
                    && SSGameMange.GetInstance().m_SSGameScene.m_SSBall != null)
                {
                    SSGameMange.GetInstance().m_SSGameScene.m_SSBall.ResetInfo();
                }
                return;
            }

            DaoJiShi--;
            ShowDaoJiShi();
        }

        void ShowDaoJiShi()
        {
            //SSDebug.Log("ShowDaoJiShi -> DaoJiShi ======== " + DaoJiShi);
            CheckChangeTimeTuJi();
            //此处添加显示倒计时代码
            int fenVal = DaoJiShi / 60;
            int miaoVal = DaoJiShi % 60;
            if (m_TimeFen != null)
            {
                m_TimeFen.ShowNumUI(fenVal);
            }

            if (m_TimeMiao != null)
            {
                m_TimeMiao.ShowNumUI(miaoVal);
            }
        }

        bool IsChangeTimeTuJi = false;
        /// <summary>
        /// 检测是否需要更换倒计时数字图集
        /// </summary>
        void CheckChangeTimeTuJi()
        {
            if (DaoJiShi <= 10 && IsChangeTimeTuJi == false)
            {
                IsChangeTimeTuJi = true;
                if (m_TimeFen != null)
                {
                    m_TimeFen.ChangeNumSpriteArray(TeShuTimeNumArray);
                }

                if (m_TimeMiao != null)
                {
                    m_TimeMiao.ChangeNumSpriteArray(TeShuTimeNumArray);
                }
            }
            else if (DaoJiShi > 10 && IsChangeTimeTuJi == true)
            {
                IsChangeTimeTuJi = false;
                if (m_TimeFen != null)
                {
                    m_TimeFen.ChangeNumSpriteArray(OldTimeNumArray);
                }

                if (m_TimeMiao != null)
                {
                    m_TimeMiao.ChangeNumSpriteArray(OldTimeNumArray);
                }
            }
        }
    }
    public DaoJiShiData m_DaoJiShiData;

    internal void Init()
    {
        StartCoroutine(LoopDownDaoJiShi());
    }

    void SetDaoJiShi()
    {
        if (m_DaoJiShiData != null)
        {
            m_DaoJiShiData.DownTime();
        }
    }

    IEnumerator LoopDownDaoJiShi()
    {
        if (m_DaoJiShiData == null)
        {
            yield break;
        }

        if (SSGameMange.GetInstance() == null)
        {
            yield break;
        }

        if (SSGameMange.GetInstance().m_SSGameUI == null)
        {
            yield break;
        }

        m_DaoJiShiData.Init();
        yield return new WaitForSeconds(1f);

        do
        {
            if (SSGameMange.GetInstance().m_SSGameUI.IsCreateStartFireBall == true)
            {
                //创建了开始发球界面,倒计时停止
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            SetDaoJiShi();
            if (m_DaoJiShiData.IsEndDaoJiShi == false)
            {
                yield return new WaitForSeconds(1f);
            }
        }
        while (m_DaoJiShiData.IsEndDaoJiShi == false);
    }

    private void Update()
    {
        if (Time.frameCount % 10 == 0)
        {
            if (m_DaoJiShiData != null && m_DaoJiShiData.IsEndDaoJiShi == true)
            {
                //删除倒计时
                RemoveSelf();
            }
        }
    }

    bool IsRemoveSelf = false;
    void RemoveSelf()
    {
        if (SSGameMange.GetInstance() == null
            || SSGameMange.GetInstance().m_SSGameUI == null
            || SSGameMange.GetInstance().m_SSGameScene == null)
        {
            return;
        }

        if (IsRemoveSelf == false)
        {
            IsRemoveSelf = true;
            Destroy(gameObject);
            SSGameMange.GetInstance().m_SSGameUI.ResetInfo();
            SSGameMange.GetInstance().m_SSGameUI.RemoveGameDaoJiShi();
            //游戏结束
            SSGameMange.GetInstance().m_SSGameScene.OnGameOver();

            //展示玩家获胜/失败/平局UI界面
            SSGameMange.GetInstance().m_SSGameUI.ShowPlayerGameRaceResult();
        }
    }
}
