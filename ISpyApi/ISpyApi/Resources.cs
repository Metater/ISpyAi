using ISpyApi.Factories;

namespace ISpyApi;

public record Resources(
    Random Random,
    ImageFactory ImageFactory,
    CodeFactory CodeFactory,
    Action<Guid, object> SendSchema
);