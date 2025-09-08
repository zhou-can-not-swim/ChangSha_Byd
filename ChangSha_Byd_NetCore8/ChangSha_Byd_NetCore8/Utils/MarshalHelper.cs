using ChangSha_Byd_NetCore8.Protocols.QHStocker.Model.Attr;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ChangSha_Byd_NetCore8.Utils
{
    public static class MarshalHelper
    {
        private static void RespectEndianness(Type type, byte[] data, int startOffset = 0)
        {
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo fieldInfo in fields)
            {
                Type fieldType = fieldInfo.FieldType;
                if (fieldInfo.IsStatic || fieldType == typeof(string))
                {
                    continue;
                }

                int num = Marshal.OffsetOf(type, fieldInfo.Name).ToInt32();
                int num2 = startOffset + num;
                bool isArray = fieldType.IsArray;
                bool num3 = !isArray && (from subField in fieldType.GetFields()
                                         where !subField.IsStatic
                                         select subField).Count() == 0;
                EndianAttribute customAttribute = fieldInfo.GetCustomAttribute<EndianAttribute>(inherit: false);
                if (num3 || fieldType.IsEnum)
                {
                    if (customAttribute != null && ((customAttribute.Endianness == Endianness.BigEndian && BitConverter.IsLittleEndian) || (customAttribute.Endianness == Endianness.LittleEndian && !BitConverter.IsLittleEndian)))
                    {
                        Type t = (fieldType.IsEnum ? Enum.GetUnderlyingType(fieldType) : fieldType);
                        Array.Reverse(data, num2, Marshal.SizeOf(t));
                    }

                    continue;
                }

                RespectEndianness(fieldType, data, num2);
                if (!isArray || customAttribute == null || ((customAttribute.Endianness != Endianness.BigEndianArray || !BitConverter.IsLittleEndian) && (customAttribute.Endianness != Endianness.LittleEndianArray || BitConverter.IsLittleEndian)))
                {
                    continue;
                }

                MarshalAsAttribute customAttribute2 = fieldInfo.GetCustomAttribute<MarshalAsAttribute>(inherit: false);
                if (customAttribute2 == null)
                {
                    continue;
                }

                int sizeConst = customAttribute2.SizeConst;
                if (sizeConst > 0)
                {
                    int num4 = Marshal.SizeOf(fieldType.GetElementType());
                    int num5 = num2;
                    for (int j = 0; j < sizeConst; j++)
                    {
                        Array.Reverse(data, num5, num4);
                        num5 += num4;
                    }
                }
            }
        }

        public static ushort ReverseBytes(ushort value)
        {
            return (ushort)((uint)((value & 0xFF) << 8) | ((uint)(value & 0xFF00) >> 8));
        }

        public static uint ReverseBytes(uint value)
        {
            return ((value & 0xFF) << 24) | ((value & 0xFF00) << 8) | ((value & 0xFF0000) >> 8) | ((value & 0xFF000000u) >> 24);
        }

        public static ulong ReverseBytes(ulong value)
        {
            return ((value & 0xFF) << 56) | ((value & 0xFF00) << 40) | ((value & 0xFF0000) << 24) | ((value & 0xFF000000u) << 8) | ((value & 0xFF00000000L) >> 8) | ((value & 0xFF0000000000L) >> 24) | ((value & 0xFF000000000000L) >> 40) | ((value & 0xFF00000000000000uL) >> 56);
        }

        public static T BytesToStruct<T>(byte[] rawData)
        {
            T val = default(T);
            RespectEndianness(typeof(T), rawData);
            GCHandle gCHandle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
            try
            {
                return (T)Marshal.PtrToStructure(gCHandle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                gCHandle.Free();
            }
        }

        public static byte[] StructToBytes<T>(T data)
        {
            byte[] array = new byte[Marshal.SizeOf(data)];
            GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
            try
            {
                IntPtr ptr = gCHandle.AddrOfPinnedObject();
                Marshal.StructureToPtr(data, ptr, fDeleteOld: false);
            }
            finally
            {
                gCHandle.Free();
            }

            RespectEndianness(typeof(T), array);
            return array;
        }

        public static ushort[] BytesToUShorts(byte[] source)
        {
            ushort[] array = new ushort[source.Length / 2];
            Buffer.BlockCopy(source, 0, array, 0, source.Length);
            return array;
        }

        public static byte[] UShortsToBytes(ushort[] source)
        {
            byte[] array = new byte[source.Length * 2];
            Buffer.BlockCopy(source, 0, array, 0, source.Length * 2);
            return array;
        }

        public static IEnumerable<string> ParseEnumNames<TEnum>(TEnum flags, TEnum none) where TEnum : Enum
        {
            return from i in Enum.GetValues(typeof(TEnum)).OfType<TEnum>()
                   where !i.Equals(none) && flags.HasFlag(i)
                   select Enum.GetName(typeof(TEnum), i);
        }
    }
}
