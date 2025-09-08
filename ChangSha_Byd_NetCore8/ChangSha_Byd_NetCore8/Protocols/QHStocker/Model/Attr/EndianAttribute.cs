

namespace ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Attr
{
    /// <summary>
    /// 用来将大端和小端进行互换
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EndianAttribute : Attribute
    {
        public Endianness Endianness { get; private set; }

        public EndianAttribute(Endianness endianness)
        {
            Endianness = endianness;
        }
    }

    public enum Endianness
    {
        BigEndian,
        LittleEndian,
        BigEndianArray,
        LittleEndianArray
    }
}
