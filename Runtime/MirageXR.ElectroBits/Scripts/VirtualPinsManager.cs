using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MirageXR.ElectroBits
{
    public enum ValueType
    {
        Digital,
        Analog,
        String
    }

    [System.Serializable]
    public class PinData
    {
        public static int INPUT = 1;
        public static int OUTPUT = 3;

        public enum IOMode
        {
            input = 1,
            output = 3
        };

        [JsonProperty("pinIndex")]
        public int pinIndex;
        [JsonProperty("ioMode")]
        public IOMode ioMode;
        [JsonProperty("digitalValue")]
        public int digitalValue;
        [JsonProperty("analogValue")]
        public int analogValue;
        [JsonProperty("activeValueType")]
        public ValueType activeValueType;
        [JsonProperty("textValue")]
        public string textValue;
    }

    public class VirtualPinsManager : MirageXRBase
    {
        public List<PinData> Pins;
        [SerializeField] int _maxPwm;
        public new MirageNetworkManager MirageNetworkManager;

        void Start()
        {
            MirageNetworkManager.OnRecive.AddListener(HandleIncomingData);
        }

        public void HandleIncomingData(MessageBase msg)
        {
            switch (msg.method)
            {
                case ClientMethod.SET_PIN_COUNT:
                    int pinCount = ((MessageTypeInt)msg).data;
                    Pins = new List<PinData>(pinCount);
                    for (int i = 0; i < pinCount; i++)
                    {
                        Pins.Add(new PinData
                        {
                            pinIndex = i,
                            digitalValue = 0,
                            analogValue = 0,
                            ioMode = PinData.IOMode.input,
                            activeValueType = ValueType.Digital
                        });
                    }
                    break;
                case ClientMethod.RECEIVE_BROADCAST:
                    var decerializedData = JsonConvert.DeserializeObject<List<PinData>>(((MessageTypeString)msg).data);
                    for (int i = 0; i < Pins.Count; i++)
                    {
                        Pins[i].analogValue = decerializedData[i].analogValue;
                        Pins[i].digitalValue = decerializedData[i].digitalValue;
                        Pins[i].textValue = decerializedData[i].textValue;
                        Pins[i].ioMode = decerializedData[i].ioMode;
                        Pins[i].pinIndex = decerializedData[i].pinIndex;
                        Pins[i].activeValueType = decerializedData[i].activeValueType;
                    }
                    break;
                case ClientMethod.SET_MAX_PWM:
                    int maxPwm = ((MessageTypeInt)msg).data;
                    _maxPwm = maxPwm;
                    break;
            }
        }

        public void SetDigitalPin(int index, int value)
        {
            var isInput = Pins[index].ioMode == PinData.IOMode.input;
            if (isInput)
            {
                Debug.Log("Pin needs to be output to set it's digitalValue");
                return;
            }
            MirageNetworkManager.SendServerRequest(ServerMethod.SetDigitalPin, args: new { pin = index, value });
        }

        public void SetTextPin(int index, string value)
        {
            var isInput = Pins[index].ioMode == PinData.IOMode.input;
            if (isInput)
            {
                Debug.Log("Pin needs to be output to set it's textValue");
                return;
            }
            MirageNetworkManager.SendServerRequest(ServerMethod.SetTextPin, args: new { pin = index, value });
        }

        public PinData ReadPin(int index)
        {
            return Pins[index];
        }
    }
}
