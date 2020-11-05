using UnityEngine;

public class AreaColScript : MonoBehaviour
{
    bool isAlreadyTriggered;
    private void OnTriggerEnter(Collider other)
    {
        if (isAlreadyTriggered == false)
        {
            if (other.gameObject.tag == "Player")
            {
                isAlreadyTriggered = true;
                foreach (Transform child in transform)
                {
                    child.GetComponent<Enemy>().Trigger(other.gameObject);
                }
            }
        }
    }
}
