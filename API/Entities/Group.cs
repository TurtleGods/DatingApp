using System;
using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class Group(string name)
{
    [Key]
    public required string Name { get; set; } = name;
}
