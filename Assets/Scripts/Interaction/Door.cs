using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform panel;
    [SerializeField] private Vector3 openOffset = new Vector3(0f, 3f, 0f);
    [SerializeField] private float openSpeed = 2f;
    [SerializeField] private bool canClose = true;

    private Vector3 _closedPosition;
    private Vector3 _openPosition;

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        _closedPosition = panel.localPosition;
        _openPosition = _closedPosition + openOffset;
    }

    private void Update()
    {
        Vector3 targetPosition = IsOpen ? _openPosition : _closedPosition;

        panel.localPosition = Vector3.MoveTowards(
            panel.localPosition,
            targetPosition,
            openSpeed * Time.deltaTime
        );
    }

    public void Open()
    {
        IsOpen = true;
    }

    public void Close()
    {
        if (!canClose)
        {
            return;
        }

        IsOpen = false;
    }
}