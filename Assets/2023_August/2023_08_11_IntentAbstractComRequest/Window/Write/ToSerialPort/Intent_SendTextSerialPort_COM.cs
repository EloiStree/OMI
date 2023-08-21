

public class Intent_SendTextSerialPort_COM 
{
    public string m_serialPortAliasCOM = "COM44";
    public string m_textToSend = "";
    public int m_baudRate=9600;
    public ParityEnum m_parity;
    public StopBitEnum m_stopBits;
    public DataBitsEnum m_dataBits;

    public enum ParityEnum { None, Even, Mark, Odd }
    public enum StopBitEnum { _1, _1_5, _2 }
    public enum DataBitsEnum { _8,_5, _6, _7,  }
}

