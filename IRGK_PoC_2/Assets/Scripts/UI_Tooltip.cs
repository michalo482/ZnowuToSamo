using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Tooltip : MonoBehaviour
{

    [SerializeField] private float xLimit = 960;

    [SerializeField] private float yLimit = 540;

    [SerializeField] private float xOffset = 150;
    [SerializeField] private float yOffset = 150;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void AdjustPosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        float newXoffset = 0;
        float newYoffset = 0;

        if (mousePosition.x > xLimit)
        {
            newXoffset = -xOffset;
        }
        else
        {
            newXoffset = xOffset;
        }

        if (mousePosition.y > yLimit)
        {
            newYoffset = -yOffset;
        }
        else
        {
            newYoffset = yOffset;
        }

        transform.position = new Vector3(mousePosition.x + newXoffset, mousePosition.y + newYoffset);
    }
}
