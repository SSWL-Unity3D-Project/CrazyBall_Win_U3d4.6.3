using UnityEngine;

public class SSWeiDangMesh : MonoBehaviour
{
    public enum WeiDangEnum
    {
        /// <summary>
        /// 伴随游戏难度改变围挡材质
        /// </summary>
        MeshChange = 0,
        /// <summary>
        /// 曲棍球碰上围挡时播放围挡的材质闪烁动画
        /// </summary>
        Flash = 1,
    }
    /// <summary>
    /// 围挡材质控制
    /// </summary>
    WeiDangEnum m_WeiDangEnum = WeiDangEnum.Flash;

    [System.Serializable]
    public class WeiDangMeshData
    {
        public SSMeshChange ssmeshChange;
        public Material[] materialArray = new Material[3];
        internal void SetMeshMaterial(int index)
        {
            if (index < 0 || index >= materialArray.Length)
            {
                return;
            }

            if (ssmeshChange != null)
            {
                ssmeshChange.ChangeMeshRender(materialArray[index]);
            }
        }
    }
    public WeiDangMeshData[] m_WeiDangMeshDataArray;

    [System.Serializable]
    public class MeshAniData
    {
        /// <summary>
        /// 动画播放器
        /// </summary>
        public Animator animator;
        internal void PlayAni()
        {
            if (animator != null)
            {
                animator.SetTrigger("IsPlay");
            }
        }
    }
    /// <summary>
    /// 材质动画数据
    /// </summary>
    public MeshAniData m_MeshAniData;

    /// <summary>
    /// 设置围挡材质
    /// </summary>
    internal void SetMeshMaterial(int index)
    {
        if (m_WeiDangEnum != WeiDangEnum.MeshChange)
        {
            return;
        }

        if (m_WeiDangMeshDataArray.Length <= 0)
        {
            return;
        }

        for (int i = 0; i < m_WeiDangMeshDataArray.Length; i++)
        {
            if (m_WeiDangMeshDataArray[i] != null)
            {
                m_WeiDangMeshDataArray[i].SetMeshMaterial(index);
            }
        }
    }

    /// <summary>
    /// 播放围挡材质动画
    /// </summary>
    internal void PlayWeiDangAni()
    {
        if (m_WeiDangEnum != WeiDangEnum.Flash)
        {
            return;
        }

        if (m_MeshAniData != null)
        {
            m_MeshAniData.PlayAni();
        }
    }
}
