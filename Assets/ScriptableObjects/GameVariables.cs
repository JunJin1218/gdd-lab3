using UnityEngine;

[CreateAssetMenu(fileName = "GameVariables", menuName = "Scriptable Objects/GameVariables")]
public class GameVariables : ScriptableObject
{
    public float PlayerAccelX = 20f;
    public float PlayerDecelX = 40f;
    public float PlayerMaxSpeedX = 20f;
    public float PlayerJumpForce = 8f;
    public float PlayerAttackForce = 20f;
    public float PlayerGroundCheckDistance = 0.8f;
    public float PlayerDashForce = 15f;
}
