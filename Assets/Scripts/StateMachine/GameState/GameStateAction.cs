using GameplayAbilitySystem;
using UnityEngine;

[CreateAssetMenu(menuName = "Game State/ActionState")]
public class GameStateAction : State
{
    public System.Type AbilityType;
    public override void Enter() 
    {
        GameStateManager.Instance.GetFolowMouse().gameObject.SetActive(true);
        GameStateManager.Instance.GetFolowMouse().OnCellSelected += OnCellSelected;
        GameStateManager.Instance.GetFolowMouse().AbilityType = AbilityType;
        Debug.Log("Enter action");
    }
    public override void Tick() 
    {
        Debug.Log("In Action State");
        if(Input.GetMouseButtonUp(1))
        {
            GameStateManager.Instance.ShouldDoAction = false;
            Debug.Log("Canceled action");
        }
    }

    private void OnCellSelected(Vector3Int pos)
    {
        bool FirstTime = !GameStateManager.Instance.HasDoneAction;

        GameStateManager.Instance.HasDoneAction = true;
        GameStateManager.Instance.ShouldDoAction = false;
        GameStateManager.Instance.PlayerAgent.AppendInput(AbilityType, (Vector2Int)pos);
        Ticker.Instance.TickCurrent(FirstTime);
        Debug.Log("Action done");
    }

    public override void Exit() 
    {
        GameStateManager.Instance.GetFolowMouse().OnCellSelected -= OnCellSelected;
        GameStateManager.Instance.GetFolowMouse().gameObject.SetActive(false);
    }
    public override State SelectTransition() 
    {
        if(GameStateManager.Instance.ShouldEndTurn)
        {
            GameStateManager.Instance.ShouldDoAction = false;
            return GameStateManager.Instance.EndTurnState;
        }

        if(!GameStateManager.Instance.ShouldDoAction)
        {
            return GameStateManager.Instance.IdleState;
        }
        return null;
    }

    protected override void Initialize(){}
}
