using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class WindowSerialPortWrapper : SerialPortLayer
{

    public override void Close()
    {
        throw new System.NotImplementedException();
    }

    public override void Dispose()
    {
        throw new System.NotImplementedException();
    }

    public override SerialPortGateEnums.BaudRate GetBaudRate()
    {
        throw new System.NotImplementedException();
    }

    public override SerialPortGateEnums.DataBits GetDataBits()
    {
        throw new System.NotImplementedException();
    }

    public override SerialPortGateEnums.Parity GetParity()
    {
        throw new System.NotImplementedException();
    }

    public override SerialPortGateEnums.SerialPortName GetPortName()
    {
        throw new System.NotImplementedException();
    }

    public override SerialPortGateEnums.StopBits GetStopBits()
    {
        throw new System.NotImplementedException();
    }

    public override bool IsOpen()
    {
        throw new System.NotImplementedException();
    }

    public override void Open()
    {
        throw new System.NotImplementedException();
    }

    public override string ReadLine()
    {
        throw new System.NotImplementedException();
    }

    public override void SetParams(SerialPortGateEnums.SerialPortName portName, SerialPortGateEnums.BaudRate baudRate, SerialPortGateEnums.Parity parity, SerialPortGateEnums.DataBits dataBits, SerialPortGateEnums.StopBits stopBits)
    {
        throw new System.NotImplementedException();
    }

    public override void WriteLine(string text)
    {
        throw new System.NotImplementedException();
    }
}