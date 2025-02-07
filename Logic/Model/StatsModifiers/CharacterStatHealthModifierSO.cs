using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStatHealthModifierSo", menuName = "Scriptable Objects/CharacterStatHealthModifierSo")]
public class CharacterStatHealthModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float value)
    {
       HealthComponent health = character.GetComponent<HealthComponent>();

        if (health != null)
            health.RestoreHealth((int)value);
    }
}
