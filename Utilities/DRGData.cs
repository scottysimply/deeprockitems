using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace deeprockitems.Utilities
{
    public class DRGData
    {
        public static Color GetTeamColor(int team)
        {
            Color color = team switch
            {
                1 => new(.9f, .3f, .3f, .85f), // Red
                2 => new(.3f, .9f, .3f, .85f), // Green
                3 => new(.4f, .8f, .8f, .8f), // Blue
                4 => new(.8f, .8f, .25f, .85f), // Yellow
                5 => new(.8f, .25f, .75f, .85f), // Pink
                _ => new(.95f, .95f, .95f, .8f) // No team
            };
            return color;
        }
    }
}
