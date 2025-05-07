using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Rotator : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,0, speed * Time.deltaTime));
    }
}
