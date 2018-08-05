using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    private Camera mainCamera;
    public Transform character;
    public GameObject trapPrefab;
    public GameObject levelContainer;

    private float trapY;
    private float dropTime;

	void Start ()
    {
        mainCamera = Camera.main;

        // y-position of camera - half height of camera - little bit offset.
        trapY = mainCamera.transform.position.y + mainCamera.orthographicSize + 2f;
	}
	
	void Update ()
    {
        // Drop a trap
		if(dropTime <= 0f)
        {
            GameObject trap = Instantiate(trapPrefab, levelContainer.transform, true);

            float trapX = character.position.x + Random.Range(4f, 10f);
            trap.transform.position = new Vector3(trapX, trapY, 0f);

            dropTime = Random.Range(3f, 6f);
        }
        // Decrase dropTime
        else
        {
            dropTime -= Time.deltaTime;
        }
	}
}
