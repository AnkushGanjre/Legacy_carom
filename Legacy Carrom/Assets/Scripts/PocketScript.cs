using UnityEngine;
using UnityEngine.Events;

public class PocketScript : MonoBehaviour
{
    [SerializeField] private UnityEvent forceZero;
    [SerializeField] private GameObject stricker;
    private string queenTag = "QueenTag";
    private string whiteCoinTag = "WhiteCoinTag";
    private string BlackCoinTag = "BlackCoinTag";
    private string strikerTag = "StrikerTag";

    [SerializeField] private Transform p1PocketedCoins;
    [SerializeField] private Transform p2PocketedCoins;

    public bool coinPocketed;
    public bool queenPocketed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool value = stricker.GetComponent<StrickerControllerScript>().player1Active;
        if (collision.CompareTag(queenTag))
        {
            Debug.Log("Queen");
            queenPocketed = true;
            if (value)
            {
                collision.transform.SetParent(p1PocketedCoins);
                collision.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                collision.transform.GetComponent<Rigidbody2D>().angularVelocity = 0;
                collision.transform.rotation = Quaternion.identity;
                collision.transform.localPosition = Vector3.zero;
            }
            else
            {
                collision.transform.SetParent(p2PocketedCoins);
                collision.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                collision.transform.GetComponent<Rigidbody2D>().angularVelocity = 0;
                collision.transform.rotation = Quaternion.identity;
                collision.transform.localPosition = Vector3.zero;
            }
        }
        if (collision.CompareTag(whiteCoinTag))
        {
            Debug.Log("White");
            coinPocketed = true;
            if (value)
            {
                collision.transform.SetParent(p1PocketedCoins);
                collision.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                collision.transform.GetComponent<Rigidbody2D>().angularVelocity = 0;
                collision.transform.rotation = Quaternion.identity;
                collision.transform.localPosition = Vector3.zero;
            }
            else
            {
                collision.transform.SetParent(p2PocketedCoins);
                collision.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                collision.transform.GetComponent<Rigidbody2D>().angularVelocity = 0;
                collision.transform.rotation = Quaternion.identity;
                collision.transform.localPosition = Vector3.zero;
            }
        }
        if (collision.CompareTag(BlackCoinTag))
        {
            Debug.Log("Black");
            coinPocketed = true;
            if (value)
            {
                collision.transform.SetParent(p1PocketedCoins);
                collision.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                collision.transform.GetComponent<Rigidbody2D>().angularVelocity = 0;
                collision.transform.rotation = Quaternion.identity;
                collision.transform.localPosition = Vector3.zero;
            }
            else
            {
                collision.transform.SetParent(p2PocketedCoins);
                collision.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                collision.transform.GetComponent<Rigidbody2D>().angularVelocity = 0;
                collision.transform.rotation = Quaternion.identity;
                collision.transform.localPosition = Vector3.zero;
            }
        }
        if (collision.CompareTag(strikerTag))
        {
            Debug.Log("Striker");
            if (value)
            {
                collision.transform.SetParent(p2PocketedCoins);
                collision.transform.localPosition = Vector3.zero;
                collision.transform.rotation = Quaternion.identity;
                forceZero.Invoke();
            }
            else
            {
                collision.transform.SetParent(p1PocketedCoins);
                collision.transform.localPosition = Vector3.zero;
                collision.transform.rotation = Quaternion.identity;
                forceZero.Invoke();
            }
            
        }
    }
}
