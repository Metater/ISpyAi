using ISpyApi.Models;

namespace ISpyApi.Fibbage;

public record FibbagePlayer(Game Game, bool IsHost, string Username) : Player(Game, IsHost, Username)
{

}