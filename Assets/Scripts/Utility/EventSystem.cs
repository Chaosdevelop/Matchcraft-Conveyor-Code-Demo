using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BaseCore
{
	/// <summary>
	/// Event system for transmitting events to subscribers.
	/// </summary>
	public static class EventSystem
	{
		static Dictionary<object, List<IEventSubscriber>> subscribersDict = new Dictionary<object, List<IEventSubscriber>>();

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		public static void Clear()
		{
			subscribersDict.Clear();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void SubscribeOnStart()
		{
			foreach (var initializable in GameObject.FindObjectsOfType<MonoBehaviour>(true).OfType<IStartUpMonoBehaviourSubscriber>())
			{
				if (initializable is MonoBehaviour monoBehaviour)
				{
					initializable.SelfSubscribe();
				}
			}
		}

		/// <summary>
		/// Sends an event from the owner object to its subscribers.
		/// </summary>
		/// <typeparam name="T">The type of the event data.</typeparam>
		/// <param name="fromOwner">The owner object sending the event.</param>
		/// <param name="data">The event data.</param>
		public static void SendEventToSubscribers<T>(object fromOwner, T data) where T : struct
		{
			if (subscribersDict.TryGetValue(fromOwner, out List<IEventSubscriber> ownerSubscribersList))
			{
				SendToSubscribers(ownerSubscribersList, data);
			}
		}

		/// <summary>
		/// Sends a global event to all subscribers regardless of the owner object.
		/// </summary>
		/// <typeparam name="T">The type of the event data.</typeparam>
		/// <param name="data">The event data.</param>
		public static void SendEventToAll<T>(T data) where T : struct
		{
			foreach (var ownerSubscribers in subscribersDict.Values.ToArray())
			{
				SendToSubscribers(ownerSubscribers, data);
			}
		}

		/// <summary>
		/// Sends an event to a list of subscribers.
		/// </summary>
		/// <typeparam name="T">The type of the event data.</typeparam>
		/// <param name="subscribersList">The list of subscribers.</param>
		/// <param name="data">The event data.</param>
		static void SendToSubscribers<T>(List<IEventSubscriber> subscribersList, T data) where T : struct
		{
			var matchedSubscribers = subscribersList.OfType<IEventSubscriber<T>>().ToArray();
			for (int i = 0; i < matchedSubscribers.Length; i++)
			{
				matchedSubscribers[i].ReceiveEvent(data);
			}
		}

		/// <summary>
		/// Subscribes a subscriber to the model's events.
		/// </summary>
		/// <param name="model">The model to subscribe to.</param>
		/// <param name="subscriber">The subscriber.</param>
		public static void Subscribe(object model, IEventSubscriber subscriber)
		{
			if (!subscribersDict.ContainsKey(model))
			{
				subscribersDict[model] = new List<IEventSubscriber>();
			}

			var subscribersList = subscribersDict[model];
			if (!subscribersList.Contains(subscriber))
			{
				subscribersList.Add(subscriber);
				(subscriber as ISubscriberInitializer)?.OnSubscribe(model);
			}
		}

		/// <summary>
		/// Subscribes multiple subscribers to the model's events.
		/// </summary>
		/// <param name="model">The model to subscribe to.</param>
		/// <param name="subscribers">The list of subscribers.</param>
		public static void Subscribe(object model, List<IEventSubscriber> subscribers)
		{
			if (!subscribersDict.ContainsKey(model))
			{
				subscribersDict[model] = new List<IEventSubscriber>();
			}

			var subscribersList = subscribersDict[model];
			foreach (var subscriber in subscribers)
			{
				if (!subscribersList.Contains(subscriber))
				{
					subscribersList.Add(subscriber);
					(subscriber as ISubscriberInitializer)?.OnSubscribe(model);
				}
			}
		}

		/// <summary>
		/// Unsubscribes all subscribers from the model's events.
		/// </summary>
		/// <param name="model">The model to unsubscribe from.</param>
		public static void UnsubscribeAll(object model)
		{
			if (subscribersDict.ContainsKey(model))
			{
				subscribersDict.Remove(model);
			}
		}

		/// <summary>
		/// Unsubscribes a specific subscriber from the model's events.
		/// </summary>
		/// <param name="model">The model to unsubscribe from.</param>
		/// <param name="subscriber">The subscriber.</param>
		public static void Unsubscribe(object model, IEventSubscriber subscriber)
		{
			if (subscribersDict.TryGetValue(model, out var subscribersList))
			{
				subscribersList.Remove(subscriber);
			}
		}

		public static void SelfSubscribe(this IEventSubscriber subscriber)
		{
			Subscribe(subscriber, new List<IEventSubscriber> { subscriber });
		}

		public static void SelfUnsubscribe(this IEventSubscriber subscriber)
		{
			UnsubscribeAll(subscriber);
		}
	}

	/// <summary>
	/// Interface for event subscribers.
	/// </summary>
	public interface IEventSubscriber
	{
	}

	/// <summary>
	/// Interface for initializing subscribers when they are subscribed.
	/// </summary>
	public interface ISubscriberInitializer : IEventSubscriber
	{
		/// <summary>
		/// Called when the subscriber is subscribed to a model.
		/// </summary>
		/// <param name="owner">The model the subscriber is subscribed to.</param>
		void OnSubscribe(object owner);
	}

	/// <summary>
	/// Interface for event subscribers that handle specific types of events.
	/// </summary>
	/// <typeparam name="T">The type of event data the subscriber handles.</typeparam>
	public interface IEventSubscriber<T> : IEventSubscriber where T : struct
	{
		/// <summary>
		/// Receives an event of type T.
		/// </summary>
		/// <param name="data">The event data.</param>
		void ReceiveEvent(T data);
	}

	/// <summary>
	/// Interface for MonoBehaviour subscribers that subscribe on start.
	/// </summary>
	public interface IStartUpMonoBehaviourSubscriber : IEventSubscriber
	{
	}
}
