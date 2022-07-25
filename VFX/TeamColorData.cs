using UnityEngine;

public class TeamColorData
{
    private static Color[] _teamColors = new Color[] { Color.white, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow, Color.yellow };

    public static Color GetTeamColor(int team)
    {
        if(team == -1)
        {
            return Color.red;
        }

        return _teamColors[team];
    }
}
