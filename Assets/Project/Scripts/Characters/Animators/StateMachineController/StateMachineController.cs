using UnityEngine;
using System.Collections;

public abstract class StateMachineController
{
#if UNITY_EDITOR
    protected UnityEditorInternal.AnimatorController _animator;
#endif

    public abstract void Do(Animator animator, object param);

    public abstract void Undo();
}
