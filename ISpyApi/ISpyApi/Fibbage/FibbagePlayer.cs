using ISpyApi.Models;

namespace ISpyApi.Fibbage;

public record FibbagePlayer(Game Game, string Username) : Player(Game, Username)
{

}