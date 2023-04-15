﻿using System.ComponentModel.DataAnnotations;

namespace AutoUpdater.Abstractions.Models
{
    public class AppVersion : IComparable<AppVersion>
    {
        public required int Minor { get; set; }

        public required int Major { get; set; }

        public required int Revision { get; set; }

        [Required]
        public string Raw => ToString();

        public int CompareTo(AppVersion? other)
        {
            if (other == null) return 1;
            if (ReferenceEquals(this, other)) return 0;
            if (Major != other.Major) return 1;
            if (Minor != other.Minor) return 1;
            if (Revision != other.Revision) return 1;
            return -1;

        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Revision}";
        }

        public static implicit operator AppVersion(string str)
        {
            var versions = str.Split('.');
            return new AppVersion
            {
                Major = int.Parse(versions[0]),
                Minor = int.Parse(versions[1]),
                Revision = int.Parse(versions[2]),
            };
        }

    }
}