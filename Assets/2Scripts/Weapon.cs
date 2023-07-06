using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range };
    public Type type;
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;

    public Transform bulletPos; // 프리팹 생성 위치 변수
    public GameObject bullet; // 프리팹 저장 변수
    public Transform bulletCasePos;
    public GameObject bulletCase;

    public int maxAmmo;
    public int curAmmo;

    public void Use()
    {
        if(type == Type.Melee)
        {
            StopCoroutine("Swing"); // 같은 코루틴을 시작하기 위해서 진행되는 코루틴 정지 후
            StartCoroutine("Swing"); // 새로운 코루틴 시작으로 로직이 꼬이지 않게 한다.
        }
        else if(type == Type.Range && curAmmo > 0)
        {
            curAmmo--;
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing() // 근접 무기
    {
        //1
        yield return new WaitForSeconds(0.5f); // 1프레임 대기
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        //2
        yield return new WaitForSeconds(0.1f); // 1프레임 대기
        meleeArea.enabled = false;

        //3
        yield return new WaitForSeconds(0.3f); // 1프레임 대기
        trailEffect.enabled = false;
    }

    IEnumerator Shot()
    {
        // 총알 발사
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 100;

        yield return null;

        // 탄피 배출
        GameObject intantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = intantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddForce(Vector3.up * 5, ForceMode.Impulse);
    }
    // Use() 메인루틴 -> Swing() 서브루틴 -> 메인 루틴(교차실행)
    // Use() 메인루틴 + Swing() - 코루틴 (Co-Op)
}
