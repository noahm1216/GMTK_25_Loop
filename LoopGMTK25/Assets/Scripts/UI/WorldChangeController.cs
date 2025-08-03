using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WorldChangeController : MonoBehaviour
{
    
    public delegate void WorldSwitchEvent();
    public static event WorldSwitchEvent OnWorldSwitchEvent;

    public GameObject buttonParent;
    public GameObject buttonPrefab;
    
    
    
    
    private List<string> _maskNames = new List<string>(){"Mask1", "Mask2", "Mask3", "Mask4", "Mask5", "Mask6"};
    private List<string> _physicsNames = new List<string>(){"PhysicsA", "PhysicsB", "PhysicsC", "PhysicsD", "PhysicsE", "PhysicsF"};
    public List<Color> colors = new List<Color>()
    {
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
        Color.cyan,
        Color.magenta,
    };
    public List<Sprite> icons = new List<Sprite>();
    

    private const float ButtonWidth = 50f;
    private const float ButtonPadding = 5f;

    private Region[] _regions;
    
    private Transform[] _pastOrder;
    private Transform[] _currentOrder;


    public static void RaiseWorldSwitchEvent()
    {
        OnWorldSwitchEvent?.Invoke();
    }

    private void OnEnable()
    {
        OnWorldSwitchEvent += WorldSwitch;
        
        AddButtons();

        _pastOrder = GetOrder();
    }
    
    private void OnDisable()
    {
        OnWorldSwitchEvent -= WorldSwitch;
    }

    private void WorldSwitch()
    {
        
        MultiWorldDividerController2D[] dividers = FindObjectsOfType<MultiWorldDividerController2D>();

        _currentOrder = GetOrder();
        
        foreach (var divider in dividers)
        {
            divider.planes = _currentOrder.ToList();
        }

        _pastOrder = GetOrder();
    }

    private void AddButtons()
    {
        _regions = FindObjectsOfType<Region>().OrderBy(x => x.transform.position.x).ToArray();
        
        float totalWidth = (ButtonWidth + ButtonPadding * 2) * _regions.Length;
        float step = totalWidth / _regions.Length;
        
        buttonParent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, totalWidth + ButtonPadding * 2);

        for (int i = 0; i < _regions.Length; i++)
        {
            GameObject prefab = Instantiate(buttonPrefab, buttonParent.transform);
            prefab.name = "Button"+i;
            RectTransform rectTransform = prefab.GetComponent<RectTransform>();
            Image spriteRenderer = prefab.GetComponent<Image>();
            spriteRenderer.color = colors[
                _maskNames.IndexOf(LayerMask.LayerToName(_regions[i].gameObject.layer))
            ];

            if (i < icons.Count && icons[i] != null)
            {
                spriteRenderer.sprite = icons[i];
            }

            rectTransform.anchoredPosition = new Vector2(i * step + step * .5f + ButtonPadding, -25);
            
            UIDraggable uiDraggable = prefab.GetComponent<UIDraggable>();
            uiDraggable.region = _regions[i];
            
        }
    }

    private Transform[] GetOrder()
    {
        return buttonParent.GetComponentsInChildren<UIDraggable>().
            OrderBy(x => x.GetComponent<RectTransform>().position.x).Select(x=>x.region.transform).ToArray();
    }
    
}
