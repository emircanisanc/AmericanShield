using UnityEngine;

public class Chicken : MonoBehaviour
{
    public float moveSpeed = 0.5f;          // Neneyi ne kadar hızlı hareket ettireceğimizi belirleyen değişken
    public float idleTime = 3f;           // Bekleme süresi

    private Animator animator;            // Animator bileşenine referans
    private bool isMoving = false;        // Nenenin hareket halinde olup olmadığını kontrol eden değişken
    private Vector3 targetPosition;       // Hedef noktanın konumu

    private void Start()
    {
        animator = GetComponent<Animator>();   // Animator bileşenini al
        SetRandomTargetPosition();               // İlk hedef noktayı belirle
    }

    private void Update()
    {
        if (!isMoving)
        {
            // Bekleme süresi bittiğinde idle animasyonunu oynat
            idleTime -= Time.deltaTime;
            if (idleTime <= 0)
            {
                animator.SetTrigger("Idle");  // "Idle" animasyonunu tetikle
                idleTime = 3f;  // Bekleme süresini sıfırla veya istediğiniz süreyle güncelle
            }
        }
        else
        {
            // Hedefe doğru hareket et ve walk animasyonunu oynat
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            animator.SetTrigger("Walk");  // "Walk" animasyonunu tetikle

            // Hedef noktaya ulaşıldığında yeni hedef nokta belirle
            if (transform.position == targetPosition)
            {
              Invoke("SetRandomTargetPosition",3f);
            }
        }
    }

    private void SetRandomTargetPosition()
    {
        // Rasgele bir nokta seç ve hedef olarak belirle
        float randomX = Random.Range(-2f, 2f);  
        float randomZ = Random.Range(-2f, 2f);
        float randomY = transform.position.y;  
        targetPosition = new Vector3(randomX, randomY, randomZ);
        // Hedef noktaya doğru dön ve hareketi başlat
        transform.LookAt(targetPosition);
        isMoving = true;
    }
}
