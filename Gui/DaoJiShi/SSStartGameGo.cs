using UnityEngine;

public class SSStartGameGo : MonoBehaviour
{
    bool IsRemoveSelf = false;
    internal void Init()
    {
        if (SSGameMange.GetInstance() != null
            && SSGameMange.GetInstance().m_SSGameScene != null)
        {
            SSGameMange.GetInstance().m_SSGameScene.OnFireBallEvent += OnFireBallEvent;
        }
    }

    private void OnFireBallEvent()
    {
        if (SSGameMange.GetInstance() == null)
        {
            return;
        }

        if (SSGameMange.GetInstance().m_SSGameScene != null)
        {
            SSGameMange.GetInstance().m_SSGameScene.OnFireBallEvent -= OnFireBallEvent;
        }
        
        if (SSGameMange.GetInstance().m_SSGameUI != null)
        {
            SSGameMange.GetInstance().m_SSGameUI.RemoveStartGameGo();
            SSGameMange.GetInstance().m_SSGameUI.RemoveStartFireBall();
        }
        RemoveSelf();
    }

    void RemoveSelf()
    {
        if (IsRemoveSelf == false)
        {
            IsRemoveSelf = true;
            Destroy(gameObject);
        }
    }
}
