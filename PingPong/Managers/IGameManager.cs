using PingPong.Models;

namespace PingPong.Managers
{
    public interface IGameManager
    {
        void AwardPoint(Point point);

        PlayerId GetNextToServe();
    }
}
