using UnityEngine;
public interface IState<T>
{
    void OperateEnter(T sender); //해당 state에 입장 했을 때 실행되는 함수
    void OperateUpdate(T sender);//해당 state일 때 Update(매 프레임마다) 실행되는 함수
    void OperateExit(T sender);//해당 state에서 벗어났을 때 실행되는 함수
}
