using Unity.Entities;

namespace ie.TUDublin.GE2.Components.Statemachine {
    
    /// <summary>
    /// States a ships Statemachine can have
    /// </summary>
    
    public struct AttackingState : IComponentData { }
    
    public struct FleeState : IComponentData { }

    public struct PursueState : IComponentData { }
    
    public struct SearchState : IComponentData { }
    
}