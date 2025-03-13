using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusItem : BaitBase
{
    [Header("�o������"), SerializeField]
    private float spownTime = 0;

    private PlayerBase player;

    private new void Awake()
    {
        base.Awake();

        player = FindAnyObjectByType<PlayerBase>();
    }

    // Start is called before the first frame update
    new void Start()
    {
        //base.Start();
        StartCoroutine(TimeDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        if(player.IsDead)   // �v���C���[���S��
        {
            StopCoroutine(TimeDestroy());   // �R���[�`�����~�߂�
            Destroy(gameObject);
        }
    }

    private IEnumerator TimeDestroy()   // ��莞�Ԍo�ߌ�ɏ���
    {
        yield return new WaitForSeconds(spownTime);

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            base.BaseProcess(false);

            StopCoroutine(TimeDestroy());   // �R���[�`�����~�߂�
            Destroy(gameObject);
        }
    }
}
