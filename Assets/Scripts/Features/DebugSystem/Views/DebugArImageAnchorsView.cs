using Features.Ar.Data;
using UnityEngine;

namespace Features.UI.View
{
    public class DebugArImageAnchorsView : MonoBehaviour
    {
        [SerializeField] private DebugArImageAnchorView[] _anchorViews;

        public bool TryGetArAnchorPosition(string imageName, out PositionData positionData)
        {
            positionData = new PositionData();
            foreach (var anchor in _anchorViews)
            {
                if (!anchor.ImageName.Contains(imageName)) continue;
                positionData.Position = anchor.Transform.position;
                positionData.Rotation = anchor.Transform.rotation;
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            try
            {
                Destroy(gameObject);
            }
            catch
            {
                // ignored
            }
        }
    }
}