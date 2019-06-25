using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class SSDaoJu : SSGameMono
{
    [System.Serializable]
    public class DaoJuData
    {
        /// <summary>
        /// 玩家击出的曲棍球碰上道具后的得分
        /// </summary>
        public int FenShu = 10;
        /// <summary>
        /// 爆炸特效预制
        /// </summary>
        public GameObject TeXiaoPrefab;
    }
    public DaoJuData m_DaoJuData;

    void OnTriggerEnter(Collider col)
    {
        CheckAddFenShuToPlayer(col.gameObject);
    }

    internal void CreateTeXiao()
    {
        if (m_DaoJuData != null && m_DaoJuData.TeXiaoPrefab != null)
        {
            Transform trParent = SSGameMange.GetInstance().m_CleanupData.DaoJuParent;
            GameObject obj = (GameObject)Instantiate(m_DaoJuData.TeXiaoPrefab, trParent, transform);
            CheckDestroyThisTimed(obj);
        }
    }
    
    void CheckAddFenShuToPlayer(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }

        SSBall ball = obj.GetComponent<SSBall>();
        if (ball != null)
        {
            SSGlobalData.PlayerEnum indexPlayer = ball.GetBallPlayerIndex();
            if (indexPlayer != SSGlobalData.PlayerEnum.Null && m_DaoJuData != null)
            {
                SSGlobalData.GetInstance().AddPlayerFenShu(indexPlayer, m_DaoJuData.FenShu);
                if (SSGameMange.GetInstance() != null && SSGameMange.GetInstance().m_SSGameScene != null)
                {
                    SSGameMange.GetInstance().m_SSGameScene.RemoveDaoJu(gameObject);
                }

                if (SSGameMange.GetInstance() != null && SSGameMange.GetInstance().m_SSGameUI != null)
                {
                    SSGameMange.GetInstance().m_SSGameUI.CreateDaoJuJiaFenUI(indexPlayer, m_DaoJuData.FenShu);
                }
                CreateTeXiao();
                RemoveSelf();
            }
        }
    }
}
