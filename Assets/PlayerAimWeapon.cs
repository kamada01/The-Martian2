using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAimWeapon : MonoBehaviour
{
    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs{
        public Vector3 gunEndPointPosition;
        public Vector3 shootPosition;
    }
    private Transform aimTransform;
    private Transform objectEffect;
    private Transform aimGunEndPointTransform;
    private Animator aimAnimator;
    private AnimatorStateInfo stateInfo;

    public int damage = 10;
    public int bulletSpeed = 10;
    public float firerate = 0;
    public Transform muzzle;

    [SerializeField] private Transform bullet;

    float timeToFire = 0;
    // Start is called before the first frame update
    void Start()
    {
        //aimTransform = GetComponent<Transform>();
    }
    private void Awake(){
        aimTransform = transform.Find("Aim");
        objectEffect = aimTransform.transform.Find("effect");
        aimAnimator = objectEffect.GetComponent<Animator>();
        aimGunEndPointTransform =aimTransform.transform.Find("gunEndPointTransform");

    }

    

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
            return;
        MouseAim();
        HandleShooting();
    }

    private void FixedUpdate()
    {

    }

    private void MouseAim(){
        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.rotation = Quaternion.Euler(0 , 0 , angle);
    }

    private void HandleShooting(){
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)){
            Vector3 mousePosition = GetMouseWorldPosition();
            aimAnimator.SetBool("Shoot", true);
            OnShoot?.Invoke(this, new OnShootEventArgs{
                    gunEndPointPosition = aimGunEndPointTransform.position,
                    shootPosition = mousePosition,
            });
            Fire();
        }
        //aimAnimator.SetBool("Shoot", false);
        else{
            stateInfo = aimAnimator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 1 && !aimAnimator.IsInTransition(0))
            {
                aimAnimator.SetBool("Shoot", false);
                
            }
        }
    }

     void Fire() {

        //GameController.Instance.GetComponent<AudioManager>().PlaySound("GunShot");


        SpawnBullet();

    }

    void SpawnBullet() {

        /*if (bullet == null)
            return;*/

        //Transform shot = Instantiate(bullet, muzzle.position, muzzle.rotation);
        //shot.GetComponent<Bullet>().damage = damage;

        //Instantiate(muzzleFlash, muzzle.position, muzzle.rotation);

        // Instantiate the bullet
        Transform bullet1 = Instantiate(bullet, muzzle.position, muzzle.rotation);

        // Calculate the direction
        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 aimDirection = (mousePosition - transform.position).normalized;

        // Set the bullet's direction
        bullet1.GetComponent<Rigidbody2D>().velocity = aimDirection * bulletSpeed;

        return;

    }


    public static Vector3 GetMouseWorldPosition(){
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetMouseWorldPositionWithZ() {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera){
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition,Camera worldCamera){
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
    

}
