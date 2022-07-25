using UnityEngine;

public class Engaging : IEngageMovment
{
    private AIController _controller;
    private float _maxRange;

    public Engaging(float maxRange)
    {
        _maxRange = maxRange;
    }

    public void Init(AIController controller)
    {
        _controller = controller;
    }

    public void Move(Unit target)
    {
        if(target == null)
        {
            return;
        }
        if(Vector2.Distance(target.Position2D, _controller.AttachedUnit.Position2D) > _maxRange)
        {
            _controller.MoveToPoint(target.Position2D);
        }
        else
        {
            _controller.MoveToDirection(_controller.AttachedUnit.Position2D - target.Position2D);
        }
    }
}