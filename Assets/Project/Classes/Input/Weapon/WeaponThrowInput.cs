using UnityEngine;
using System.Collections;

public class WeaponThrowInput : WeaponInput
{
    private float _sensivity;

    private float _minAngle, _maxAngle;
    private float _angle;

    public WeaponThrowInput(Weapon weapon, float sensivity, float minAngle, float maxAngle) : base(weapon)
    {
        _angle = _minAngle;

        _minAngle = minAngle;
        _maxAngle = maxAngle;

        _sensivity = sensivity;
    }

    public override Vector2 GetInput()
    {
        float input = _sensivity*Input.GetAxis("Vertical") * Time.deltaTime;
        _angle += input;
        _angle = Mathf.Clamp(_angle, _minAngle, _maxAngle);
        var res = new Vector2(weapon.Range * Mathf.Cos(_angle * Mathf.Deg2Rad), weapon.Range * Mathf.Sin(_angle * Mathf.Deg2Rad));

        return res;
    }
}
