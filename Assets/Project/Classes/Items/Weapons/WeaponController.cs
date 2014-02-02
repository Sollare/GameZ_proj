using System;
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
    /// <summary>
    /// Управляет отображением оружия. Задается в момент инстанцирования объекта в руки персонажа
    /// </summary>
    public WeaponView EquippedWeaponView { get; set; }

    /// <summary>
    /// Ввод пользователя
    /// </summary>
    public Vector2 input { get; private set; }

    /// <summary>
    /// Используется для получения значения ввода для текущего типа оружия
    /// </summary>
    private WeaponInput inputController;

    /// <summary>
    /// Чувствительность управления
    /// </summary>
    public Vector2 sensivity { get; set; }

    private PlayerController _player;

    private Transform _weaponRoot;
    private Transform _weaponInstance;

    private bool _weaponReadyToHit = true;
    private bool _alternativeFirePressed = false;

    void Awake()
    {
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
        weaponInstance.transform.localEulerAngles = Vector3.zero;
        weaponInstance.tag = "Untagged";

        weaponInstance.collider2D.isTrigger = true;

        var rb = weaponInstance.ForceComponent<Rigidbody2D>();
        rb.isKinematic = true;

        _weaponInstance = weaponInstance.transform;
        EquippedWeapon = weapon;
        EquippedWeaponView = weaponInstance.GetComponent<WeaponView>();
        
        if(EquippedWeaponView)
            EquippedWeaponView.InitializeWith(_player, this, weapon, _weaponInstance);

        inputController = weapon.GetInputController();


        InvokeWeaponStateChanged(EquippedWeapon, WeaponState.Equipped);
    }

    public void UneqipHoldedWeapon()
    {
        if (EquippedWeapon)
        {
            _player.characterAnimator.StopAnimations();

            for(int i = 0; i < _weaponRoot.childCount; i++)
                Destroy(_weaponRoot.GetChild(i).gameObject);

            InvokeWeaponStateChanged(EquippedWeapon, WeaponState.Unequipped);

            EquippedWeapon = null;
            EquippedWeaponView = null;
            _weaponInstance = null;
            inputController = null;
        }
    }
    
    void Update()
    {
        //if (Input.GetAxis("NextItem") > 0.1f)
        //{
        //    var item = (Weapon)_player.InventoryController.GetItemAt(i);

        //    if (item != null)
        //        EquipWeapon(item);

        //    if (i < _player.InventoryController.ItemsCount - 1)
        //        i++;
        //    else 
        //        i = 0;
        //}

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var item = (Weapon)_player.InventoryController.GetItemAt(0);

            if (item != null)
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

            if(sht >= 1f)
                AttackPressed();

            if (!_alternativeFirePressed && thrw >= 0.1f) // Первая реакция на нажатие (мышь зажата, дальше реакции не будет)
            {
                _alternativeFirePressed = true;
                AltPressed(true);

            }
            else if(_alternativeFirePressed && thrw < 0.1f) // Когда отпустили
            {
                _alternativeFirePressed = false;
                AltPressed(false);
            }
        }

        // Получаем значение ввода
        if (inputController != null)
        {
            input = inputController.GetInput();

            if (!_player.characterAnimator.facingRight)
            {
                var inp = input;
                inp.x *= -1f;
                input = inp;
            }
        }
    }

    void AttackPressed()
    {
        if (EquippedWeapon == null) return;

        if (_alternativeFirePressed && EquippedWeapon is WeaponThrow)
            // Если зажата альтернативная атака, и у нас метательное оружие
        {
            ThrowEquippedWeapon();
        }
        else
        {
            _weaponReadyToHit = false;
            _player.Attack(null);

            // Выполняем атаку
            EquippedWeapon.Attack(_player, _weaponInstance);

            if (EquippedWeaponView)
                EquippedWeaponView.Attack(_weaponInstance);

            StartCoroutine(TimeCoroutine(EquippedWeapon.Rate, delegate
            {
                _weaponReadyToHit = true;
            }));
        }
    }

    void AltPressed(bool pressed)
    {
        if (EquippedWeaponView)
        {
            if (EquippedWeaponView is WeaponThrowView) // Если вызвана альтернативная атака у бросающего оружия
            {
                _player.InputController.enabled = !pressed;
                input = Vector2.zero;

                if (pressed)
                    EquippedWeaponView.StartDraw();
                else
                    EquippedWeaponView.StopDraw();
            }
        }
    }

    void ThrowEquippedWeapon()
    {
        if (EquippedWeapon)
        {
            var throwWeapon = EquippedWeapon as WeaponThrow;

            var prefab = EquippedWeapon.GetPickablePrefab();
            var instance = Instantiate(prefab, _weaponRoot.position, Quaternion.identity) as GameObject;

            var pickableItem = instance.ForceComponent<PickableItem>();
            pickableItem.HoldedItem = EquippedWeapon;

            instance.rigidbody2D.velocity = input.normalized*throwWeapon.ThrowForce;
                //ApplyForceMode(input * throwWeapon.Range * throwWeapon.ThrowForce, ForceMode.Force);
            instance.rigidbody2D.AddTorque(throwWeapon.ThrowForce * Time.fixedDeltaTime * UnityEngine.Random.Range(-10f, 10f));
            
            _player.InputController.enabled = true;
            UneqipHoldedWeapon();

            _player.InventoryController.RemoveItem(throwWeapon);
        }
    }

    IEnumerator TimeCoroutine(float time, OnCompleted callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }
}
