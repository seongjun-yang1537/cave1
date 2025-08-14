using System;

namespace Domain
{
    [Serializable]
    public struct GameDateTime : IComparable<GameDateTime>
    {
        public static float HoursPerDay = 24f;

        public GameDate Date { get; private set; }
        public float Time { get; private set; }

        public int Hour => (int)(Time * HoursPerDay);
        public float TotalHours => (Date.TotalDays * HoursPerDay) + (Time * HoursPerDay);

        public GameDateTime(GameDate date, float time = 0f)
        {
            int extraDays = (int)time;
            Date = date + extraDays;
            Time = time - extraDays;
        }

        public GameDateTime(int totalDays, float time = 0f)
        {
            int extraDays = (int)time;
            Date = new GameDate(totalDays + extraDays);
            Time = time - extraDays;
        }

        public GameDateTime AddHours(float hours)
        {
            float timeAsDayFraction = hours / HoursPerDay;
            return AddTime(timeAsDayFraction);
        }

        public GameDateTime AddTime(float time)
        {
            float newTime = this.Time + time;
            int daysToAdd = (int)newTime;

            return new GameDateTime(this.Date + daysToAdd, newTime - daysToAdd);
        }

        public static GameDateTime operator +(GameDateTime dt, float hours)
        {
            return dt.AddHours(hours);
        }

        public static GameDateTime operator +(GameDateTime a, GameDate b)
        {
            int newTotalDays = a.Date.TotalDays + b.TotalDays;
            return new GameDateTime(newTotalDays, a.Time);
        }

        public static GameDateTime operator -(GameDateTime dt, float hours)
        {
            return dt.AddHours(-hours);
        }

        public static TimeSpan operator -(GameDateTime a, GameDateTime b)
        {
            return TimeSpan.FromHours(a.TotalHours - b.TotalHours);
        }

        public static bool operator >(GameDateTime a, GameDateTime b) => a.TotalHours > b.TotalHours;
        public static bool operator <(GameDateTime a, GameDateTime b) => a.TotalHours < b.TotalHours;
        public static bool operator >=(GameDateTime a, GameDateTime b) => a.TotalHours >= b.TotalHours;
        public static bool operator <=(GameDateTime a, GameDateTime b) => a.TotalHours <= b.TotalHours;

        public static bool operator ==(GameDateTime a, GameDateTime b) => a.Equals(b);
        public static bool operator !=(GameDateTime a, GameDateTime b) => !a.Equals(b);

        public override string ToString() => $"{Date.ToString()}, {Hour:D2}h";
        public int CompareTo(GameDateTime other) => TotalHours.CompareTo(other.TotalHours);
        public override bool Equals(object obj) => obj is GameDateTime other && this.TotalHours == other.TotalHours;
        public override int GetHashCode() => TotalHours.GetHashCode();
    }
}