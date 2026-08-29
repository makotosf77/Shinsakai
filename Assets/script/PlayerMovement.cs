using NUnit.Framework.Constraints;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movespeed = 1f;
    [Header("Move Area")]
    [SerializeField] private RectTransform battleArea;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        Move();
    }
    private void Move()
    {
        float horizonal = 0f;
        float vertonal = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            horizonal -= 1f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            horizonal += 1f;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            vertonal += 1f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            vertonal -= 1f;
        }
        if(Input.GetKey(KeyCode.A))
        {
            horizonal -= 1f;
        }
        if(Input.GetKey(KeyCode.D))
        {
            horizonal += 1f;
        }
        if(Input.GetKey(KeyCode.W))
        {
            vertonal += 1f;
        }
        if(Input.GetKey(KeyCode.S))
        {
            vertonal-=1f;
        }
        Vector2 direction = new Vector2(horizonal, vertonal);
        if(direction.magnitude>1f)
        {
            direction.Normalize();
        }
        Vector2 newPosition = rectTransform.anchoredPosition+direction*movespeed*Time.deltaTime;
        ClampPosition(ref newPosition);
        rectTransform.anchoredPosition= newPosition;

    }
    private void ClampPosition(ref Vector2 position)
    {
        if(battleArea == null)
        {
            return;
        }
        Rect areaRect = battleArea.rect;
        float halfWidth = rectTransform.rect.width / 2f;
        float halfHeight = rectTransform.rect.height / 2f;
        float minX = areaRect.xMin+halfWidth;
        float maxX = areaRect.xMax-halfWidth;
        float minY = areaRect.yMin+halfHeight;
        float maxY = areaRect.yMax-halfHeight;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);
    }
}
