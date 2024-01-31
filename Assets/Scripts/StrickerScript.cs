using UnityEngine;
using UnityEngine.EventSystems;

public class StrickerScript : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    private RectTransform parentRectTransform;
    private RectTransform currentRectTransform;
    private Vector2 initialPosition;
    private Vector3 launchStartPosition;
    private Rigidbody2D rb;

    private Collider2D boardCollider;
    [SerializeField] private Transform playArea;

    private void Awake()
    {
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
        currentRectTransform = GetComponent<RectTransform>();
        boardCollider = playArea.GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPosition = currentRectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, eventData.position, eventData.pressEventCamera, out localPointerPosition))
        {
            if (parentRectTransform.rect.Contains(localPointerPosition))
            {
                // Pointer is within parent's transform
                
                float newX = Mathf.Clamp(localPointerPosition.x, parentRectTransform.rect.min.x, parentRectTransform.rect.max.x);
                currentRectTransform.anchoredPosition = new Vector2(newX, initialPosition.y);
                launchStartPosition = currentRectTransform.anchoredPosition;
            }
            else
            {
                // Pointer is outside parent's transform
                currentRectTransform.anchoredPosition = launchStartPosition;
                rb.isKinematic = true;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 dragEndPosition = eventData.position;
        float pullBackDistance = Vector3.Distance(launchStartPosition, dragEndPosition);

        // Calculate the launch force
        float launchForce = pullBackDistance * 5f;

        // Calculate the launch direction (opposite of the drag direction)
        Vector3 launchDirection = -(dragEndPosition - launchStartPosition).normalized;

        // Apply the launch force to the rigidbody
        rb.isKinematic = false;
        rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
    }
}
