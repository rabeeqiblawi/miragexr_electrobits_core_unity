using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace MirageXR.ElectroBits
{
    public enum ClientMethod
    {
        RECEIVE_BROADCAST = 0,
        SET_PIN_COUNT = 1,
        SET_MAX_PWM = 2,
    }

    public enum ServerMethod
    {
        SetDigitalPin = 0,
        GetPinsCount = 1,
        SetTextPin = 2,
    }

    [System.Serializable]
    public class MessageBase
    {
        public ClientMethod method;
        public object data;
        public new string ToString() { return $"method : {method}"; }
    }

    [System.Serializable]
    public class MessageTypeInt : MessageBase
    {
        public new int data;
        public new string ToString()
        {
            return base.ToString() + $"data :{data}";
        }
    }

    [System.Serializable]
    public class MessageTypeString : MessageBase
    {
        public new string data;
        public new string ToString()
        {
            return base.ToString() + $"data :{data}";
        }
    }

    public class MessageFactory : MonoBehaviour
    {
        public static MessageBase DeserializeMessage(string json)
        {
            var baseMessage = JsonConvert.DeserializeObject<MessageBase>(json);
            MessageBase output = baseMessage.method switch
            {
                ClientMethod.RECEIVE_BROADCAST => (JsonConvert.DeserializeObject<MessageTypeString>(json)),
                ClientMethod.SET_PIN_COUNT => JsonConvert.DeserializeObject<MessageTypeInt>(json),
                ClientMethod.SET_MAX_PWM => JsonConvert.DeserializeObject<MessageTypeInt>(json),
                _ => throw new System.NotImplementedException(),
            };
            return output;
        }
    }
}
