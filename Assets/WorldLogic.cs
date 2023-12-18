using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldLogic : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private ClassicModeLogic.World world;
    [SerializeField] private ClassicModeLogic logic;

    public void OnPointerEnter(PointerEventData eventData)
    {
        logic.SetWorld(world);
    }
}
