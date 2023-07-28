
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_STANDALONE_WIN
using System.IO.Ports;
#endif

public class SerialPortUtility2 
{

    public string[] GetSerialPortNames() {

#if UNITY_STANDALONE_WIN
        return SerialPort.GetPortNames();
#else
        return new string[0];
#endif
    }

    public uint[] GetSerialPortId() {

        List<uint> ports=  new List<uint>();
        foreach (var item in GetSerialPortNames())
        {
            uint value;
            string idAsString = item.ToUpper().Replace("COM", "");
            if (uint.TryParse(idAsString, out value)){
                ports.Add(value);
            }

        }
        return ports.ToArray() ;
    }
}