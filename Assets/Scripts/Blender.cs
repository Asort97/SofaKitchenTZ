using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class Blender : Container
{
    [SerializeField] private GameObject blender_idle;
    [SerializeField] private GameObject blender_anim;
    private List<GameObject> spawned = new();
    private Collider2D boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        InitRequireItems();
    }

    private void OnEnable()
    {
        BlenderButton.StartBlender += OnStartBlender;
        BlenderAnimation.OnEndBlending += OnEndBlend;
    }

    private void OnDisable() 
    {
        BlenderAnimation.OnEndBlending -= OnEndBlend;
        BlenderButton.StartBlender -= OnStartBlender;
    }

    private void OnEndBlend()
    {
        boxCollider.enabled = false;
        CurrentStageIndex = Mathf.Clamp(CurrentStageIndex+1, 0, Stages.Length-1);
        InitRequireItems();
    }

    private void OnStartBlender()
    {
        blender_idle.SetActive(false);
        blender_anim.SetActive(true);
    }

    public override void OnAddItem(Item item)
    {
        var stage = Stages[CurrentStageIndex];

        foreach (var skin in stage.skinToActivate)
        {
            if(skin.type == item.GetItemType())
            {
                if(skin.IsInstantiate)
                {
                    if(spawned.Count == 0)
                    {
                        GameObject spawnedItem = Instantiate(skin.skinObject, skin.position, Quaternion.identity, skin.parent);
                        spawned.Add(spawnedItem);
                    }
                    else
                    {
                        Vector2 lastSpawnedPosition = spawned[spawned.Count-1].transform.position;
                        GameObject spawnedItem = Instantiate(skin.skinObject, new Vector2(lastSpawnedPosition.x, lastSpawnedPosition.y+0.5f), Quaternion.identity, skin.parent);
                        spawned.Add(spawnedItem);
                    }
                }
                else
                {
                    skin.skinObject.SetActive(true);
                }
            } 
        }
    }
}
