using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class JoyStick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public GameObject handle;
    public GameObject targetObject;
    public float maxDistance;
    Vector2 startPos;
    Vector2 zuruu;
    PlayerControl playerControl;
    public float moveSpeed;
    public bool canMove = false;
    Vector2 movePos;
    Vector2 mainStartPos;
    public bool isDinamic = true;
    private void Update()
    {
        if (canMove)
        {
            transform.Translate(movePos * (isDinamic ? moveSpeed : 0));
            if (Vector2.Distance(handle.transform.position, Input.mousePosition) <= 50)
            {
                canMove = false;
            }
        }
    }
    private void Start()
    {
        startPos = handle.transform.localPosition;
        mainStartPos = transform.position;
        playerControl = targetObject.GetComponent<PlayerControl>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        canMove = true;
        if (Vector2.Distance(eventData.position, startPos) <= maxDistance)
        {
            handle.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        movePos = eventData.position - (Vector2)transform.position;
        movePos.Normalize();
        if (Vector2.Distance(eventData.position, startPos + (Vector2)transform.position) <= maxDistance)
        {
            handle.transform.position = eventData.position;
            zuruu = eventData.position - startPos - (Vector2)transform.position;
        }
        else
        {
            zuruu = eventData.position - startPos - (Vector2)transform.position;
            zuruu.Normalize();
            zuruu *= maxDistance;
            handle.transform.position = startPos + (Vector2)transform.position + zuruu;
        }
        if (Vector2.Distance(handle.transform.position, eventData.position) > 50)
        {
            canMove = true;
        }
        zuruu.Normalize();
        playerControl.Move(zuruu);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        canMove = false;
        handle.transform.localPosition = startPos;
        playerControl.Move(Vector2.zero);
        transform.position = mainStartPos;
    }
    public void ResetPos()
    {
        transform.position = mainStartPos;
    }
}
