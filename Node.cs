using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// This is a script for a node system I built into a VR spellcasting duel game Titled "Wizard's Wrath: Duellists" The code was relatively simple, but is a good example of fairly clean and easy to navigate code
// Obviously there are some minor issues with the way that player input is parsed, especially using an analog stick,
// outside of this code some measures were taken to ensure that the player's input was parsed correctly however there is still a lot I could improve on here
// regardless, this is just a simple example of my style for creating a specialized node system for a game.


public enum Direction
{
    FORWARD,
    LEFT,
    BACK,
    RIGHT,
    NIL

}

[System.Serializable]
public struct Connection
{
    [SerializeField] public Node point;
    [SerializeField] public List<Direction> directions;
}


public class Node : MonoBehaviour
{

    [SerializeField] List<Connection> connections;
    [SerializeField] Player assignedPlayer;


    void Start()
    {
        //disabling mesh renderer at runtime
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
    private Connection nullConnection = new Connection {point = null, directions = new List<Direction> {Direction.NIL}};
    //Code Section: return node transform to player

    public Transform getNodeTransform()
    {
        return this.transform;
    }

    public Node getNode()
    {
        return this;
    }
    
    public void moveToNewNode(Vector2 input)
    {
        List<Direction> directions = parseDirectionFromInput(input);
        var destNode = findDestinationNode(directions);
        assignedPlayer.currentNode = destNode != null ? destNode : this;
        Debug.Log("Moved to node: " + assignedPlayer.currentNode.getNodeTransform().position.ToString());
    }

    Node findDestinationNode(List<Direction> directions)
    {
        if(directions.Count == 1)
        {
            return getConnectionByDirection(directions[0]).point;
        }
        else
        {
            return getConnectionByDirection(directions[0], directions[1]).point;
        }
    }


    //Code Section: get direction from input


    List<Direction> parseDirectionFromInput(Vector2 stickInput)
    {
        return generateDirectionList(stickInput.x, stickInput.y);   
    }

    List<Direction> generateDirectionList(float x, float y)
    {
        List<Direction> directions = prioritizeDirections(x, y);
        removeNils(directions);
        return directions;
    }

    void removeNils(List<Direction> directions)
    {
        for(int i = 0; i < directions.Count; i++)
        {
            if(directions[i] == Direction.NIL)
            {
                directions.RemoveAt(i);
            }
        }
    }

    List<Direction> prioritizeDirections(float x, float y)
    {
        if(Math.Abs(x) > Math.Abs(y))
        {
            return new List<Direction> {parseDirectionX(x), parseDirectionY(y)};
        }
        else
        {
            return new List<Direction> {parseDirectionY(y), parseDirectionX(x)};
        }
    }

    Direction parseDirectionX(float stickX)
    {
        if (stickX > assignedPlayer.deadzone)
        {
            return Direction.RIGHT;
        }
        else if (stickX < assignedPlayer.deadzone * -1)
        {
            return Direction.LEFT;
        }
        else
        {
            return Direction.NIL;
        }
    }

    Direction parseDirectionY(float stickY)
    {
        if (stickY > assignedPlayer.deadzone)
        {
            return Direction.FORWARD;
        }
        else if (stickY < assignedPlayer.deadzone * -1)
        {
            return Direction.BACK;
        }
        else
        {
            return Direction.NIL;
        }
    }

    //Code Section: get connection by direction
    Connection getConnectionByDirection(Direction travelDirection)
    {
       Connection connection = nullConnection;
        for(int i = 0; i < connections.Count; i++)
        {
            if(connections[i].directions.Contains(travelDirection))
            {
              connection = connections[i];
            }
        }
        return connection;
    } 
    //override for two directions - diagonal movement
    Connection getConnectionByDirection(Direction fallbackDirection, Direction secondaryDirection)
    {
       Connection connection = getConnectionByDirection(fallbackDirection);
        for(int i = 0; i < connections.Count; i++)
        {
            if(connections[i].directions.Contains(fallbackDirection) && connections[i].directions.Contains(secondaryDirection))
            {
               connection = connections[i];
            }
        }
        if (connection.point == null)
        {
           connection = getConnectionByDirection(secondaryDirection);
        }
        return connection;
    }
}
