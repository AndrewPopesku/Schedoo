﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Schedoo.Server.Models;

public class TimeSlot : IComparable<TimeSlot>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string ClassName { get; set; }
    
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    

    public override bool Equals(object? obj)
    {
        if (obj is TimeSlot other)
        {
            return this.Id == other.Id;
        }

        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            
            hash = hash * 23 + this.Id.GetHashCode();
            hash = hash * 23 + this.ClassName.GetHashCode();

            return hash;
        }
    }
    
    public int CompareTo(TimeSlot other)
    {
        return this.Id.CompareTo(other.Id);
    }
}