using UnityEngine;

public class MovingPlatform : Interactable
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform destinationPosition;
    public float moveSpeed = 2f; // 이동 속도

    public override void SetInteract()
    {
        //상호작용할 함수를 이벤트에 등록하기

    }

    private void GetItem()
    {
        base.SetInteract();

    }

    private void FixedUpdate()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform); // 플레이어를 발판의 자식으로 설정
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null); // 플레이어를 다시 원래 상태로 복귀
        }
    }
}
