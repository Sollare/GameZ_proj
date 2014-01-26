using System.Collections.Generic;
using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
public class AttackStateController : StateMachineController
{
    private List<UnityEditorInternal.State> statesChanged;
    private float _param;

    public override void Do(Animator animator, object param)
    {
        if (animator == null || !(param is float))
        {
            Debug.LogWarning("Animator is null");
            return;
        }

        if (statesChanged != null)
        {
            Debug.LogWarning("StateController already did job");
            return;
        }

        if ((_param = (float)param) <= 0)
        {
            Debug.LogWarning("Invalid parameter value = " + _param);
            return;
        }

        statesChanged = new List<UnityEditorInternal.State>();
        _animator = animator.runtimeAnimatorController as UnityEditorInternal.AnimatorController;;
        
        // Number of layers:
        int layerCount = _animator.layerCount;

        // Names of each layer:
        for (int i = 0; i < _animator.layerCount; i++)
        {
            //Debug.Log(string.Format("Layer {0}: {1}", i, ac.GetLayer(i).name));
            var stateMachine = _animator.GetLayer(i).stateMachine;

            for (int j = 0; j < stateMachine.stateCount; j++)
            {
                var state = stateMachine.GetState(j);
                //Debug.Log(string.Format("\tState {0}: {1}", j, state.name));

                if (state.name.Contains("Attack"))
                {
                    //Debug.Log("\t Changing speed from " + state.speed);
                    statesChanged.Add(state);
                    state.speed *= _param;
                }
            }
        }
    }

    public override void Undo()
    {
        if (statesChanged == null)
        {
            Debug.LogWarning("AttackStateController did not job yet");
            return;
        }

        foreach (var state in statesChanged)
        {
            state.speed /= _param;
        }
    }
}
#endif
