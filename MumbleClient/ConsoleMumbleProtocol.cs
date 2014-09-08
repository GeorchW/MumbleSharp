using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using MumbleSharp;
using MumbleSharp.Model;

namespace MumbleClient
{
    /// <summary>
    /// A test mumble protocol.
    /// Currently just prints out text messages in the root channel. Voice data isn't decoded!
    /// 
    /// TBH this could be removed entirely and only use the event, but whatever, I'll leave it for demonstration purposes.
    /// </summary>
    public class ConsoleMumbleProtocol : BasicMumbleProtocol
    {
        public override void TextMessage(MumbleSharp.Packets.TextMessage textMessage)
        {
            User user;
            if (!UserDictionary.TryGetValue(textMessage.Actor, out user))   //If we don't know the user for this packet, just ignore it
                return;

            if (textMessage.ChannelId == null)
            {
                if (textMessage.TreeId == null)
                {
                    //personal message: no channel, no tree
                    for (int i = 0; i < textMessage.Message.Length; i++)
                        Console.WriteLine(user.Name + " (only for you): " + textMessage.Message[i]);
                }
                else
                {
                    //recursive message: sent to multiple channels
                    for (int i = 0; i < textMessage.Message.Length; i++)
                        Console.WriteLine(user.Name + " (for tree " + textMessage.TreeId[0] + "): " + textMessage.Message[i]);
                }
                return;
            }
            Channel c;
            if (!ChannelDictionary.TryGetValue(textMessage.ChannelId[0], out c))    //If we don't know the channel for this packet, just ignore it
                return;

            for (int i = 0; i < textMessage.Message.Length; i++)
                Console.WriteLine(user.Name + " (" + c.Name + "): " + textMessage.Message[i]);
        }
    }
}
