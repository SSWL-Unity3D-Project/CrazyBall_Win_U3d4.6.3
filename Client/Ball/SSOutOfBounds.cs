using UnityEngine;

public class SSOutOfBounds : MonoBehaviour
{
    public SSGlobalData.PlayerEnum IndexPlayer = SSGlobalData.PlayerEnum.Null;
    void OnTriggerEnter(Collider col)
    {
        ballOut(col);
    }

    void ballOut(Collider col)
    {
        if (IndexPlayer == SSGlobalData.PlayerEnum.Null)
        {
            return;
        }

        //创建开始发球界面
        if (SSGameMange.GetInstance() != null && SSGameMange.GetInstance().m_SSGameUI != null)
        {
            SSGameMange.GetInstance().m_SSGameUI.CreateStartFireBall(IndexPlayer);
        }

        //玩家没有接上曲棍球
        //把曲棍球初始化到玩家的球拍处
        SSBall ball = col.gameObject.GetComponent<SSBall>();
        if (ball)
        {
            ball.SetBallPosition(IndexPlayer);
        }
    }
}
