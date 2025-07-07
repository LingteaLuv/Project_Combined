using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChargingBarUI : MonoBehaviour
{
    public Slider _chargeSlider;
    public TextMeshProUGUI _chargeText;
    private ThrowableWeapon currentWeapon;

    private void OnEnable()
    {
        PlayerWeaponManager.OnRightWeaponChanged += HandleRightWeaponChanged;
    }

    private void OnDisable()
    {
        PlayerWeaponManager.OnRightWeaponChanged -= HandleRightWeaponChanged;
    }
    private void HandleRightWeaponChanged(WeaponBase weapon)
    {
        currentWeapon = weapon as ThrowableWeapon;
    }

    private void Update()
    {
        //패턴매칭 한 결과 아니면 null리턴
        if (currentWeapon == null)
        {
            Hide();
            return;
        }

        if (currentWeapon.isCharging)
        {
            float normalizedTime = currentWeapon.CurrentChargeNormalized;
            Show();
            UpdateChargeUI(normalizedTime);
        }
        else
        {
            Hide();
        }
    }

    private void UpdateChargeUI(float normalizedTime)
    {
       _chargeSlider.value = normalizedTime;
        _chargeText.text = $"투척 게이지 {Mathf.RoundToInt(normalizedTime * 100)}%";
    }

    private void Show()
    {
        _chargeSlider.gameObject.SetActive(true);
        _chargeText.gameObject.SetActive(true);
    }

    private void Hide()
    {
        _chargeSlider.gameObject.SetActive(false);
        _chargeText.gameObject.SetActive(false);
    }
}
