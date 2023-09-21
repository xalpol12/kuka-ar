using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.Scripts.Multithreading
{
	public class UnityMainThreadDispatcher : MonoBehaviour {

		public static UnityMainThreadDispatcher Instance;
		
		private static readonly Queue<Action> ExecutionQueue = new();
		
		private void Awake()
		{
			Instance = this;
		}

		public void Update() 
		{
			lock(ExecutionQueue) 
			{
				while (ExecutionQueue.Count > 0) 
				{
					ExecutionQueue.Dequeue().Invoke();
				}
			}
		}

		/// <summary>
		/// Locks the queue and adds the Action to the queue
		/// </summary>
		/// <param name="action">function that will be executed from the main thread.</param>
		public void Enqueue(Action action)
		{
			Enqueue(ActionWrapper(action));
		}

		private void Enqueue(IEnumerator action) 
		{
			lock (ExecutionQueue) 
			{
				ExecutionQueue.Enqueue (() => 
				{
					StartCoroutine (action);
				});
			}
		}

		/// <summary>
		/// Locks the queue and adds the Action to the queue, returning a Task which is completed when the action completes
		/// </summary>
		/// <param name="action">function that will be executed from the main thread.</param>
		/// <returns>A Task that can be awaited until the action completes</returns>
		public Task EnqueueAsync(Action action)
		{
			var tcs = new TaskCompletionSource<bool>();

			Enqueue(ActionWrapper(WrappedAction));
			return tcs.Task;

			void WrappedAction() 
			{
				try 
				{
					action();
					tcs.TrySetResult(true);
				} catch (Exception ex) 
				{
					tcs.TrySetException(ex);
				}
			}
		}

		private static IEnumerator ActionWrapper(Action a)
		{
			a();
			yield return null;
		}
		
	}
}