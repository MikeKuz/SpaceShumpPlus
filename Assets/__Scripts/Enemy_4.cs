using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Part - ��� ���� ������������ ����� ������ WeaponDefinition, 
///  ��������������� ��� �������� ������
/// </summary>
[System.Serializable]
public class Part
{
    //�������� ���� ���� ����� ������ ������������ � ����������
    public string       name;           //��� ���� �����
    public float        health;         //������� ��������� ���� �����
    public string[]     protectedBy;    //������ �����, ���������� ���

    //��� ��� ���� ���������������� ������������� � Start().
    //�����������, ��� �����, �������� ��������� ����������� ������
    [HideInInspector]   //�� ��������� ���������� ���� ��������� � ����������
    public GameObject go; // ������� ������ ���� ����
    [HideInInspector]
    public Material mat;    //�������� ��� ����������� �����������
}


/// <summary>
/// Enemy_4 ��������� �� ������� ��������, �������� ��������� ����� �� ������
///  � ������������ � ���. ����������  �����, �������� ������ ��������� ����
///  � ���������� ���������, ���� ����� ��� �� ���������.
/// </summary>



public class Enemy_4 : Enemy
{
    [Header("Set in Inspector: Enemy_4")]
    public Part[] parts;    //������ ������ ������������ �������
    public new readonly int score = 100;

    protected override int Score
    {
        get => base.Score = score;
    }

    private Vector3 p0, p1; //��� ����� ��� ������������
    private float timeStart;    //����� �������� ����� �������
    private float duration = 4; //����������������� �����������
    
    //[Header("Set in Inspector: Enemy_3")]

    //[Header("Set Dynamically: Enemy_3")]

    void Start()
    {
        //��������� ������� ��� ������� � Main.SpawnEnemy(),
        // ������� ������� �� ��� ��������� �������� � p0 � p1
        p0 = p1 = pos;
        InitMovement();

        //�������� � ��� ������� ������ � �������� ����� � parts
        Transform t;
        foreach(Part prt in parts)
        {
            t = transform.Find(prt.name);
            if(t!=null)
            {
                prt.go = t.gameObject;
                prt.mat = prt.go.GetComponent<Renderer>().material;
            }
        }
    }

    void InitMovement()
    {
        p0 = p1;    //���������� p1 � p0 
        //������� ����� ����� p1 �� ������
        float widMinRad = bndCheck.camWidth - bndCheck.radius;
        float hgtMinRad = bndCheck.camHeight - bndCheck.radius;
        p1.x = Random.Range(-widMinRad, widMinRad);
        p1.y = Random.Range(-hgtMinRad, hgtMinRad);

        //�������� �����
        timeStart = Time.time;
    }

    public override void Move()
    {
        //���� ����� �������������� Eneme.Move � ��������� 
        // �������� ������������
        float u = (Time.time - timeStart) / duration;

        if(u>=1)
        {
            InitMovement();
            u = 0;
        }

        u = 1 - Mathf.Pow(1 - u, 2);    //��������� ������� ���������
        pos = (1 - u) * p0 + u * p1;    //������� �������� ������������
    }

    //��� �� ������� ��������� ����� ����� � ������� parts  n 
    // �� ����� ��� �� ������ �� ������� ������
    Part FindPart(string n)
    {
        foreach(Part prt in parts)
        {
            if (prt.name == n)
            {
                return (prt);
            }
        }
        return (null);
    }

    Part FindPart(GameObject go)
    {
        foreach(Part prt in parts)
        {
            if(prt.go==go)
            {
                return (prt);
            }
        }
        return (null);
    }

    //��� ������� ���������� true, ���� ������ ����� ����������
    bool Destroyed(GameObject go)
    {
        return(Destroyed(FindPart(go)));
    }    
    
    bool Destroyed(string n)
    {
        return(Destroyed(FindPart(n)));
    }

    bool Destroyed(Part prt)
    {
        if(prt==null)       //���� ������ �� ����� �� ���� ��������
        {
            return (true);  //������� true (�� ����: ��, ���� ����������)
        }
        //������� ��������� ���������: prt.health<=0
        //���� prt.health <=0, ������� true(��, ���� ����������)
        return (prt.health <= 0);
    }

    // ���������� � ������� ���� ������� ���� �����, � �� ���� �������
    void ShowLocalizedDamage(Material m)
    {
        m.color = Color.red;
        damageDoneTime = Time.time + showDamageDuration;
        showingDamage = true;
    }

    // �������������� ����� OnCollisionEnter �� �������� Enemy.cs
    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        switch(other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                //���� ������� �� ��������� ������, �� ���������� ���
                if(!bndCheck.isOnScreen)
                {
                    Destroy(other);
                    break;
                }
                // �������� ��������� �������
                GameObject goHit = collision.contacts[0].thisCollider.gameObject;
                Part prtHit = FindPart(goHit);
                if(prtHit==null)
                {
                    goHit = collision.contacts[0].otherCollider.gameObject;
                    prtHit = FindPart(goHit);
                }
                // ��������� �������� �� ��� ��� ����� �������
                if(prtHit.protectedBy!=null)
                {
                    foreach(string s in prtHit.protectedBy)
                    {
                        // ���� ���� �� ���� �� ���������� ������ ���
                        // �� ���������...
                        if(!Destroyed(s))
                        {
                            //...�� �������� ����������� ���� �����
                            return;
                        }
                    }
                }
                // ��� ����� �� ��������, ������� �� �����������
                // �������� ����������� ���� �� Projectile.type � Main.WEAP_DICT
                prtHit.health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                //�������� ������ ��������� � �����
                ShowLocalizedDamage(prtHit.mat);
                if(prtHit.health<=0)
                {
                    //������ ���������� ����� �������
                    // �������������� ������������ �����
                    prtHit.go.SetActive(false);
                }
                //��������� ��� �� ������� ��������� ��������
                bool allDestroyed = true;   //������������ ��� ��������
                foreach(Part prt in parts)
                {
                    if(!Destroyed(prt)) //���� �����-�� ����� ��� ����������...
                    {
                        allDestroyed = false;   //...�������� false � allDestroyed
                        break;  // � �������� ���� foreach
                    }
                }
                if(allDestroyed)
                {
                    //...��������� �����-�������� Main ��� ���� ������� ��������
                    Main.S.ShipDestroyed(this);
                    //���������� ���� ������ Enemy
                    Destroy(this.gameObject);
                    base.MessageScoreCunterAboutKill(Score);
                }
                Destroy(other); //���������� ������ ProjectileHero
                break;
        }
    }
}
