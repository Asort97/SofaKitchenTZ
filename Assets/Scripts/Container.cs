using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Container : MonoBehaviour
{
    public static Action<int> CompletedStage;
    [Serializable]
    public struct Skin
    {
        public string type;
        public GameObject skinObject;
        public bool IsInstantiate;
        public Transform parent;
        public Vector2 position;
    }

    [Serializable]
    public class Stage
    {
        public List<Item> completedItem = new();
        public Item[] requireItems;
        public Skin[] skinToActivate;
        public bool isCompleted;
    }

    public Stage[] Stages;
    public int CurrentStageIndex;

    
    public void InitRequireItems()
    {
        var stage = Stages[CurrentStageIndex];
        foreach (var requireItem in stage.requireItems)
        {
            requireItem.IsDraggable = true;
        }
    }

    public void AddToContainer(Item item)
    {
        var stage = Stages[CurrentStageIndex];

        if(IsContainItem(item))
        {
            stage.completedItem.Add(item);
            OnAddItem(item);

            if(stage.completedItem.Count == stage.requireItems.Length)
            {
                stage.isCompleted = true;
                CurrentStageIndex = Mathf.Clamp(CurrentStageIndex+1, 0, Stages.Length-1);
                CompletedStage?.Invoke(CurrentStageIndex);
                InitRequireItems();
            }
        }
    }  

    public abstract void OnAddItem(Item item);

    public bool IsContainItem(Item item)
    {
        var stage = Stages[CurrentStageIndex];
        return stage.requireItems.Contains(item);
    }

}