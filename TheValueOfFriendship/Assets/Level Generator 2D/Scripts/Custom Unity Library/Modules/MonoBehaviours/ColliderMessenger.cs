namespace CustomUnityLibrary
{
    using System.Text.RegularExpressions;
    using UnityEngine;

    /// <summary>
    /// Attached to children, it will notify parents and ancestors of collisions by calling a method of the GameObject's name
    /// </summary>
    [DisallowMultipleComponent]
    public class ColliderMessenger : MonoBehaviour
    {
        private const string MethodNamePrefix = "On";
        private const string EnterMethodNameSuffix = "Enter";
        private const string ExitMethodNameSuffix = "Exit";
        private const string StayMethodNameSuffix = "Stay";
        private const string WhitespaceRegex = @"\s+";

        void OnCollisionEnter(Collision collision)
        {
            string methodName = MethodNamePrefix + Regex.Replace(name, WhitespaceRegex, "") + EnterMethodNameSuffix;
            SendMessageUpwards(methodName, collision, SendMessageOptions.DontRequireReceiver);
        }

        void OnCollisionExit(Collision collisionInfo)
        {
            string methodName = MethodNamePrefix + Regex.Replace(name, WhitespaceRegex, "") + ExitMethodNameSuffix;
            SendMessageUpwards(methodName, collisionInfo, SendMessageOptions.DontRequireReceiver);
        }

        void OnCollisionStay(Collision collisionInfo)
        {
            string methodName = MethodNamePrefix + Regex.Replace(name, WhitespaceRegex, "") + StayMethodNameSuffix;
            SendMessageUpwards(methodName, collisionInfo);
        }
        void OnTriggerEnter(Collider other)
        {
            string methodName = MethodNamePrefix + Regex.Replace(name, WhitespaceRegex, "") + EnterMethodNameSuffix;
            SendMessageUpwards(methodName, other, SendMessageOptions.DontRequireReceiver);
        }
        void OnTriggerExit(Collider other)
        {
            string methodName = MethodNamePrefix + Regex.Replace(name, WhitespaceRegex, "") + ExitMethodNameSuffix;
            SendMessageUpwards(methodName, other, SendMessageOptions.DontRequireReceiver);
        }

        void OnTriggerStay(Collider other)
        {
            string methodName = MethodNamePrefix + Regex.Replace(name, WhitespaceRegex, "") + StayMethodNameSuffix;
            SendMessageUpwards(methodName, other, SendMessageOptions.DontRequireReceiver);
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            string methodName = MethodNamePrefix + Regex.Replace(name, WhitespaceRegex, "") + EnterMethodNameSuffix;
            SendMessageUpwards(methodName, coll, SendMessageOptions.DontRequireReceiver);
        }

        void OnCollisionExit2D(Collision2D coll)
        {
            string methodName = MethodNamePrefix + Regex.Replace(name, WhitespaceRegex, "") + ExitMethodNameSuffix;
            SendMessageUpwards(methodName, coll, SendMessageOptions.DontRequireReceiver);
        }

        void OnCollisionStay2D(Collision2D coll)
        {
            string methodName = MethodNamePrefix + Regex.Replace(name, WhitespaceRegex, "") + StayMethodNameSuffix;
            SendMessageUpwards(methodName, coll, SendMessageOptions.DontRequireReceiver);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            string methodName = MethodNamePrefix + Regex.Replace(name, WhitespaceRegex, "") + EnterMethodNameSuffix;
            SendMessageUpwards(methodName, other, SendMessageOptions.DontRequireReceiver);
        }

        void OnTriggerExit2D(Collider2D other)
        {
            string methodName = MethodNamePrefix + Regex.Replace(name, WhitespaceRegex, "") + ExitMethodNameSuffix;
            SendMessageUpwards(methodName, other, SendMessageOptions.DontRequireReceiver);
        }

        void OnTriggerStay2D(Collider2D other)
        {
            string methodName = MethodNamePrefix + Regex.Replace(name, WhitespaceRegex, "") + StayMethodNameSuffix;
            SendMessageUpwards(methodName, other, SendMessageOptions.DontRequireReceiver);
        }
    }
}