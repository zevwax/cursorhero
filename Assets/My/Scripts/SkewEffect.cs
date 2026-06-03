using UnityEngine;
using UnityEngine.UI;

namespace ZevWaxGames.CursorHero
{
    [RequireComponent(typeof(RawImage))]
    public class SkewEffect : BaseMeshEffect
    {
        public float skewX = 4f;

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive()) return;

            int count = vh.currentVertCount;
            if (count == 0) return;

            UIVertex vertex = new UIVertex();

            for (int i = 0; i < count; i++)
            {
                vh.PopulateUIVertex(ref vertex, i);

                if (vertex.position.y > 0)
                {
                    vertex.position.x += skewX;
                }

                vh.SetUIVertex(vertex, i);
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (graphic != null) graphic.SetVerticesDirty();
        }
#endif
    }
}