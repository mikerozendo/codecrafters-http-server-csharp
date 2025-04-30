using System.Text;
using codecrafters_http_server.src.Interfaces;
using codecrafters_http_server.src.Models;
using codecrafters_http_server.src.Models.RequestComponents;

namespace codecrafters_http_server.src.Resources;

public abstract class ResourceBase
{
    protected IRequest Request { get; private set; }
    protected ConfiguredResource ConfiguredResource { get; private set; }
    protected Line Line { get; private set; }
    protected Header Header { get; private set; }
    protected string[] IncommingRequestPathArgs { get; private set; }
    protected string[] ConfiguredResourcePathArgs { get; private set; }
    protected string FormatedPathWithParams { get; private set; } = string.Empty;
    public ResourceBase(IRequest request, ConfiguredResource configuredResource)
    {
        Request = request;
        ConfiguredResource = configuredResource;

        Line = request.GetRequestLine();
        Header = request.GetRequestHeaders();

        IncommingRequestPathArgs = GetIncommingRequestPathArgs();
        ConfiguredResourcePathArgs = GetConfiguredResourcePathArgs();
        FormatIfPathHasPathParams();
    }

    private string[] GetIncommingRequestPathArgs()
    {
        var requestedPathArgsFromHttpRequest = Line.Resource.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (requestedPathArgsFromHttpRequest is null)
            return [];

        if (requestedPathArgsFromHttpRequest.Length == 0)
            return [];

        return requestedPathArgsFromHttpRequest;
    }

    private string[] GetConfiguredResourcePathArgs()
    {
        var internalRequestedResourceArgs = ConfiguredResource.Path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (internalRequestedResourceArgs.Length == 0)
            return [];

        var doesInternalResourceHaveArgs = internalRequestedResourceArgs.Any(arg => arg.Equals("{path-param}"));
        if (!doesInternalResourceHaveArgs)
            return [];

        return internalRequestedResourceArgs;
    }

    protected bool DoesConfiguredResourceHasAnyArg()
    {
        return ConfiguredResourcePathArgs.Length > 0;
    }

    protected bool DoesIncommingRequestHasAnyArg()
    {
        return IncommingRequestPathArgs.Length > 0;
    }

    protected bool IncommingRequestAndConfiguredResourceHaveSameArgsCount()
    {
        return IncommingRequestPathArgs.Length == ConfiguredResourcePathArgs.Length;
    }

    private void FormatIfPathHasPathParams()
    {
        if (!IncommingRequestAndConfiguredResourceHaveSameArgsCount())
            return;

        if (!DoesIncommingRequestHasAnyArg() || !DoesConfiguredResourceHasAnyArg())
            return;

        StringBuilder formatedPath = new();
        for (int i = 0; i < ConfiguredResourcePathArgs.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(ConfiguredResourcePathArgs[i]))
                continue;

            if (ConfiguredResourcePathArgs[i].Equals("{path-param}"))
                formatedPath.Append($"{IncommingRequestPathArgs[i]}/");

            if (ConfiguredResourcePathArgs[i].Equals(IncommingRequestPathArgs[i], StringComparison.Ordinal))
                formatedPath.Append($"{ConfiguredResourcePathArgs[i]}/");
        }

        FormatedPathWithParams = formatedPath.ToString().TrimEnd('/');
    }

    private bool HasMatchOnParams()
    {
        if (!DoesConfiguredResourceHasAnyArg())
        {
            return Line.Resource.TrimEnd('/').Equals(ConfiguredResource.Path.TrimEnd('/'), StringComparison.OrdinalIgnoreCase);
        }

        return FormatedPathWithParams.Equals(string.Join("/", IncommingRequestPathArgs), StringComparison.OrdinalIgnoreCase);
    }

    protected bool HasMatchingHttpMethod() => ConfiguredResource.HttpMethod == Request.GetRequestLine().HttpMethod;

    public virtual bool HasMatchingRoute()
    {
        if (!HasMatchingHttpMethod())
            return false;

        if (DoesConfiguredResourceHasAnyArg())
            if (!IncommingRequestAndConfiguredResourceHaveSameArgsCount())
                return false;

        if (!HasMatchOnParams())
            return false;

        return true;
    }
}
