using UnityEngine;

public class InputClass : MonoBehaviour
{
    InputMap Map;

    #region Importants

    private void Awake() => Map = new InputMap();
    private void OnEnable() => Map.Enable();
    private void OnDisable() => Map.Disable();

    #endregion

    #region Keys

    public Vector2 MoveAxis() => Map.Player.MoveAxis.ReadValue<Vector2>();
    public bool Jump() => Map.Player.Jump.ReadValue<float>() != 0;
    public bool Attack() => Map.Player.Attack.ReadValue<float>() != 0;

    #endregion
}