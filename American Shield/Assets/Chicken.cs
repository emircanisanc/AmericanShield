using System.Collections;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    public Transform[] transformNoktalari;
    public float beklemeSure = 5f;
    public Animator animator;

    private int currentNoktaIndex = 0;

    private void Start()
    {
        StartCoroutine(PlayerHareket());
    }

    private IEnumerator PlayerHareket()
    {
        while (true)
        {
            // Hedef noktaya doğru ilerle
            Vector3 hedefNokta = transformNoktalari[currentNoktaIndex].position;
            Vector3 yon = hedefNokta - transform.position;
            Quaternion hedefRotasyon = Quaternion.LookRotation(new Vector3(yon.x, 0, yon.z));
            float donmeHizi = 15f; // Dönme hızını istediğiniz gibi ayarlayabilirsiniz

            while (transform.position != hedefNokta)
            {
                // Karakteri yavaşça hedef noktaya doğru ilerlet

                transform.position = Vector3.MoveTowards(transform.position, hedefNokta, Time.deltaTime);

                // Karakterin yönünü yavaşça hedef noktaya doğru döndür
                transform.rotation = Quaternion.Slerp(transform.rotation, hedefRotasyon, donmeHizi * Time.deltaTime);

                yield return null;
            }

            // Walk animasyonunu çalıştır
            animator.SetBool("isWalking", false);

            // Belirli bir süre beklemek için bekleme süresi kadar bekleyin
            yield return new WaitForSeconds(beklemeSure);

            // Idle animasyonunu çalıştır
            animator.SetBool("isWalking", true);

            // Bir sonraki noktaya ilerle
            currentNoktaIndex = (currentNoktaIndex + 1) % transformNoktalari.Length;
        }
    }
}
