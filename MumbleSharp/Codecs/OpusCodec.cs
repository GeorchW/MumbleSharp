﻿using System;

namespace MumbleSharp.Codecs
{
    public class OpusCodec
        : IVoiceCodec
    {
        public byte[] Decode(byte[] encodedData)
        {
            throw new NotImplementedException();
        }

        public byte[] Encode(byte[] pcm)
        {
            throw new NotImplementedException();
        }
    }
}
