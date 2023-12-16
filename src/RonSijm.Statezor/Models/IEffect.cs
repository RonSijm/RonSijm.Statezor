namespace RonSijm.Statezor.Models;

public interface IEffect
{
    Func<Type, bool> TypeCriteria { get; set; }
}