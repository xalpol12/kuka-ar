using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.Scripts.Multithreading
{
	public class UnityMainThreadDispatcher : MonoBehaviour {

		private static readonly Queue<Action> ExecutionQueue = new();

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
			Enqueue(ActionWrapper(WrappedAction));
			return tcs.Task;
		}

		IEnumerator ActionWrapper(Action a)
		{
			a();
			yield return null;
		}
		
		#region Singleton logic

		private static UnityMainThreadDispatcher instance = null;

		private static bool Exists() 
		{
			return instance != null;
		}

		public static UnityMainThreadDispatcher Instance() 
		{
			if (!Exists ()) 
			{
				throw new Exception (
					"UnityMainThreadDispatcher could not find the UnityMainThreadDispatcher object. " +
					"Please ensure you have added the MainThreadExecutor Prefab to your scene.");
			}
			return instance;
		}


		void Awake()
		{
			if (instance == null) 
			{
				instance = this;
				DontDestroyOnLoad(gameObject);
			}
		}

		void OnDestroy() 
		{
			instance = null;
		}

		#endregion
	}
}