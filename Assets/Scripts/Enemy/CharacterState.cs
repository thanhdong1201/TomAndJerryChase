using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,       
    Chasing,    
    Catching, 
    Attacking,  
    Stunned,  
    Lost,      
    Flying
}
public interface CharacterState
{
    CharacterState GetId();
    void Enter(CharacterState state);
    void Update(CharacterState state);
    void Exit(CharacterState state);
}
