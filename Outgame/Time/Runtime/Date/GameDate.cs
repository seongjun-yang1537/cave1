using System;
using Corelib.Utils;

namespace Domain
{
    [Serializable]
    public class GameDate : IComparable<GameDate>
    {
        private readonly int MAX_WEEK_DAYS = ExEnum.Count<GameDayOfWeek>();

        private readonly int _totalDays;

        public int TotalDays => _totalDays;
        public int Week => (_totalDays / MAX_WEEK_DAYS) + 1;
        public GameDayOfWeek Day => (GameDayOfWeek)(_totalDays % MAX_WEEK_DAYS);
        public bool IsWeekend => Day == GameDayOfWeek.Saturday || Day == GameDayOfWeek.Sunday;
        public bool IsWeekday => !IsWeekend;

        public GameDate(int totalDaysSinceStart)
        {
            _totalDays = totalDaysSinceStart >= 0 ? totalDaysSinceStart : 0;
        }

        public static GameDate operator +(GameDate date, int days)
        {
            return new GameDate(date._totalDays + days);
        }

        public static GameDateTime operator +(GameDate a, GameDateTime b)
        {
            int newTotalDays = a.TotalDays + b.Date.TotalDays;
            return new GameDateTime(newTotalDays, b.Time);
        }

        public static GameDate operator -(GameDate date, int days)
        {
            return new GameDate(date._totalDays - days);
        }

        public static int operator -(GameDate a, GameDate b)
        {
            return a._totalDays - b._totalDays;
        }

        public static GameDate operator ++(GameDate date)
        {
            return new GameDate(date._totalDays + 1);
        }

        public static bool operator >(GameDate a, GameDate b) => a._totalDays > b._totalDays;
        public static bool operator <(GameDate a, GameDate b) => a._totalDays < b._totalDays;
        public static bool operator >=(GameDate a, GameDate b) => a._totalDays >= b._totalDays;
        public static bool operator <=(GameDate a, GameDate b) => a._totalDays <= b._totalDays;
        public static bool operator ==(GameDate a, GameDate b) => a._totalDays == b._totalDays;
        public static bool operator !=(GameDate a, GameDate b) => a._totalDays != b._totalDays;

        public override string ToString() => $"Week {Week}, {Day}";

        public int CompareTo(GameDate other) => _totalDays.CompareTo(other._totalDays);

        public override bool Equals(object obj) => obj is GameDate other && this == other;

        public override int GetHashCode() => _totalDays.GetHashCode();
    }
}