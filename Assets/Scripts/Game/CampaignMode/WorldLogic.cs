using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.CampaignMode
{
    public class WorldLogic : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private CampaignModeLogic.World world;
        [SerializeField] private CampaignModeLogic logic;

        public void OnPointerEnter(PointerEventData eventData)
        {
            logic.TrySetWorld(world);
        }
    }
}
