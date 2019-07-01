using UnityEngine;

public class SSPlayerPaddle : MonoBehaviour
{
    SSGameScene.PaddleData m_PaddleData;
    internal SSGlobalData.PlayerEnum IndexPlayer = SSGlobalData.PlayerEnum.Null;
    // Use this for initialization
    internal void Init(SSGameScene.PaddleData dt, SSGlobalData.PlayerEnum indexPlayer)
    {
        m_PaddleData = dt;
        IndexPlayer = indexPlayer;

        switch (indexPlayer)
        {
            case SSGlobalData.PlayerEnum.PlayerOne:
                {
                    InputEventCtrl.GetInstance().ClickStartBtOneEvent += ClickStartBtOneEvent;
                    break;
                }
            case SSGlobalData.PlayerEnum.PlayerTwo:
                {
                    InputEventCtrl.GetInstance().ClickStartBtTwoEvent += ClickStartBtTwoEvent;
                    break;
                }
        }
    }

    private void ClickStartBtOneEvent(InputEventCtrl.ButtonState val)
    {
        OnClickFireBt(val);
    }

    private void ClickStartBtTwoEvent(InputEventCtrl.ButtonState val)
    {
        OnClickFireBt(val);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LineUpdate();
    }
    
    void LineUpdate()
    {
        //if the horizontal button is pressed move our paddle using velocity, if not set its movement to 0.
        float input = GetInput();
        bool isMovePaddle = GetIsMovePaddle(input);
        if (input != 0f)
        {
            if (isMovePaddle == true)
            {
                rigidbody.velocity = new Vector3(0f, 0f, input * m_PaddleData.moveSpeed);
            }
        }
        else
        {
            rigidbody.velocity = Vector3.zero;
        }
    }

    bool GetIsMovePaddle(float input)
    {
        bool isMovePaddle = true;
        Vector3 pos = transform.position;
        float posZ = pos.z;
        if (input > 0f)
        {
            if (posZ >= m_PaddleData.maxPos)
            {
                if (posZ > m_PaddleData.maxPos)
                {
                    pos.z = m_PaddleData.maxPos;
                    transform.position = pos;
                }
                isMovePaddle = false;
            }
        }
        else if (input < 0f)
        {
            if (posZ <= m_PaddleData.minPos)
            {
                if (posZ < m_PaddleData.minPos)
                {
                    pos.z = m_PaddleData.minPos;
                    transform.position = pos;
                }
                isMovePaddle = false;
            }
        }
        else
        {
            if (posZ >= m_PaddleData.maxPos)
            {
                if (posZ > m_PaddleData.maxPos)
                {
                    pos.z = m_PaddleData.maxPos;
                    transform.position = pos;
                }
                isMovePaddle = false;
            }
            else if (posZ <= m_PaddleData.minPos)
            {
                if (posZ < m_PaddleData.minPos)
                {
                    pos.z = m_PaddleData.minPos;
                    transform.position = pos;
                }
                isMovePaddle = false;
            }
        }
        return isMovePaddle;
    }

    float GetInput()
    {
        float hor = 0f;
        if (InputEventCtrl.GetInstance() != null)
        {
            hor = InputEventCtrl.GetInstance().GetPlayerDirVal(IndexPlayer);
            switch (IndexPlayer)
            {
                case SSGlobalData.PlayerEnum.PlayerOne:
                    {
                        //玩家1的方向反转
                        hor *= -1f;
                        break;
                    }
            }
        }
        return hor;
    }

    /// <summary>
    /// 曲棍球组件
    /// </summary>
    SSBall m_SSBall = null;
    /// <summary>
    /// 填充曲棍球
    /// </summary>
    internal void FillBall(float posZ, SSBall ballCom)
    {
        if (ballCom == null)
        {
            return;
        }

        m_SSBall = ballCom;
        //修改曲棍球的父级
        ballCom.transform.SetParent(transform);
        ballCom.transform.localPosition = new Vector3(0f, 0f, posZ);
    }

    void OnClickFireBt(InputEventCtrl.ButtonState val)
    {
        if (val == InputEventCtrl.ButtonState.UP)
        {
            return;
        }
        FireBall();
    }

    /// <summary>
    /// 发射曲棍球
    /// </summary>
    void FireBall()
    {
        if (SSGameMange.GetInstance() == null
            || SSGameMange.GetInstance().m_SSGameUI == null
            || SSGameMange.GetInstance().m_SSGameScene == null)
        {
            return;
        }

        if (SSGameMange.GetInstance().m_SSGameUI.m_GameUIData.IsGameStart == false)
        {
            //有玩家还没拍开始游戏按键
            return;
        }

        if (m_SSBall == null)
        {
            return;
        }

        m_SSBall.Fire(transform.forward);
        m_SSBall = null;

        //创建游戏倒计时界面
        SSGameMange.GetInstance().m_SSGameUI.CreateGameDaoJiShi();
        //触发玩家发球事件
        SSGameMange.GetInstance().m_SSGameScene.PlayerFireBallEvent();
    }
}
