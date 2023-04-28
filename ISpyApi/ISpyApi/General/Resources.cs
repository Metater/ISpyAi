using ISpyApi.General;

namespace ISpyApi;

public record Resources(
    Random Random,
    ImageFactory ImageFactory,
    CodeFactory CodeFactory,
    Action<Guid, object> SendSchema
);