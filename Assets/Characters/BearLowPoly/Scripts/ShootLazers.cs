using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLazers : MonoBehaviour
{
    [SerializeField] GameObject PlayerModel;
    [SerializeField] GameObject lazerPrefab;


    private ArrayList lazerObjects;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ShootLazer();
        }
    }

    public void ShootLazer()
    {
        Vector3 direction = PlayerModel.transform.position - this.transform.position;

        Quaternion rotation = Quaternion.LookRotation(direction);

        GameObject lazer = Instantiate(lazerPrefab, transform.position, rotation);

        lazerObjects.Add(lazer);

        Invoke("DeleteFrontLazer", 1.5f);
    }

    void DeleteFrontLazer()
    {
        Destroy((GameObject) lazerObjects[0]);

        lazerObjects.RemoveAt(0);
    }

    private void Awake()
    {
        lazerObjects = new ArrayList();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
}
