namespace RonSijm.Statezor.Models;

public interface IEffectResult : IEffect
{
    object GetValue();
}