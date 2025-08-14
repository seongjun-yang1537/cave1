using System;

namespace PathX.TriangleGraph
{
    public readonly struct PTriangleGraphEdge : IEquatable<PTriangleGraphEdge>
    {
        public readonly Guid from;
        public readonly Guid to;
        public readonly float weight;

        public PTriangleGraphEdge(Guid from, Guid to, float weight = 0.0f)
        {
            this.weight = weight;
            this.from = from;
            this.to = to;
        }

        public bool Equals(PTriangleGraphEdge other)
        {
            return this.from == other.from && this.to == other.to;
        }

        public override bool Equals(object obj)
        {
            return obj is PTriangleGraphEdge other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (from, to).GetHashCode();
        }
    }
}