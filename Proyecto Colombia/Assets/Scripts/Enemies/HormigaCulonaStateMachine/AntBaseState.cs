using UnityEngine;

public abstract class AntBaseState
{
    public abstract void EnterState(AntStateManager _context, Rigidbody2D _rb);

    public abstract void UpdateState(AntStateManager _context, Rigidbody2D _rb);

}
