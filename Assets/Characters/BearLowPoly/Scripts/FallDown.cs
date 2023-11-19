using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDown : MonoBehaviour
{
    [SerializeField] float fallSpeed;
    // Update is called once per frame
    void Update()
    {
        Vector3 down = new Vector3(0, -1f, 0);
        // down.y -= 0.5f;
        transform.position += down * fallSpeed * Time.deltaTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
}
