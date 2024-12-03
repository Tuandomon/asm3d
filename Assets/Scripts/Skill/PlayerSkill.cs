using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public GameObject newSwordPrefab; // Prefab của kiếm mới
    public float skillDuration = 10f; // Thời gian tồn tại của kỹ năng
    public int manaCost = 20; // MP tiêu tốn
    public float rotationSpeed = 100f; // Tốc độ quay của kiếm mới
    public float rotationDistance = 1f; // Khoảng cách từ player đến kiếm mới

    private MP playerMP;
    private GameObject newSword; // Biến lưu trữ đối tượng kiếm mới
    private bool skillActive = false; // Trạng thái của kỹ năng

    void Start()
    {
        playerMP = GetComponent<MP>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && playerMP.CurrentMP >= manaCost && !skillActive)
        {
            StartCoroutine(ActivateSkill());
        }

        // Quay kiếm mới quanh người chơi nếu kỹ năng đang hoạt động
        if (skillActive && newSword != null)
        {
            newSword.transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
            newSword.transform.rotation = Quaternion.Euler(0, 0, 90); // Đặt kiếm nằm ngang
            Vector3 direction = (newSword.transform.position - transform.position).normalized;
            newSword.transform.position = transform.position + direction * rotationDistance;
        }
    }

    private IEnumerator ActivateSkill()
    {
        skillActive = true;

        // Tiêu tốn mana
        playerMP.CurrentMP -= manaCost;
        playerMP.UpdateManaUI();

        // Tạo kiếm mới và thiết lập vị trí ban đầu
        newSword = Instantiate(newSwordPrefab, transform.position + Vector3.right * rotationDistance, Quaternion.Euler(0, 0, 90), transform);

        // Đợi thời gian tồn tại của kỹ năng
        yield return new WaitForSeconds(skillDuration);

        // Hủy kiếm mới sau khi hết thời gian
        Destroy(newSword);
        skillActive = false;
    }
}



