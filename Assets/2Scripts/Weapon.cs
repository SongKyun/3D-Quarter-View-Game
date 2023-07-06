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

    public Transform bulletPos; // ������ ���� ��ġ ����
    public GameObject bullet; // ������ ���� ����
    public Transform bulletCasePos;
    public GameObject bulletCase;

    public int maxAmmo;
    public int curAmmo;

    public void Use()
    {
        if(type == Type.Melee)
        {
            StopCoroutine("Swing"); // ���� �ڷ�ƾ�� �����ϱ� ���ؼ� ����Ǵ� �ڷ�ƾ ���� ��
            StartCoroutine("Swing"); // ���ο� �ڷ�ƾ �������� ������ ������ �ʰ� �Ѵ�.
        }
        else if(type == Type.Range && curAmmo > 0)
        {
            curAmmo--;
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing() // ���� ����
    {
        //1
        yield return new WaitForSeconds(0.5f); // 1������ ���
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        //2
        yield return new WaitForSeconds(0.1f); // 1������ ���
        meleeArea.enabled = false;

        //3
        yield return new WaitForSeconds(0.3f); // 1������ ���
        trailEffect.enabled = false;
    }

    IEnumerator Shot()
    {
        // �Ѿ� �߻�
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 100;

        yield return null;

        // ź�� ����
        GameObject intantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = intantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddForce(Vector3.up * 5, ForceMode.Impulse);
    }
    // Use() ���η�ƾ -> Swing() �����ƾ -> ���� ��ƾ(��������)
    // Use() ���η�ƾ + Swing() - �ڷ�ƾ (Co-Op)
}
