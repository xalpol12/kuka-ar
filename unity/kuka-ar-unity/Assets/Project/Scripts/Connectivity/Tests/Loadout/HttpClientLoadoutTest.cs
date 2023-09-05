using System.Collections;
using TMPro;
using UnityEngine;

namespace Project.Scripts.Connectivity.Tests.Loadout
{
    public class HttpClientLoadoutTest : MonoBehaviour
    {
        [SerializeField]
        private ulong samples;

        [SerializeField] private TMP_Text processed;

        private bool isBusy;

        private void Start()
        {
            isBusy = false;
        }

        private void Update()
        {
            if (isBusy) return;
            if (Input.GetKey(KeyCode.KeypadEnter))
            {
                processed.text = "Start all endpoints tests";
                isBusy = true;
                TestAll();
            } else if (Input.GetKey(KeyCode.Alpha1))
            {
                processed.text = "Start robots tests";
                isBusy = true;
                StartCoroutine(TestRobots());
            } else if (Input.GetKey(KeyCode.Alpha2))
            {
                processed.text = "Start configured tests";
                isBusy = true;
                StartCoroutine(TestConfiguredRobots());
            } else if (Input.GetKey(KeyCode.Alpha3))
            {
                processed.text = "Start stickers tests";
                isBusy = true;
                StartCoroutine(TestStickers());
            }
        }

        private void TestAll()
        {
            StartCoroutine(TestRobots());
            StartCoroutine(TestConfiguredRobots());
            StartCoroutine(TestStickers("Writing to all files ="));
        }

        private IEnumerator TestRobots()
        {
            for (ulong i = 0; i < samples + 1; i++)
            {
                processed.text = "Current robot sample = " + i;
                StartCoroutine(FakeServerInvoker.Invoker.GetRobots(i));
                yield return null;
            }
            
            isBusy = false;
        }
        
        private IEnumerator TestConfiguredRobots()
        {
            for (ulong i = 0; i < samples + 1; i++)
            {
                processed.text = "Current configured robot sample = " + i;
                StartCoroutine(FakeServerInvoker.Invoker.GetConfiguredRobots(i));
                yield return null;
            }
            isBusy = false;
        }
        
        private IEnumerator TestStickers(string message = "Current sticker sample =")
        {
            for (ulong i = 0; i < samples + 1; i++)
            {
                processed.text = message + i;
                StartCoroutine(FakeServerInvoker.Invoker.GetStickers(i));
                yield return null;
            }
            isBusy = false;
        }
    }
}
