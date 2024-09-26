using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.U2D;

public class BlenderButton : MonoBehaviour
{
    public static Action StartBlender;
    [SerializeField] private Sprite enabledSprite;
    [SerializeField] private Sprite disabledSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int enabledAfterId = -1;
    private bool isEnabled;
    private bool canPress;

    private void OnEnable()
    {
        Container.CompletedStage += OnCompletedStage;
        BlenderAnimation.OnEndBlending += OnEndBlend;
    }

    private void OnDisable()
    {
        BlenderAnimation.OnEndBlending -= OnEndBlend;
        Container.CompletedStage -= OnCompletedStage;    
    }

    private void OnEndBlend()
    {
        spriteRenderer.sprite = disabledSprite;
    }  

    private void OnCompletedStage(int id)
    {
        if(id == enabledAfterId)
        {
            canPress = true;
        }
    }

    private void OnMouseDown()
    {   
        if(!canPress) return;
        
        if(!isEnabled)
        {
            Debug.Log($"Click button");
            StartBlender?.Invoke();
            spriteRenderer.sprite = enabledSprite;
            isEnabled = true;
        }
    }
}