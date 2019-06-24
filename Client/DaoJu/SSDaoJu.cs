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
    }
    public DaoJuData m_DaoJuData;

    void OnTriggerEnter(Collider col)
    {
        CheckAddFenShuToPlayer(col.gameObject);
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
                RemoveSelf();
            }
        }
    }
}
