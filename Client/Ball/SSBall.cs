using UnityEngine;

public class SSBall : MonoBehaviour
{
    [System.Serializable]
    public class BallData
    {
        /// <summary>
        /// 初始化到玩家球拍的z坐标偏差
        /// </summary>
        public float posOffsetZ = 8f;
        /// <summary>
        /// 曲棍球的初始速度
        /// </summary>
        public float ballSpeed = 20f;
        internal float ballSpeeding
        {
            get
            {
                float speed = ballSpeed;
                if (SSGameMange.GetInstance() != null
                    && SSGameMange.GetInstance().m_SSGameScene != null
                    && SSGameMange.GetInstance().m_SSGameScene.m_BallData != null)
                {
                    speed = SSGameMange.GetInstance().m_SSGameScene.m_BallData.moveSpeed;
                }
                return speed;
            }
        }
        /// <summary>
        /// 撞上碰撞物后反弹的最小角度
        /// </summary>
        internal float minBounceAngle = 25;
        /// <summary>
        /// 撞上碰撞物后反弹的最大角度
        /// </summary>
        internal float maxBounceAngle = 45;
        internal int badBounceLayer = 10;
        internal int m_badBounceCount = 0;
        internal int timesBeforeCorrectingBadBounce = 2;
        /// <summary>
        /// 是否运动
        /// </summary>
        internal bool isMove = false;
        internal SSGlobalData.PlayerEnum IndexPlayer = SSGlobalData.PlayerEnum.Null;
    }
    public BallData m_BallData;

    // Use this for initialization
    internal void Init()
    {
        InitPosition(SSGlobalData.PlayerEnum.PlayerOne);
    }

    internal void ResetInfo()
    {
        InitPosition(SSGlobalData.PlayerEnum.PlayerOne);
    }

    void InitPosition(SSGlobalData.PlayerEnum indexPlayer)
    {
        SetBallPosition(indexPlayer);
    }

    internal void SetBallPosition(SSGlobalData.PlayerEnum indexPlayer)
    {
        if (SSGameMange.GetInstance() == null || SSGameMange.GetInstance().m_SSGameScene == null)
        {
            return;
        }

        SSPlayerPaddle paddle = SSGameMange.GetInstance().m_SSGameScene.GetPlayerPaddle(indexPlayer);
        if (paddle != null)
        {
            m_BallData.isMove = false;
            rigidbody.isKinematic = true;
            paddle.FillBall(m_BallData.posOffsetZ, this);
        }
        SetBallPlayerIndex(indexPlayer);
    }

    /// <summary>
    /// 发射曲棍球
    /// </summary>
    public void Fire(Vector3 forward)
    {
        transform.SetParent(null);
        rigidbody.isKinematic = false;
        m_BallData.isMove = true;
        //m_BallData.ballSpeeding = m_BallData.ballSpeed;
        rigidbody.velocity = forward * m_BallData.ballSpeeding;
    }

    void Update()
    {
        if (m_BallData.isMove == false)
        {
            return;
        }
        moveAtConstantSpeed();
    }

    //move the ball at a constant speed
    void moveAtConstantSpeed()
    {
        //float mod = (m_hasHitPaddle == false) ? m_speedScalarBeforeHittingPaddle : 1f;
        Vector3 vec = rigidbody.velocity;
        vec.y = 0f;
        if (rigidbody.isKinematic == false)
        {
            rigidbody.velocity = vec.normalized * m_BallData.ballSpeeding;
        }
    }

    void OnCollisionExit(Collision col)
    {
        //SSDebug.Log("col == " + col.gameObject.layer);
        if (col.gameObject.layer == m_BallData.badBounceLayer)
        {
            m_BallData.m_badBounceCount++;
            if (m_BallData.m_badBounceCount > m_BallData.timesBeforeCorrectingBadBounce)
            {
                //SSDebug.Log("handle bad bounce!");
                handleBadBounce();
            }
        }
        else
        {
            m_BallData.m_badBounceCount = 0;
        }

        SSPlayerPaddle paddle = col.gameObject.GetComponent<SSPlayerPaddle>();
        if (paddle != null)
        {
            SetBallPlayerIndex(paddle.IndexPlayer);
        }
    }

    void SetBallPlayerIndex(SSGlobalData.PlayerEnum indexPlayer)
    {
        if (m_BallData != null)
        {
            m_BallData.IndexPlayer = indexPlayer;
            //SSDebug.Log("SetBallPlayerIndex -> indexPlayer ============= " + indexPlayer);
        }
    }

    internal SSGlobalData.PlayerEnum GetBallPlayerIndex()
    {
        if (m_BallData != null)
        {
            return m_BallData.IndexPlayer;
        }
        return SSGlobalData.PlayerEnum.Null;
    }

    void handleBadBounce()
    {
        if (rigidbody.isKinematic == true)
        {
            return;
        }

        Vector3 vel = rigidbody.velocity.normalized;
        float angleBounce = Random.Range(m_BallData.minBounceAngle, m_BallData.maxBounceAngle);
        //SSDebug.Log("handleBadBounce -> vel == " + vel);
        if (vel.z > 0.9f)
        {
            if (vel.x <= 0)
            {
                rigidbody.velocity = Quaternion.AngleAxis(-angleBounce, Vector3.up) * rigidbody.velocity;
            }
            else
            {
                rigidbody.velocity = Quaternion.AngleAxis(angleBounce, Vector3.up) * rigidbody.velocity;
            }
        }
        else if (vel.z < -0.9f)
        {
            if (vel.x <= 0)
            {
                rigidbody.velocity = Quaternion.AngleAxis(angleBounce, Vector3.up) * rigidbody.velocity;
            }
            else
            {
                rigidbody.velocity = Quaternion.AngleAxis(-angleBounce, Vector3.up) * rigidbody.velocity;
            }
        }
    }
}
