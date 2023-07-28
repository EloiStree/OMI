using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerialPortGateEnums
{

    public class SerialPortName 
    {
        public string m_serialPort="";
        public SerialPortName()
        {
            m_serialPort = "";
        }
        public SerialPortName(string value)
        {
            m_serialPort = value;
        }
    }

    public enum Parity { Even, Mark, None, Odd, Space}
    public enum StopBits:int { None=0, One=1, OnePointFive=5, Two=2  }
    public enum DataBits: ushort { _5=5,_6=6,_7=7,_8=8 }
    public enum BaudRate : int { _9600=9600, _75=75, _150=150, _300=300, _600=600, _1200=1200, _2400=2400, _4800=4800, _19200=19200, _38400=38400, _57600=57600, _115200= 115200, _230400= 230400 }
    public enum Handshake  { None , RequestToSend, RequestToSendXOnXOff , XOnXOff }

    public static void TryParse(string baudRate, out bool converted, out SerialPortGateEnums.BaudRate value)
    {
        value = BaudRate._9600;
        baudRate = baudRate.Trim();
        converted = true;
        if (baudRate == "9600") value = BaudRate._9600;
        else if (baudRate == "75") value = BaudRate._75;
        else if (baudRate == "150") value = BaudRate._150;
        else if (baudRate == "300") value = BaudRate._300;
        else if (baudRate == "600") value = BaudRate._600;
        else if (baudRate == "1200") value = BaudRate._1200;
        else if (baudRate == "2400") value = BaudRate._2400;
        else if (baudRate == "4800") value = BaudRate._4800;
        else if (baudRate == "19200") value = BaudRate._19200;
        else if (baudRate == "38400") value = BaudRate._38400;
        else if (baudRate == "57600") value = BaudRate._57600;
        else if (baudRate == "115200") value = BaudRate._115200;
        else if (baudRate == "230400") value = BaudRate._230400;
        else converted = false;
    }

    public static void TryParse(string text, out bool converted, out SerialPortGateEnums.Parity parity)
    {
        converted = false;
        parity = Parity.None;
        text = text.Trim().ToLower();
        if (text == "even") { parity = Parity.Even; converted = true; }
        else if (text == "mark") { parity = Parity.Mark; converted = true; }
        else if (text == "none") { parity = Parity.None; converted = true; }
        else if (text == "odd") { parity = Parity.Odd; converted = true; }
        else if (text == "space") { parity = Parity.Space; converted = true; }
    }

    public static void TryParse(string text, out bool converted, out SerialPortGateEnums.StopBits stopbits)
    {
        converted = false;
        stopbits = StopBits.None;
        text = text.Trim().ToLower();
        if (text == "none") { stopbits = StopBits.None; converted = true; }
        else if (text == "one") { stopbits = StopBits.One; converted = true; }
        else if (text == "onepointfive"){ stopbits = StopBits.OnePointFive; converted = true; }
        else if (text == "two"){ stopbits = StopBits.Two; converted = true; }
    }

    public static StopBits ParseOrDefault(string stopBits, SerialPortGateEnums.StopBits defaultIfError)
    {
        try
        {
            TryParse(stopBits, out bool convert, out StopBits value);
            if (convert) return value;
            else return defaultIfError;
        }
        catch (Exception) { 
            return defaultIfError;
        }
        
    }
    public static DataBits ParseOrDefault(string dataBits, SerialPortGateEnums.DataBits defaultIfError)
    {
        try
        {
            TryParse(dataBits, out bool convert, out DataBits value);
            if (convert) return value;
            else return defaultIfError;
        }
        catch (Exception)
        {
            return defaultIfError;
        }
    }

    public static void TryParse(string databits, out bool convert, out SerialPortGateEnums.DataBits value)
    {
        value = DataBits._8;
        databits = databits.Trim();
        convert = true;
        if (databits == "5") value = DataBits._5;
        else if (databits == "6") value = DataBits._6;
        else if (databits == "7") value = DataBits._7;
        else if (databits == "8") value = DataBits._8;
        else convert = false;
    }

    public static SerialPortGateEnums.Parity ParseOrDefault(string parity, SerialPortGateEnums.Parity defaultIfError)
    {
        try
        {
            TryParse(parity, out bool convert, out SerialPortGateEnums.Parity value);
            if (convert) return value;
            else return defaultIfError;
        }
        catch (Exception)
        {
            return defaultIfError;
        }
    }

    public static BaudRate ParseOrDefault(string baudRate, SerialPortGateEnums.BaudRate defaultIfError)
    {
        try
        {
            TryParse(baudRate, out bool convert, out SerialPortGateEnums.BaudRate value);
            if (convert) return value;
            else return defaultIfError;
        }
        catch (Exception)
        {
            return defaultIfError;
        }
    }

    

    public static SerialPortName CreatePortName(string portName)
    {
        return new SerialPortName(portName);
    }

    public static void TryParse(string text, out bool converted, out SerialPortGateEnums.Handshake handshake)
    {
        converted = false;
        handshake = Handshake.None;
        text = text.Trim().ToLower();
        if (text == "requesttosend") { handshake = Handshake.RequestToSend; converted = true; }
        else if (text == "requesttosendxonxoff") { handshake = Handshake.RequestToSendXOnXOff; converted = true; }
        else if (text == "none") { handshake = Handshake.None; converted = true; }
        else if (text == "xonxoff") { handshake = Handshake.XOnXOff; converted = true; }
    }

}
