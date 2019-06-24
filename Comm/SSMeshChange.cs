using UnityEngine;

public class SSMeshChange : MonoBehaviour
{
    public enum MeshState
    {
        oldMesh = 0,
        newMesh = 1,
    }

    [System.Serializable]
    public class SkinnedMeshData
    {
        public Material oldMesh;
        public Material newMesh;
        public SkinnedMeshRenderer meshRender;
        internal void ChangeMeshRender(MeshState type)
        {
            Material mat = null;
            switch (type)
            {
                case MeshState.oldMesh:
                    {
                        mat = oldMesh;
                        break;
                    }
                case MeshState.newMesh:
                    {
                        mat = newMesh;
                        break;
                    }
            }
            ChangeMeshRender(mat);
        }

        internal void ChangeMeshRender(Material mat)
        {
            if (mat != null && meshRender != null)
            {
                meshRender.material = mat;
            }
        }
    }

    [System.Serializable]
    public class MeshRenderData
    {
        public Material oldMesh;
        public Material newMesh;
        public MeshRenderer meshRender;
        internal void ChangeMeshRender(MeshState type)
        {
            Material mat = null;
            switch (type)
            {
                case MeshState.oldMesh:
                    {
                        mat = oldMesh;
                        break;
                    }
                case MeshState.newMesh:
                    {
                        mat = newMesh;
                        break;
                    }
            }
            ChangeMeshRender(mat);
        }

        internal void ChangeMeshRender(Material mat)
        {
            if (mat != null && meshRender != null)
            {
                meshRender.material = mat;
            }
        }
    }

    [System.Serializable]
    public class MeshData
    {
        public SkinnedMeshData skinnedMeshData;
        public MeshRenderData meshRenderData;
        internal void ChangeMeshRender(MeshState type)
        {
            if (skinnedMeshData != null)
            {
                skinnedMeshData.ChangeMeshRender(type);
            }

            if (meshRenderData != null)
            {
                meshRenderData.ChangeMeshRender(type);
            }
        }

        internal void ChangeMeshRender(Material mat)
        {
            if (skinnedMeshData != null)
            {
                skinnedMeshData.ChangeMeshRender(mat);
            }

            if (meshRenderData != null)
            {
                meshRenderData.ChangeMeshRender(mat);
            }
        }
    }
    public MeshData m_MeshData;
    
    internal void ChangeMeshRender(MeshState type)
    {
        if (m_MeshData != null)
        {
            m_MeshData.ChangeMeshRender(type);
        }
    }

    internal void ChangeMeshRender(Material mat)
    {
        if (m_MeshData != null)
        {
            m_MeshData.ChangeMeshRender(mat);
        }
    }
}
