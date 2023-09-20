using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableBehaviour : MonoBehaviour
{
    private DropAreaBehaviour dropArea;
    private Canvas parentCanvas;
    private CursorIconManager cursorManager;
    private TrackerManager trackerManager;
    [SerializeField] private bool changeCursorOnHover;

    [SerializeField] private RectTransform draggingTransform;
    private BoxCollider2D col;

    private Vector2 offsetVector;
    private bool pickedUp;

    [Header("Return Animation")]
    [SerializeField] private float returnAnimDuration;
    [SerializeField] private AnimationCurve returnAnimCurve;
    private Coroutine returnRoutine = null;
    private Vector2 beginPosition = Vector2.zero;

    //[Header("Drag Notification")]
    private Vector2 startDragPos;
    [SerializeField] private TrackerVisual trackerVisual;

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        dropArea = DropAreaBehaviour.instance;
        beginPosition = draggingTransform.anchoredPosition;
        parentCanvas = GetComponentInParent<Canvas>();
        col = draggingTransform.gameObject.GetComponent<BoxCollider2D>();
        col.size = draggingTransform.sizeDelta;
        cursorManager = CursorIconManager.instance;
        trackerManager = TrackerManager.Instance;
    }

    private void Update()
    {

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null && hit.collider == col)
        {
            //Change cursor
            if (changeCursorOnHover)
            {
                cursorManager.isHovering = true;
                cursorManager.targetCursorState = 2;
            }
            else
            {
                cursorManager.isHovering = true;
                cursorManager.targetCursorState = 5;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (returnRoutine != null) { StopCoroutine(returnRoutine); }
                Vector2 adjustedMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y) / parentCanvas.scaleFactor;
                offsetVector = draggingTransform.anchoredPosition - adjustedMousePos;
                startDragPos = draggingTransform.anchoredPosition;
                pickedUp = true;
            }
        }

        if (Input.GetMouseButtonUp(0) && pickedUp) 
        {
            if (!dropArea.mouseInArea)
            { 
                ReturnToStart();
            }

            cursorManager.isHovering = false;
            cursorManager.overriden = false;

            Vector2 moveVector = draggingTransform.anchoredPosition - startDragPos;
            trackerVisual.SpawnNotification($"Moved object vector({Mathf.RoundToInt(moveVector.x)}, {Mathf.RoundToInt(moveVector.y)})", 3f, 10f);

            trackerManager.totalDragDistance += Vector2.Distance(draggingTransform.anchoredPosition, startDragPos);

            pickedUp = false;
        }

        if (Input.GetMouseButton(0) && pickedUp)
        {
            //Change cursor
            cursorManager.overriden = true;
            cursorManager.isHovering = true;
            cursorManager.targetCursorState = 3;

            Vector2 adjustedMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y) / parentCanvas.scaleFactor;
            draggingTransform.anchoredPosition = adjustedMousePos + offsetVector;
        }
    }

    private void ReturnToStart()
    {
        //Animate back to start position
        if(returnRoutine != null) { StopCoroutine(returnRoutine); }
        returnRoutine = StartCoroutine(ReturnStartIE());
    }

    public IEnumerator ReturnStartIE()
    {
        float timeValue = 0;
        Vector2 startPosition = draggingTransform.anchoredPosition;

        while (timeValue < 1)
        {
            timeValue += Time.deltaTime / returnAnimDuration;
            float evaluatedTimeValue = returnAnimCurve.Evaluate(timeValue);
            Vector2 newLocation = Vector2.Lerp(startPosition, beginPosition, evaluatedTimeValue);
            draggingTransform.anchoredPosition = newLocation;

            yield return null;
        }

        yield return null;

        returnRoutine = null;
    }
}
