namespace RonSijm.Statezor.Components;

/// <inheritdoc />
/// <summary>
/// Component that exposes the protected methods to public ones
/// </summary>
public class PublicEventsComponent : ComponentBase
{
    public virtual void OnStateHasChanged()
    {
        StateHasChanged();
    }

    public async Task OnInvokeAsync(Action workItem)
    {
        await InvokeAsync(workItem);
    }

    public async Task OnInvokeAsync(Func<Task> workItem)
    {
        await InvokeAsync(workItem);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
    }

    public void DoOnInitialized()
    {
        OnInitialized();
    }

    public async Task DoInitializedAsync()
    {
        await OnInitializedAsync();
    }
}