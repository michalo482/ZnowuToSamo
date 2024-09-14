using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{

    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private Slider slider;
    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image flaskImage;

    
    [Header("Currency info")]
    [SerializeField] private TextMeshProUGUI currentCurrency;
    [SerializeField] private float soulsAmount;
    [SerializeField] private float increasRate = 10;

    private SkillManager skills;
    
    // Start is called before the first frame update
    void Start()
    {
        if (playerStats != null)
        {
            playerStats.onHpChange += UpdateHpUi;
        }

        skills = SkillManager.instance;
    }

    // Update is called once per frame
    void Update()
    {

        UpdateCurrencyAmount();

        if (Input.GetKeyDown(KeyCode.LeftShift) && skills.dash.dashUnlocked)
        {
            SetCooldownOf(dashImage);
        }

        if (Input.GetKeyDown(KeyCode.Q) && skills.parry.parryUnlocked)
        {
            SetCooldownOf(parryImage);
        }

        if (Input.GetKeyDown(KeyCode.F) && skills.crystal.crystalUnlocked)
        {
            SetCooldownOf(crystalImage);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && skills.sword.swordUnlocked)
        {
            SetCooldownOf(swordImage);
        }

        if (Input.GetKeyDown(KeyCode.R) && skills.blackhole.blackholeUnlocked)
        {
            SetCooldownOf(blackholeImage);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
        {
            SetCooldownOf(flaskImage);
        }
        
        CheckCooldownOf(dashImage, skills.dash.cooldown);
        CheckCooldownOf(parryImage, skills.parry.cooldown);
        CheckCooldownOf(crystalImage, skills.crystal.cooldown);
        CheckCooldownOf(swordImage, skills.sword.cooldown);
        CheckCooldownOf(blackholeImage, skills.blackhole.cooldown);
        CheckCooldownOf(flaskImage, Inventory.instance._flaskCooldown);
    }

    private void UpdateCurrencyAmount()
    {
        if (soulsAmount < PlayerManager.instance.GetCurrency())
        {
            soulsAmount += Time.deltaTime * increasRate;
        }
        else
        {
            soulsAmount = PlayerManager.instance.GetCurrency();
        }

        currentCurrency.text = ((int)soulsAmount).ToString();
    }

    private void UpdateHpUi()
    {
        slider.maxValue = playerStats.GetMaxHp();
        slider.value = playerStats.currentHp;
    }

    private void SetCooldownOf(Image image)
    {
        if (image.fillAmount <= 0)
        {
            image.fillAmount = 1;
        }
    }

    private void CheckCooldownOf(Image image, float cooldown)
    {
        if (image.fillAmount > 0)
        {
            image.fillAmount -= 1 / cooldown * Time.deltaTime;
        }
    }
    
}
