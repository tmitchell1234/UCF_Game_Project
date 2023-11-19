using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    int timer = 0;
    bool nextmove = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (timer < 10) { timer++; } else { timer = 0; nextmove = true; }

        // When eough enemies are defeated, lower the wall and reset the lowering condition to false
        if(nextmove)
        {
            nextmove = false;

            transform.position += new Vector3(0, -0.5f, 0);
        }
    }
}
