using UnityEngine;

public interface INodeInput
{
    bool AcceptsCable(DataCable cable, Transform port);
    bool IsPortOccupied(DataCable cable, Transform port);
    void ConnectCable(DataCable cable, Transform port);
    void DisconnectCable(DataCable cable, Transform port);
}