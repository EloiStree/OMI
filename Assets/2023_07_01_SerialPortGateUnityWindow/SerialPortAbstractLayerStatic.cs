using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using UnityEngine;

public class SerialPortAbstractLayerStatic
{

    public static string[] m_currentPort = new string[0];
    /// <summary>
    /// Dont forget to put somewhere in you scene a code that refresh the lsit of port available.
    /// </summary>
    /// <param name="ports"></param>
    public static void SetCurrentPortAvailable(IEnumerable<string> ports)
    {
        m_currentPort = ports.ToArray();
    }
    public static void GetSerialPorts(out IEnumerable<string> serialPort) { serialPort = m_currentPort; }
    public static IEnumerable<string> GetSerialPorts() { return m_currentPort; }

    internal static SerialPortLayer CreateFromSpecificPlatform(
        string portName,
        int baudRate,
        SerialPortGateEnums.Parity parity,
        int dataBits, 
        SerialPortGateEnums.StopBits stopBits)
    {
        return CreateFromSpecificPlatform(
            new SerialPortGateEnums.SerialPortName(portName),
            (SerialPortGateEnums.BaudRate)baudRate,
            parity, 
            (SerialPortGateEnums.DataBits) dataBits,
            stopBits); 
            
    }


    public static SerialPortLayer CreateFromSpecificPlatform(
        string portName,
        string baudRate,
        string parity, 
        string dataBits,
        string stopBits)
    {
        return CreateFromSpecificPlatform(
            SerialPortGateEnums.CreatePortName(portName),
            SerialPortGateEnums.ParseOrDefault(baudRate, SerialPortGateEnums.BaudRate._9600),
            SerialPortGateEnums.ParseOrDefault(parity, SerialPortGateEnums.Parity.None),
            SerialPortGateEnums.ParseOrDefault(dataBits, SerialPortGateEnums.DataBits._8),
            SerialPortGateEnums.ParseOrDefault(stopBits, SerialPortGateEnums.StopBits.None)
            );
    }

    public static SerialPortLayer CreateFromSpecificPlatform(SerialPortGateEnums.SerialPortName portName,
        SerialPortGateEnums.BaudRate baudRate, SerialPortGateEnums.Parity parity,
        SerialPortGateEnums.DataBits dataBits, SerialPortGateEnums.StopBits stopBits)
    {
#if UNITY_STANDALONE

        return new WindowSerialPort(portName, baudRate,parity,dataBits,stopBits);

#elif UNITY_ANDROID
        return null;
#else
        return null;
#endif

    }
}

#if UNITY_STANDALONE
[System.Serializable]
public class WindowSerialPort : SerialPortLayer
{
    public SerialPort m_serialPort;
   

    public WindowSerialPort(SerialPortGateEnums.SerialPortName portName, SerialPortGateEnums.BaudRate baudRate, SerialPortGateEnums.Parity parity, SerialPortGateEnums.DataBits dataBits, SerialPortGateEnums.StopBits stopBits)
    {
        SetParams(portName, 
            baudRate, 
            parity,
            dataBits, 
            stopBits);
    }

    public override void Close()
    {
        if (m_serialPort != null) {
            m_serialPort.Close();
            m_serialPort.Dispose();
        }
    }

    public override void Dispose()
    {
        if (m_serialPort != null)
        {
            m_serialPort.Close();
            m_serialPort.Dispose();
        }
    }

    public override SerialPortGateEnums.BaudRate GetBaudRate()
    {
        SerialPortGateEnums.TryParse(""+m_serialPort.BaudRate, out bool convert, out SerialPortGateEnums.BaudRate value);
        return value;
    }

    public override SerialPortGateEnums.DataBits GetDataBits()
    {
        SerialPortGateEnums.TryParse("" + m_serialPort.BaudRate, out bool convert, out SerialPortGateEnums.DataBits value);
        return value;
    }

    public override SerialPortGateEnums.Parity GetParity()
    {
        SerialPortGateEnums.TryParse("" + m_serialPort.BaudRate, out bool convert, out SerialPortGateEnums.Parity value);
        return value;
    }

    public override SerialPortGateEnums.SerialPortName GetPortName()
    {

        return new SerialPortGateEnums.SerialPortName(m_serialPort.PortName);
    }

    public override SerialPortGateEnums.StopBits GetStopBits()
    {
        SerialPortGateEnums.TryParse("" + m_serialPort.BaudRate, out bool convert, out SerialPortGateEnums.StopBits value);
        return value;
    }

    public override bool IsOpen()
    {
        return m_serialPort.IsOpen;
    }

    public override void Open()
    {
        m_serialPort.Open();
    }

    public override string ReadLine()
    {
        return m_serialPort.ReadLine();
    }

    public override void SetParams(SerialPortGateEnums.SerialPortName serialportname, SerialPortGateEnums.BaudRate baudRate, SerialPortGateEnums.Parity parity, SerialPortGateEnums.DataBits dataBits, SerialPortGateEnums.StopBits stopBits)
    {
        Dispose();
        Convert(parity, out Parity wParity);
        Convert(stopBits, out StopBits wStopBits);

        string wportName = serialportname.m_serialPort;
        int wBaudRate = (int) baudRate;
        int wdataBits= (int) dataBits;


        m_serialPort = new SerialPort(wportName, wBaudRate, wParity, wdataBits, wStopBits);

    }

    private void Convert(SerialPortGateEnums.StopBits stopBits, out StopBits wStopBits)
    {
        wStopBits = StopBits.None;
        switch (stopBits)
        {
            case SerialPortGateEnums.StopBits.None:
                wStopBits = StopBits.None;
                break;
            case SerialPortGateEnums.StopBits.One:
                wStopBits = StopBits.One;
                break;
            case SerialPortGateEnums.StopBits.OnePointFive:
                wStopBits = StopBits.OnePointFive;
                break;
            case SerialPortGateEnums.StopBits.Two:
                wStopBits = StopBits.Two;
                break;
            default:
                break;
        }
    }

    private void Convert(SerialPortGateEnums.Parity parity, out Parity wParity)
    {
        wParity = Parity.None;
        switch (parity)
        {
            case SerialPortGateEnums.Parity.Even:
                wParity= Parity.Even;
                break;
            case SerialPortGateEnums.Parity.Mark:
                wParity= Parity.Mark;
                break;
            case SerialPortGateEnums.Parity.None:
                wParity = Parity.None;
                break;
            case SerialPortGateEnums.Parity.Odd:
                wParity = Parity.Odd;
                break;
            case SerialPortGateEnums.Parity.Space:
                wParity = Parity.Space;
                break;
            default:
                break;
        }
    }

    public override void WriteLine(string text)
    {
        m_serialPort.WriteLine(text);
    }
}
#endif

