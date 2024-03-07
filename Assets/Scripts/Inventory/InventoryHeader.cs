using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryHeader : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField]
    private Transform _targetTr;    // �̵��� UI
    
    private Vector2 _startingPoint; // �ʱ� ui ��ġ
    private Vector2 _moveBegin;     // ���� ���콺 ���� ��ġ
    private Vector2 _moveOffset;    // ���� ������ ���� ��ǥ - �ʱ� ���콺 ��ġ

    private void Awake()
    {
        // �̵� ��� UI�� �������� ���� ���, �ڵ����� �θ�� �ʱ�ȭ
        if (_targetTr == null)
            _targetTr = transform.parent;
    }

    // �巡�� ���� ��ġ ����
    public void OnPointerDown(PointerEventData eventData)
    {
        _startingPoint = _targetTr.position;
        _moveBegin = eventData.position;
    }

    // �巡�� : ���콺 Ŀ�� ��ġ�� �̵�
    public void OnDrag(PointerEventData eventData)
    {
        _moveOffset = eventData.position - _moveBegin;
        _targetTr.position = _startingPoint + _moveOffset;
    }
}