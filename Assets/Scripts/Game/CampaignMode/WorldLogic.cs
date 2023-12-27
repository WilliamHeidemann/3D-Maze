using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.CampaignMode
{
    public class WorldLogic : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private World world;
        [SerializeField] private LevelSelectLogic logic;

        public void OnPointerEnter(PointerEventData eventData)
        {
            logic.TrySetWorld(world);
        }
    }
}
