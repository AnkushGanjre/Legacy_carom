using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StrickerControllerScript : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Slider strikerSlider;
    [SerializeField] private GameObject forcePointerPrefab; // Prefab of the force pointer sprite
    [SerializeField] private float forceMultiplier;
    
    private GameObject forcePointer; // Instance of the force pointer sprite
    private Vector2 initialPosition; // Initial position of the object being dragged
    private Quaternion targetRotation;
    private Vector2 direction;
    private Rigidbody2D rb;
    private float scaleValue;
    private float forceAmount;
    public bool strikerInAction;
    [SerializeField] float pointerScaleLimit = 100f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        strikerSlider.onValueChanged.AddListener(StrickerSlidePos);
    }
    private void Update()
    {
        if (strikerInAction)
        {
            if (rb.velocity.magnitude < 2f)
            {
                gameObject.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                gameObject.transform.GetComponent<Rigidbody2D>().angularVelocity = 0;
                gameObject.transform.rotation = Quaternion.identity;
                gameObject.transform.localPosition = Vector3.zero;
                strikerSlider.interactable = true;
                strikerSlider.value = 0f;
                strikerInAction = false;
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Create the force pointer sprite
        forcePointer = Instantiate(forcePointerPrefab, gameObject.transform);
        initialPosition = transform.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (forcePointer != null)
        {
            // Setting the size of forcePointer
            scaleValue = Vector2.Distance(transform.position, eventData.position);
            if (scaleValue > pointerScaleLimit)
            {
                scaleValue = pointerScaleLimit;
            }
            forcePointer.transform.localScale = new Vector3 (scaleValue, scaleValue, scaleValue);

            // Calculate the direction of dragging
            Vector3 dragDirection = initialPosition - eventData.position;

            // Calculate the angle of the drag direction
            float anglek = Mathf.Atan2(dragDirection.y, dragDirection.x) * Mathf.Rad2Deg;
            anglek = anglek + 180;

            if (anglek > 345 || anglek < 90)
            {
                anglek = 345;
            }
            if (anglek < 195)
            {
                anglek = 195;
            }
            // Rotate gameObject on Z-axis with given degree
            targetRotation = Quaternion.Euler(0f, 0f, anglek + 90);
            forcePointer.transform.rotation = targetRotation;

            // Calculate the direction of the drag
            float radians = anglek * Mathf.Deg2Rad;
            float radius = 50f;
            float x = radius * Mathf.Cos(radians);
            float y = radius * Mathf.Sin(radians);
            direction = new Vector2(x, y);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        forceAmount = Vector2.Distance(transform.position, eventData.position);
        if (forceAmount > scaleValue)
        {
            forceAmount = scaleValue;
        }
        if (forceAmount > 75)
        {
            forceAmount = forceAmount * 2.25f;
        }
        if (forceAmount > 50)
        {
            forceAmount = forceAmount * 1.75f;
        }
        if (forceAmount > 25)
        {
            forceAmount = forceAmount * 1.25f;
        }
        forceAmount = forceAmount * 25;
        Debug.Log("Strike Force: " + forceAmount);

        // Normalize the direction vector to ensure consistent speed
        Vector3 normalizedDirection = -direction;

        // Apply the force to the rigidbody
        rb.AddForce(normalizedDirection * forceAmount * forceMultiplier);

        // Disabling Slider
        strikerSlider.interactable = false;

        // Destroy the force pointer prefab
        Destroy(forcePointer);

        StartCoroutine(SetBoolTrue());
    }
    private IEnumerator SetBoolTrue()
    {
        yield return new WaitForSeconds(1);
        strikerInAction = true;
    }
    private void StrickerSlidePos(float value)
    {
        float adjustment = 541.5f;
        // Keep the current Y and Z positions of the target object
        Vector3 currentPosition = gameObject.transform.position;
        gameObject.transform.position = new Vector3(value + adjustment, currentPosition.y, currentPosition.z);
    }
    public void ForceZero()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
    }
}
