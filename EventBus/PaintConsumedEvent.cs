using EventBus;
using PaintScripts;

namespace EventChannels
{
    public struct PaintConsumedEvent : IEvent
    {
        public readonly PaintCan PaintCan;
        public PaintConsumedEvent(PaintCan paintCan)
        {
            PaintCan = paintCan;
        }
    }
}