using MineSharp.Core.Common;
using MineSharp.Core.Common.Effects;
using MineSharp.Core.Common.Entities;
using MineSharp.Protocol.Packets.Clientbound.Play;
using System.Collections.Concurrent;

namespace MineSharp.Bot.Plugins;

/// <summary>
/// This Plugin tracks entities.
/// </summary>
public class EntityPlugin : Plugin
{
    private const double VELOCITY_CONVERSION = 8000d;
    
    /// <summary>
    /// All Entities loaded by the client.
    /// </summary>
    public readonly IDictionary<int, Entity> Entities;

    public event Events.EntityEvent? OnEntitySpawned;
    public event Events.EntityEvent? OnEntityDespawned;
    public event Events.EntityEvent? OnEntityMoved;

    public EntityPlugin(MinecraftBot bot) : base(bot)
    {
        this.Entities = new ConcurrentDictionary<int, Entity>();
        
        this.Bot.Client.On<SpawnEntityPacket>(this.HandleSpawnEntityPacket);
        this.Bot.Client.On<RemoveEntitiesPacket>(this.HandleRemoveEntitiesPacket);
        this.Bot.Client.On<SetEntityVelocityPacket>(this.HandleSetEntityVelocityPacket);
        this.Bot.Client.On<UpdateEntityPositionPacket>(this.HandleUpdateEntityPositionPacket);
        this.Bot.Client.On<UpdateEntityPositionAndRotationPacket>(this.HandleUpdateEntityPositionAndRotationPacket);
        this.Bot.Client.On<UpdateEntityRotationPacket>(this.HandleUpdateEntityRotationPacket);
        this.Bot.Client.On<TeleportEntityPacket>(this.HandleTeleportEntityPacket);
        this.Bot.Client.On<UpdateAttributesPacket>(this.HandleUpdateAttributesPacket);
    }

    private Task HandleSpawnEntityPacket(SpawnEntityPacket packet)
    {
        var entityInfo = this.Bot.Data.Entities.GetById(packet.Type);
        
        var newEntity = new Entity(
            entityInfo, packet.EntityId!, new Vector3(packet.X, packet.Y, packet.Z),
            packet.Pitch,
            packet.Yaw,
            new Vector3(ConvertToVelocity(packet.VelocityX), ConvertToVelocity(packet.VelocityY), ConvertToVelocity(packet.VelocityZ)),
            true,
            new Dictionary<int, Effect?>());

        this.Entities.TryAdd(packet.EntityId, newEntity);
        this.OnEntitySpawned?.Invoke(this.Bot, newEntity);
        
        return Task.CompletedTask;
    }

    private Task HandleRemoveEntitiesPacket(RemoveEntitiesPacket packet)
    {
        foreach (var entityId in packet.EntityIds)
        {
            if (!this.Entities.Remove(entityId, out var entity))
                continue;
            
            this.OnEntityDespawned?.Invoke(this.Bot, entity);
        }

        return Task.CompletedTask;
    }

    private Task HandleSetEntityVelocityPacket(SetEntityVelocityPacket packet)
    {
        if (!this.Entities.TryGetValue(packet.EntityId, out var entity))
        {
            return Task.CompletedTask;
        }

        entity.Velocity = new Vector3(
            ConvertToVelocity(packet.VelocityX),
            ConvertToVelocity(packet.VelocityY),
            ConvertToVelocity(packet.VelocityZ));
        
        return Task.CompletedTask;
    }

    private Task HandleUpdateEntityPositionPacket(UpdateEntityPositionPacket packet)
    {
        if (!this.Entities.TryGetValue(packet.EntityId, out var entity))
            return Task.CompletedTask;

        entity.Position.Add(new Vector3(
            ConvertDeltaPosition(packet.DeltaX),
            ConvertDeltaPosition(packet.DeltaY),
            ConvertDeltaPosition(packet.DeltaZ)));
        
        entity.IsOnGround = packet.OnGround;

        this.OnEntityMoved?.Invoke(this.Bot, entity);
        return Task.CompletedTask;
    }

    private Task HandleUpdateEntityPositionAndRotationPacket(UpdateEntityPositionAndRotationPacket packet)
    {
        if (!this.Entities.TryGetValue(packet.EntityId, out var entity))
            return Task.CompletedTask;

        entity.Position.Add(new Vector3(
            ConvertDeltaPosition(packet.DeltaX),
            ConvertDeltaPosition(packet.DeltaY),
            ConvertDeltaPosition(packet.DeltaZ)));

        entity.Yaw = entity.Yaw;
        entity.Pitch = entity.Pitch;
        entity.IsOnGround = packet.OnGround;

        this.OnEntityMoved?.Invoke(this.Bot, entity);
        return Task.CompletedTask;
    }

    private Task HandleUpdateEntityRotationPacket(UpdateEntityRotationPacket packet)
    {
        if (!this.Entities.TryGetValue(packet.EntityId!, out var entity))
            return Task.CompletedTask;

        entity.Yaw = packet.Yaw;
        entity.Pitch = packet.Pitch;
        entity.IsOnGround = packet.OnGround;

        this.OnEntityMoved?.Invoke(this.Bot, entity);

        return Task.CompletedTask;
    }

    private Task HandleTeleportEntityPacket(TeleportEntityPacket packet)
    {
        if (!this.Entities.TryGetValue(packet.EntityId!, out var entity))
            return Task.CompletedTask;

        entity.Position.X = packet.X;
        entity.Position.Y = packet.Y;
        entity.Position.Z = packet.Z;

        entity.Yaw = packet.Yaw;
        entity.Pitch = packet.Pitch;
        this.OnEntityMoved?.Invoke(this.Bot, entity);

        return Task.CompletedTask;
    }

    private Task HandleUpdateAttributesPacket(UpdateAttributesPacket packet)
    {
        if (!this.Entities.TryGetValue(packet.EntityId, out var entity))
            return Task.CompletedTask;

        foreach (var attribute in packet.Attributes)
        {
            if (!entity.Attributes.TryAdd(attribute.Key, attribute))
            {
                entity.Attributes[attribute.Key] = attribute;
            } 
        }

        return Task.CompletedTask;
    }

    internal void AddEntity(Entity entity)
    {
        this.Entities.TryAdd(entity.ServerId, entity);
    }

    private double ConvertToVelocity(double value)
        => value / VELOCITY_CONVERSION;

    private double ConvertDeltaPosition(short delta)
        => delta / (128 * 32d);
}
