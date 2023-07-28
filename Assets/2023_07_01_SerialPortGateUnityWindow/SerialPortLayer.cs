using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

[System.Serializable]
public abstract class SerialPortLayer : ISerialPortParamsContainer, ISerialPortMinimumAction
{

    public static string[] GetPortNames() { return SerialPortAbstractLayerStatic.GetSerialPorts().ToArray(); }

    public abstract void Close();
    public abstract void Dispose();
    public abstract SerialPortGateEnums.BaudRate GetBaudRate();
    public abstract SerialPortGateEnums.DataBits GetDataBits();
    public abstract SerialPortGateEnums.Parity GetParity();
    public abstract SerialPortGateEnums.SerialPortName GetPortName();
    public abstract SerialPortGateEnums.StopBits GetStopBits();
    public abstract bool IsOpen();
    public abstract void Open();
    public abstract string ReadLine();
    public abstract void SetParams(SerialPortGateEnums.SerialPortName portName, SerialPortGateEnums.BaudRate baudRate, SerialPortGateEnums.Parity parity, SerialPortGateEnums.DataBits dataBits, SerialPortGateEnums.StopBits stopBits);
    public abstract void WriteLine(string text);
}

public interface ISerialPortParamsContainer {
    public abstract SerialPortGateEnums.SerialPortName GetPortName();
    public abstract SerialPortGateEnums.BaudRate GetBaudRate();
    public abstract SerialPortGateEnums.DataBits GetDataBits();
    public abstract SerialPortGateEnums.Parity GetParity();
    public abstract SerialPortGateEnums.StopBits GetStopBits();
    public void SetParams( SerialPortGateEnums.SerialPortName portName,
     SerialPortGateEnums.BaudRate baudRate,
     SerialPortGateEnums.Parity parity,
     SerialPortGateEnums.DataBits dataBits,
     SerialPortGateEnums.StopBits stopBits);
}

public interface ISerialPortMinimumAction
{
    public  bool IsOpen();

    public  string ReadLine();

    public  void WriteLine(string text);
    public  void Open();

    public  void Close();

    public  void Dispose();

}


public class SerialPortParamsContainer : ISerialPortParamsContainer
{

    public SerialPortGateEnums.SerialPortName m_portName;
    public SerialPortGateEnums.BaudRate m_baudRate;
    public SerialPortGateEnums.Parity m_parity;
    public SerialPortGateEnums.DataBits m_dataBits;
    public SerialPortGateEnums.StopBits m_stopBits;

    public SerialPortGateEnums.BaudRate GetBaudRate()
    {
        return m_baudRate;
    }

    public SerialPortGateEnums.DataBits GetDataBits()
    {
        return m_dataBits;
    }

    public SerialPortGateEnums.Parity GetParity()
    {
        return m_parity;
    }

    public SerialPortGateEnums.SerialPortName GetPortName()
    {
        return m_portName;
    }

    public SerialPortGateEnums.StopBits GetStopBits()
    {
        return m_stopBits;
    }

    public void SetParams(SerialPortGateEnums.SerialPortName portName, SerialPortGateEnums.BaudRate baudRate, SerialPortGateEnums.Parity parity, SerialPortGateEnums.DataBits dataBits, SerialPortGateEnums.StopBits stopBits)
    {
        m_portName = portName;
        m_baudRate = baudRate;
        m_parity = parity;
        m_stopBits = stopBits;
        m_dataBits = dataBits;
    }
}

[System.Serializable]
public class NotImplementedSerialPortWrapper : SerialPortLayer
{
    public override void Close()
    {
        throw new NotImplementedException();
    }

    public override void Dispose()
    {
        throw new NotImplementedException();
    }

    public override SerialPortGateEnums.BaudRate GetBaudRate()
    {
        throw new NotImplementedException();
    }

    public override SerialPortGateEnums.DataBits GetDataBits()
    {
        throw new NotImplementedException();
    }

    public override SerialPortGateEnums.Parity GetParity()
    {
        throw new NotImplementedException();
    }

    public override SerialPortGateEnums.SerialPortName GetPortName()
    {
        throw new NotImplementedException();
    }

    public override SerialPortGateEnums.StopBits GetStopBits()
    {
        throw new NotImplementedException();
    }

    public override bool IsOpen()
    {
        throw new NotImplementedException();
    }

    public override void Open()
    {
        throw new NotImplementedException();
    }

    public override string ReadLine()
    {
        throw new NotImplementedException();
    }

    public override void SetParams(SerialPortGateEnums.SerialPortName portName, SerialPortGateEnums.BaudRate baudRate, SerialPortGateEnums.Parity parity, SerialPortGateEnums.DataBits dataBits, SerialPortGateEnums.StopBits stopBits)
    {
        throw new NotImplementedException();
    }

    public override void WriteLine(string text)
    {
        throw new NotImplementedException();
    }
}
