using DG.Tweening;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AntIdleState : AntBaseState
{
    bool roll = true;
    Tween path;
    Vector2 startPos, targetPos;

    public float duration = 2f;


    public override void EnterState(AntStateManager _context, Rigidbody2D _rb)
    {
        roll = true;
        _rb.velocity = Vector2.zero;
        startPos = _context.StartingPosition;
    }
    public override void UpdateState(AntStateManager _context, Rigidbody2D _rb)
    {
        if(_context._contextSteering.TargetCount() > 0)
        {
            if (path != null)
            {
                path.Kill();
                path = null;
            }
            _context._contextSteering.ChooseTarget(0);
            _context.SwitchState(_context._chasingState);
            //This translates to...
            //If the player(s) enter(s) to the detection zone, asign (the closest one) as target...
            //And change to the "chasing state"
        }
        else
        {
            if (roll)
            {
                targetPos = GetRandomPointInCircle(startPos, 3);
                path = _rb.DOMove(targetPos, duration);
                roll = false;
            }
        }
        path.OnComplete(OnPathComplete);
    }
    public Vector2 GetRandomPointInCircle(Vector2 center, float radius)
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        float distance = Random.Range(0f, radius);
        return center + new Vector2(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle));
    }

    void OnPathComplete()
    {
        path.Kill();
        path = null;
        roll = true;
    }
}
