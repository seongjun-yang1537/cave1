using UnityEngine;

namespace PathX
{
    public interface IPathXWorldProvider
    {
        PathXEngine PathXEngine { get; set; }
        object WorldData { get; }
    }
}
