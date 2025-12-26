

using UnityEngine;

public class explosionDeleter : MonoBehaviour {
    public float delay = 1.0f; // Adjust this to match your animation length
    void Start() {
        Destroy(gameObject, delay);
    }
}
