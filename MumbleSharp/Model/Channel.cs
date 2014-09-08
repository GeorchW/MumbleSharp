﻿
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using MumbleSharp.Packets;

namespace MumbleSharp.Model
{
    public class Channel
        : IEquatable<Channel>
    {
        internal readonly IMumbleProtocol Owner;

        public bool Temporary { get; set; }
        public string Name { get; set; }
        public uint Id { get; private set; }
        public uint Parent { get; private set; }

        // Using a concurrent dictionary as a concurrent hashset (why doesn't .net provide a concurrent hashset?!) - http://stackoverflow.com/a/18923091/108234
        private readonly ConcurrentDictionary<User, bool> _users = new ConcurrentDictionary<User, bool>();
        public IEnumerable<User> Users
        {
            get { return _users.Keys; }
        }

        public Channel(IMumbleProtocol owner, uint id, string name, uint parent)
        {
            Owner = owner;
            Id = id;
            Name = name;
            Parent = parent;
        }

        public void SendMessage(string[] message, bool recursive)
        {
            var msg = new TextMessage
            {
                Actor = Owner.LocalUser.Id,
                Message = message,
            };

            if (recursive)
                msg.TreeId = new uint[Id];
            else
                msg.ChannelId = new uint[Id];

            Owner.Connection.SendControl<TextMessage>(PacketType.TextMessage, msg);
        }

        public override string ToString()
        {
            return Name;
        }

        internal void RemoveUser(User user)
        {
            _users.GetOrAdd(user, true);
        }

        internal void AddUser(User user)
        {
            bool _;
            _users.TryRemove(user, out _);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var c = obj as Channel;
            if (c != null)
                return Equals(c);

            return ReferenceEquals(this, obj);
        }

        public bool Equals(Channel other)
        {
            return other.Id == Id;
        }
    }
}
