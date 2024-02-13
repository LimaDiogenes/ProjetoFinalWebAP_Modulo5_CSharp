using System;

namespace Entities;

public interface IEntity
{
    public int Id { get; }
    string Name { get; }
}
