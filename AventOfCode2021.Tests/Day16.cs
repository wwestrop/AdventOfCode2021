using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AventOfCode2021.Tests
{
    public class Day16
    {
        [Fact]
        public void ParseSimplePacket()
        {
            byte[] binary = Convert.FromHexString("D2FE28");

            var (version, typeId) = ParseHeader(binary);

            if (typeId == Packet.Type.LiteralValue)
            {
                var literal = ParseLiteralValue(binary, startingFrom: 6);
                Assert.Equal((ulong)2021, literal);
            }
        }

        private ulong ParseLiteralValue(ReadOnlySpan<byte> binary, int startingFrom)
        {
            byte nibble = 0;
            bool leadingOne = false;

            ulong result = 0;

            do
            {
                (leadingOne, nibble) = ReadNibble(binary, startingFrom);
                result <<= 4;
                result |= nibble;
                startingFrom += 5;

            } while (leadingOne);

            return result;
        }

        [Fact]
        public void ParseHeaderTests()
        {
            byte[] binary = Convert.FromHexString("D2FE28");

            var (version, typeId) = ParseHeader(binary);

            Assert.Equal(6, version);
            Assert.Equal(Packet.Type.LiteralValue, typeId);
        }

        private static (int version, Packet.Type typeId) ParseHeader(ReadOnlySpan<byte> binary)
        {
            var header = BitConverter.ToUInt16(binary[0..2]);

            var packetVersionMask = 0b11100000;
            var typeIdMask = 0b00011100;

            var version = (header & packetVersionMask) >> 5;
            var typeId = (header & typeIdMask) >> 2;

            return (version, (Packet.Type)typeId);
        }

        [Theory]
        [InlineData(0b110100101111111000101000, 6, true, 0b0111)]
        [InlineData(0b110100101111111000101000, 11, true, 0b1110)]
        [InlineData(0b110100101111111000101000, 16, false, 0b0101)]
        public void ReadNibbleTest(ulong binary, int from, bool expectedLeadBit, byte expectedNibbleValue)
        {
            var itGivesMeTheBytesBackwardsBecauseOfCourseItDoes = BitConverter.GetBytes(binary)
                .Reverse()
                .SkipWhile(b => b == 0)
                .ToArray();

            var (lead, value) = ReadNibble(itGivesMeTheBytesBackwardsBecauseOfCourseItDoes, from);

            Assert.Equal(expectedLeadBit, lead);
            Assert.Equal(expectedNibbleValue, value);
        }

        private (bool trailingNibble, byte value) ReadNibble(ReadOnlySpan<byte> binary, int from)
        {
            // firstly, work out in which byte this nibble starts and ends
            var startingByte = from / 8;
            var offsetIntoFirstByte = from % 8;

            // how many bits left in this byte after we start? if < 5, we spill into the next byte
            var remainingBitsInByte = 8 - (from % 8);


            var byteVal = binary[startingByte];

            // push out anything in the upper bits that doesn't correspond to our number
            byte msbMask = (byte)(~(byte)(0b11111111 << (8 - offsetIntoFirstByte)));
            byteVal &= msbMask;     // |= ~msbMask

            var bitsRemainingToRead = 5 - remainingBitsInByte;
            if (bitsRemainingToRead > 0)
            {
                var nextByteVal = binary[startingByte + 1];

                // cut off all the lower bits other than the ones we're interested in
                byte lsbMask = (byte)(~(byte)(0b11111111 >> bitsRemainingToRead));
                nextByteVal &= lsbMask;

                // move everything up in the resultant byte to make way for these new bits
                byteVal <<= bitsRemainingToRead;

                // move second byte into the correct position too
                nextByteVal >>= 8 - bitsRemainingToRead;

                // add them together
                byteVal = (byte)(byteVal | nextByteVal);
            }
            else
            {
                byteVal >>= 0 - bitsRemainingToRead;
            }

            var leadingBitOne = ((byteVal & 0b10000) >> 4) == 1;
            return (leadingBitOne, (byte)(byteVal & 0b00001111));
        }

        private record class Packet
        {
            public readonly int Version;
            public readonly Type TypeId;
            public IReadOnlyList<Packet> Children;

            public Packet()
            {

            }

            public enum Type
            {
                LiteralValue = 4,
            }
        }

        //public record class LiteralPacket
        //{
        //    public LiteralPacket
        //}
    }
}
