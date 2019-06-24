using UnityEngine;

public class SSWeiDangMesh : MonoBehaviour
{
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

    internal void SetMeshMaterial(int index)
    {
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
}
