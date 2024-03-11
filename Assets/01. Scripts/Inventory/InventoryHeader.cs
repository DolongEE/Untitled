using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryHeader : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField]
    private Transform _targetTr;    // 이동될 UI
    
    private Vector2 _startingPoint; // 초기 ui 위치
    private Vector2 _moveBegin;     // 내가 마우스 찍은 위치
    private Vector2 _moveOffset;    // 내가 움직인 현재 좌표 - 초기 마우스 위치

    private void Awake()
    {
        // 이동 대상 UI를 지정하지 않은 경우, 자동으로 부모로 초기화
        if (_targetTr == null)
            _targetTr = transform.parent;
    }

    // 드래그 시작 위치 지정
    public void OnPointerDown(PointerEventData eventData)
    {
        _startingPoint = _targetTr.position;
        _moveBegin = eventData.position;
    }

    // 드래그 : 마우스 커서 위치로 이동
    public void OnDrag(PointerEventData eventData)
    {
        _moveOffset = eventData.position - _moveBegin;
        _targetTr.position = _startingPoint + _moveOffset;
    }
}