using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private PlayerAimWeapon playerAimWeapon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private void PlayerAimWeapon_OnShoot(object sender, PlayerAimWeapon.OnShootEventArgs e){
        WeaponTracer.Create(e.gunEndPointPosition, e.shootPosition);
        Shoot_Flash.AddFlash(e.gunEndPointPosition);
    }*/
}
