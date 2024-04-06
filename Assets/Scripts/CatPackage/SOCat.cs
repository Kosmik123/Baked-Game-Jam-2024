using System.Collections;
using System.Collections.Generic;
using CatPackage;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[CreateAssetMenu(fileName="new Cat", menuName="Custom/Cat")]
public class SOCat : ScriptableObject
{
    [Header("Display info")] [SerializeField]
    private Sprite catSprite;
    [SerializeField] private string catName;
    [SerializeField] private Color catColor;
    [SerializeField] private Color catEyeColor;
    [SerializeField] private Color catNoseColor;
    [SerializeField] private ECatBreed catBreed;
    [Space]
    [Header("Cat settings")]
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private int damage;
    [SerializeField] private float cooldown;
    [SerializeField] private int health;

    
    public void SpawnAttackPrefab(Vector3 shootPos, Transform self, int catLevel)
    {
        var mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var lookDirection = (mousePosWorld - self.position).normalized;
        var goalRotation = Quaternion.LookRotation(lookDirection, Vector3.forward);
        var attackObject = Instantiate(attackPrefab, shootPos, goalRotation);
        var attackScript = attackObject.GetComponent<AttackObject>();
        attackScript.Attack(self, Mathf.CeilToInt(
            damage + (damage / 10f) * catLevel));
    }

    public void SetMaterialColor(SpriteRenderer spriteRenderer)
    {
        // red -> nos
        // blue -> fur
        // green -> eyes
        spriteRenderer.sprite = catSprite;
        var material = spriteRenderer.material;
        material.SetColor("_Red", catNoseColor);
        material.SetColor("_Blue", catColor);
        material.SetColor("_Green", catEyeColor);
    }
    public void SetMaterialColor(Image image)
    {
        // red -> nos
        // blue -> fur
        // green -> eyes
        image.sprite = catSprite;
        var material = image.material;
        material.SetColor("_Red", catNoseColor);
        material.SetColor("_Blue", catColor);
        material.SetColor("_Green", catEyeColor);
    }

    public SCatDisplayInfo GetDisplayInfo()
    {
        return new SCatDisplayInfo()
        {
            catBreed = catBreed,
            catName = catName,
            catSprite = catSprite,
            catTier = (ECatTier)Mathf.CeilToInt((int)catBreed / 2f),
            catColor = catColor,
            catEyeColor = catEyeColor,
            catNoseColor = catNoseColor,
        };
    }

    public SCatSpecificInfo GetSpecificInfo(int catLevel)
    {
        return new SCatSpecificInfo()
        {
            cooldown = cooldown,
            damage = Mathf.CeilToInt(damage + (damage / 100f * catLevel)),
            maxHealth = Mathf.CeilToInt(health + (health / 100f * catLevel)),
        };
    }
}

public struct SCatDisplayInfo
{
    public Sprite catSprite;
    public ECatBreed catBreed;
    public ECatTier catTier;
    public string catName;
    public Color catColor;
    public Color catEyeColor;
    public Color catNoseColor;
}

public struct SCatSpecificInfo
{
    public int damage;
    public float cooldown;
    public int maxHealth;
}
