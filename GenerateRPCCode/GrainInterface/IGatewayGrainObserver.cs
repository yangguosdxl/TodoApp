using Orleans;
using System.Threading.Tasks;

namespace GrainInterface
{
    public interface IGatewayGrainObserver : IGrainObserver
    {
        void Send(int iProtocolID, int iCommunicateID, byte[] bytes, int start, int len);
    }
}
