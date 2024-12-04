using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject npcAttackPrefab; // Prefab của NPCAttack
    public MP playerMP; // Tham chiếu đến MP của người chơi
    public float summonDuration = 10f; // Thời gian tồn tại của NPCAttack
    public int summonMpCost = 40; // MP tiêu tốn cho kỹ năng triệu hồi
    public float behindDistance = 2f; // Khoảng cách phía sau người chơi để triệu hồi

    public GameObject sword; // Kiếm của Player
    public Vector3 enlargedSwordScale; // Kích thước khi phóng to kiếm
    public GameObject newDamageZone; // GameObject chứa DamageZone và BoxCollider mới
    public int enlargeMpCost = 20; // MP tiêu tốn cho kỹ năng phóng to
    public float enlargeDuration = 5f; // Thời gian phóng to kiếm

    private Transform playerTransform;
    private Vector3 originalSwordScale;
    private GameObject originalDamageZone;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        originalSwordScale = sword.transform.localScale;
        originalDamageZone = GameObject.Find("DamageZone");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TrySummonNPCAttack();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TryEnlargeSword();
        }
    }

    void TrySummonNPCAttack()
    {
        if (playerMP.CurrentMP >= summonMpCost)
        {
            // Tính toán vị trí phía sau người chơi
            Vector3 summonPosition = playerTransform.position - playerTransform.forward * behindDistance;

            // Triệu hồi NPCAttack
            GameObject npcAttack = Instantiate(npcAttackPrefab, summonPosition, playerTransform.rotation);

            // Tìm và thiết lập Player cho NPC
            NPCAttack npcAttackScript = npcAttack.GetComponent<NPCAttack>();
            npcAttackScript.SetPlayer(playerTransform);

            // Giảm MP của người chơi
            playerMP.CurrentMP -= summonMpCost;
            playerMP.UpdateManaUI();

            // Hủy NPCAttack sau thời gian tồn tại
            Destroy(npcAttack, summonDuration);
        }
        else
        {
            Debug.Log("Không đủ MP để triệu hồi NPCAttack");
        }
    }

    void TryEnlargeSword()
    {
        if (playerMP.CurrentMP >= enlargeMpCost)
        {
            // Phóng to kiếm
            sword.transform.localScale = enlargedSwordScale;

            // Kích hoạt DamageZone mới và ẩn DamageZone cũ
            originalDamageZone.SetActive(false);
            newDamageZone.SetActive(true);

            // Giảm MP của người chơi
            playerMP.CurrentMP -= enlargeMpCost;
            playerMP.UpdateManaUI();

            // Hủy phóng to kiếm sau thời gian tồn tại
            Invoke(nameof(ResetSword), enlargeDuration);
        }
        else
        {
            Debug.Log("Không đủ MP để phóng to kiếm");
        }
    }

    void ResetSword()
    {
        // Đặt lại kích thước ban đầu của kiếm
        sword.transform.localScale = originalSwordScale;

        // Kích hoạt lại DamageZone ban đầu và ẩn DamageZone mới
        newDamageZone.SetActive(false);
        originalDamageZone.SetActive(true);
    }
}





