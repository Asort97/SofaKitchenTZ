using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string type;
    [SerializeField] private Sprite textureWhileDragging;
    [SerializeField] private Sprite textureAfterUse;
    [SerializeField] private bool disableAfterUse;
    [SerializeField] private bool isReusable;
    private SpriteRenderer spriteRenderer;
    private Sprite defaultTexture;
    private Vector3 defaultPosition;
    private int defaultSortingOrder;
    private bool isOverContainer;
    private Container container;
    private bool isUsed;
    public bool IsDraggable;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultTexture = spriteRenderer.sprite;
        defaultSortingOrder = spriteRenderer.sortingOrder;
        defaultPosition = transform.position;
    }

    public string GetItemType()
    {
        return type;
    } 

    private void OnMouseDrag()
    {
        if(!IsDraggable) return;
        if(isUsed) return;

        spriteRenderer.sortingOrder = 10;
        if(textureWhileDragging) spriteRenderer.sprite = textureWhileDragging;
        transform.position = Vector2.Lerp(transform.position, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), 15f * Time.deltaTime);
    }

    private void OnMouseUp()
    {
        if(!IsDraggable) return;
        if(isUsed) return;

        spriteRenderer.sortingOrder = defaultSortingOrder;
        spriteRenderer.sprite = defaultTexture;
        transform.position = defaultPosition;

        if(isOverContainer)
        {   
            container.AddToContainer(this);
            Used();
        }
    }

    private void Used()
    {
        if(!isReusable)
        {
            isUsed = true;
        }

        if(textureAfterUse) spriteRenderer.sprite = textureAfterUse;
        if(disableAfterUse) gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent(out Container container))
        {
            this.container = container;
            isOverContainer = true;
        }        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.TryGetComponent(out Container container))
        {
            this.container = null;
            isOverContainer = false;
        }               
    }
}