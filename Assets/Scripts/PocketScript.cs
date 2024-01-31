using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class PocketScript : MonoBehaviour
{
    [SerializeField] private UnityEvent forceZero;
    private string queenTag = "QueenTag";
    private string whiteCoinTag = "WhiteCoinTag";
    private string BlackCoinTag = "BlackCoinTag";
    private string strikerTag = "StrikerTag";

    [SerializeField] private Transform pocketedCoins;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(queenTag))
        {
            Debug.Log("Queen");
            collision.transform.SetParent(pocketedCoins);
            collision.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            collision.transform.GetComponent<Rigidbody2D>().angularVelocity = 0;
            collision.transform.rotation = Quaternion.identity;
            collision.transform.localPosition = Vector3.zero;

        }
        if (collision.CompareTag(whiteCoinTag))
        {
            Debug.Log("White");
            collision.transform.SetParent(pocketedCoins);
            collision.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            collision.transform.GetComponent<Rigidbody2D>().angularVelocity = 0;
            collision.transform.rotation = Quaternion.identity;
            collision.transform.localPosition = Vector3.zero;
        }
        if (collision.CompareTag(BlackCoinTag))
        {
            Debug.Log("Black");
            collision.transform.SetParent(pocketedCoins);
            collision.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            collision.transform.GetComponent<Rigidbody2D>().angularVelocity = 0;
            collision.transform.rotation = Quaternion.identity;
            collision.transform.localPosition = Vector3.zero;
        }
        if (collision.CompareTag(strikerTag))
        {
            Debug.Log("Striker");
            collision.transform.localPosition = Vector3.zero;
            collision.transform.rotation = Quaternion.identity;
            forceZero.Invoke();
        }
    }
}
