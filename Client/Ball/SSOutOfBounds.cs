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

        if (SSGameMange.GetInstance() == null || SSGameMange.GetInstance().m_SSGameUI == null)
        {
            return;
        }
        //创建开始发球界面
        SSGameMange.GetInstance().m_SSGameUI.CreateStartFireBall(IndexPlayer);

        //玩家没有接上曲棍球
        //把曲棍球初始化到玩家的球拍处
        SSBall ball = col.gameObject.GetComponent<SSBall>();
        if (ball)
        {
            ball.SetBallPosition(IndexPlayer);
        }

        SSGlobalData.PlayerEnum indexJiaFenPlayer = SSGlobalData.PlayerEnum.Null;
        switch (IndexPlayer)
        {
            case SSGlobalData.PlayerEnum.PlayerOne:
                {
                    indexJiaFenPlayer = SSGlobalData.PlayerEnum.PlayerTwo;
                    break;
                }
            case SSGlobalData.PlayerEnum.PlayerTwo:
                {
                    indexJiaFenPlayer = SSGlobalData.PlayerEnum.PlayerOne;
                    break;
                }
        }

        if (indexJiaFenPlayer != SSGlobalData.PlayerEnum.Null)
        {
            //创建加分UI界面
            SSGameMange.GetInstance().m_SSGameUI.CreateJiaFenUI(indexJiaFenPlayer);
        }
    }
}
