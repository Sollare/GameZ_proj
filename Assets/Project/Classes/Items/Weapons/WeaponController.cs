using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    #region Событие оружия
    public delegate void WeaponStateChangedDelegate(Weapon weapon, WeaponState newState);
    private delegate void OnCompleted();

    public event WeaponStateChangedDelegate OnWeaponStateChanged;

    protected virtual void InvokeWeaponStateChanged(Weapon weapon, WeaponState newstate)
    {
        WeaponStateChangedDelegate handler = OnWeaponStateChanged;
        if (handler != null) handler(weapon, newstate);
    }
    #endregion

    public enum WeaponState
    {
        Use,
        Throw,
        Reload,
        Equipped,
        Unequipped
    }

    public Weapon EquippedWeapon;

    private PlayerController _player;
    private SpriteRenderer _equippedItemSprite;
    private Transform _weaponRoot;

    private bool _weaponReadyToHit = true;

    void Awake()
    {
        _equippedItemSprite = GetComponent<SpriteRenderer>();
        _player = transform.root.GetComponent<PlayerController>();
        _weaponRoot = GameObject.Find("WeaponRoot").transform;
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (weapon == EquippedWeapon) return;

        UneqipHoldedWeapon();

        GameObject weaponPrefab = weapon.GetEquipablePrefab();
        var weaponInstance = GameObject.Instantiate(weaponPrefab) as GameObject;
        weaponInstance.transform.parent = _weaponRoot;
        weaponInstance.transform.localPosition = Vector3.zero;
        weaponInstance.tag = "Untagged";

        weaponInstance.collider2D.isTrigger = true;

        var rb = weaponInstance.ForceComponent<Rigidbody2D>();
        rb.isKinematic = true;

        EquippedWeapon = weapon;
        InvokeWeaponStateChanged(EquippedWeapon, WeaponState.Equipped);
    }

    public void UneqipHoldedWeapon()
    {
        if (EquippedWeapon)
        {
            for(int i = 0; i < _weaponRoot.childCount; i++)
                Destroy(_weaponRoot.GetChild(i).gameObject);

            InvokeWeaponStateChanged(EquippedWeapon, WeaponState.Unequipped);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var item = (Weapon)_player.InventoryController.GetItemAt(0);

            if(item != null)
                EquipWeapon(item);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            var item = (Weapon)_player.InventoryController.GetItemAt(1);

            if (item != null)
                EquipWeapon(item);
        }

        if (_weaponReadyToHit && EquippedWeapon != null)
        {
            float sht = Input.GetAxis("Fire1");
            float thrw = Input.GetAxis("Fire2");

            if(sht == 1f)
                Attack();
        }
    }

    void Attack()
    {
        _weaponReadyToHit = false;
        _player.Attack();

        StartCoroutine(TimeCoroutine(EquippedWeapon.Rate, delegate
        {
            _weaponReadyToHit = true;
        }));
    }

    IEnumerator TimeCoroutine(float time, OnCompleted callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }
}
