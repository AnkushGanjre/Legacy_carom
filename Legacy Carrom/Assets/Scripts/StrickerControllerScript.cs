using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StrickerControllerScript : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Slider p1StrikerSlider;
    [SerializeField] private Slider p2StrikerSlider;
    [SerializeField] private GameObject forcePointerPrefab; // Prefab of the force pointer sprite
    [SerializeField] private float forceMultiplier;
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;
    [SerializeField] private GameObject pocket;
    [SerializeField] private Transform p1PocketedCoin;
    [SerializeField] private Transform p2PocketedCoin;
    [SerializeField] private Transform coinsParent;

    private GameObject forcePointer; // Instance of the force pointer sprite
    private Vector2 initialPosition; // Initial position of the object being dragged
    private Quaternion targetRotation;
    private Vector2 direction;
    private Rigidbody2D rb;
    private float scaleValue;
    private float forceAmount;
    private bool strikerInAction;
    [SerializeField] float pointerScaleLimit = 100f;
    public bool player1Active;
    private bool queenCheck;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        p1StrikerSlider.onValueChanged.AddListener(StrickerSlidePos);
        p2StrikerSlider.onValueChanged.AddListener(StrickerSlidePos);
        p2StrikerSlider.interactable = false;
        strikerInAction = false;
        player1Active = true;
        queenCheck = false;
    }
    private void Update()
    {
        if (strikerInAction)
        {
            if (rb.velocity.magnitude < 2f)
            {
                bool coinPocketeds = pocket.GetComponent<PocketScript>().coinPocketed;
                bool queenPocketeds = pocket.GetComponent<PocketScript>().queenPocketed;
                if (queenCheck && coinPocketeds)
                {
                    Debug.Log("Queen successfully Pocketed");
                }
                if (queenCheck && !coinPocketeds)      // reversing the move
                {
                    Debug.Log("Returning Queen to Board");
                    pocket.GetComponent<PocketScript>().queenPocketed = false;
                    queenCheck = false;
                    Transform childObject = p1PocketedCoin.Find("Queen");
                    if (childObject != null)
                    {
                        childObject.SetParent(coinsParent);
                        childObject.localPosition = Vector3.zero;
                    }
                    else
                    {
                        Transform childObject2 = p2PocketedCoin.Find("Queen");
                        childObject2.SetParent(coinsParent);
                        childObject2.localPosition = Vector3.zero;
                    }
                }
                if (queenPocketeds)
                {
                    queenCheck = true;
                    Debug.Log("Queen Check");
                }
                if (coinPocketeds)
                {
                    pocket.GetComponent<PocketScript>().coinPocketed = false;
                    Debug.Log("Coin Pocketed");
                }
                if (!coinPocketeds)
                {
                    Debug.Log("Coin Not Pocketed Changing Turn");
                    if (player1Active)
                    {
                        player1Active = false;
                    }
                    else
                    {
                        player1Active = true;
                    }
                }
                if (player1Active)
                {
                    Debug.Log("Player 1 Active");
                    gameObject.transform.SetParent(player1);
                    gameObject.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    gameObject.transform.GetComponent<Rigidbody2D>().angularVelocity = 0;
                    gameObject.transform.rotation = Quaternion.identity;
                    gameObject.transform.localPosition = Vector3.zero;
                    p1StrikerSlider.value = 0f;
                    p1StrikerSlider.interactable = true;
                    p2StrikerSlider.value = 0f;
                    p2StrikerSlider.interactable = false;
                    strikerInAction = false;
                }
                else
                {
                    Debug.Log("Player 2 Active");
                    gameObject.transform.SetParent(player2);
                    gameObject.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    gameObject.transform.GetComponent<Rigidbody2D>().angularVelocity = 0;
                    gameObject.transform.rotation = Quaternion.identity;
                    gameObject.transform.localPosition = Vector3.zero;
                    p1StrikerSlider.value = 0f;
                    p1StrikerSlider.interactable = false;
                    p2StrikerSlider.value = 0f;
                    p2StrikerSlider.interactable = true;
                    strikerInAction = false;
                }
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
        //Debug.Log("Strike Force: " + forceAmount);

        if (forceAmount > 500)
        {
            // Normalize the direction vector to ensure consistent speed
            Vector3 normalizedDirection = -direction;

            // Apply the force to the rigidbody
            rb.AddForce(normalizedDirection * forceAmount * forceMultiplier);
        }

        // Destroy the force pointer prefab
        Destroy(forcePointer);

        StartCoroutine(SetBoolTrue());
    }
    private IEnumerator SetBoolTrue()
    {
        yield return new WaitForSeconds(1);
        strikerInAction = true;
        p1StrikerSlider.interactable = false;
        p2StrikerSlider.interactable = false;
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
