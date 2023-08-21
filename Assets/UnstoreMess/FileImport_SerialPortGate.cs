
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static SerialPortGateEnums;

public class FileImport_SerialPortGate : MonoBehaviour
{

    public SerialPortUnityGateMono m_serialPortGate;
    public SerialPortUnityGateListenerMono m_serialPortListener;
    public UniqueIdToAliasRegisterMono m_comToAliasRegister;
    public UniqueIdOrAliasToStringChanelRegisterMono m_comToChannelRedirection;
    public GroupIdToListOfAliasAndComIdRegisterMono m_groupToSerialPort;

    public void ClearRegister()
    {

        Eloi.E_CodeTag.NotSureIfGoodIdea.Info("I am not sure if I should close all port then reopen when loaded or check one.");

        m_serialPortGate.CloseAndReopenPortIfRegistered();
    }



    public void Load(params string[] filePath)
    {
        for (int i = 0; i < filePath.Length; i++)
        {
            if (File.Exists(filePath[i]))
            {
                ImportFileText(File.ReadAllText(filePath[i]));
            }
        }
    }

    private void ImportFileText(string text)
    {
        string[] lines = text.Split('\n');
        TileLine tokens;
        for (int i = 0; i < lines.Length; i++)
        {
            char[] l = lines[i].Trim().ToCharArray();
            if (l.Length > 0 && (l[0] == '#' || (l[0] == '/' && l[1] == '/')))
                continue;

            tokens = new TileLine(lines[i]);


            //☗In♦NanoRead♦DigitalAnalogInput
            if (tokens.GetCount() > 1 && Eloi.E_StringUtility.AreEquals(tokens.GetValue(0), "☗Read", true, true))
            {

                string comAliasOrId = tokens.GetValue(1);
                string redirectionName = tokens.GetValue(2);
                Add(new Intent_ReadOpenReadSerialPortGateToChannel()
                {
                    m_channel = redirectionName,
                    m_comOrAliasId = comAliasOrId
                });
            }//☗Group♦GroupName♦ComOrAlias♦...♦ComOrAlias
            else if (tokens.GetCount() > 1 && Eloi.E_StringUtility.AreEquals(tokens.GetValue(0), "☗Group", true, true))
            {

                string groupId = tokens.GetValue(1);
                List<string> group = new List<string>();
                for (int j = 1; j < tokens.GetCount(); j++)
                {
                    group.Add( tokens.GetValue(j).Trim());
                
                }
                Add(new Intent_CreateGroupOfAliasAndId()
                {
                    m_groupId = groupId,
                    m_comIdAndAlias = group
                });
            }
            //alias♦serialport♦baudrate♦databits♦parity♦stopbits
            //XboxLeo♦COM52♦9600♦8♦None♦One
            //XboxMicro♦COM23♦9600♦8♦None♦One
            //NanoRead♦COM12♦9600♦8♦None♦One
            //NanoRead2 ♦ COM12 ♦ 9600 ♦ 8 ♦ None♦One  ♦ ☗In

            else if(tokens.GetCount() > 1 )
            {
                string alias="";
                string com="";
                int baud=9600;
                int bit=8;
                Parity parity=Parity.None;
                StopBits stop=StopBits.One;
                bool foundChannel = false ;
                string channel = "default";

                alias= tokens.GetValue(0).Trim();
                for (int j = 1; j < tokens.GetCount(); j++)
                {
                    string t = tokens.GetValue(j).Trim();
                    if (int.TryParse(t, out int value))
                    {
                        if (value < 10)
                            bit = value;
                        else bit = value;
                    }
                    else {
                        int inIndex = Eloi.E_StringUtility.IndexOf(t, "☗Read", true,true);
                        if (inIndex > -1) {
                            List<ShogiParameter> p =  ShogiParameter.FindParametersInString(t);
                            ShogiParameter.HasParam(p, "Read", out ShogiParameter inLabel);
                            foundChannel = true;
                            if (inLabel.GetValue().Length != 0) {
                                channel = inLabel.GetValue();
                            }
                        }
                        else if (Eloi.E_StringUtility.StartWith(t, "com", true, true)) { com = t; }
                        else if (Eloi.E_StringUtility.AreEquals(t, "None", true, true)) parity = Parity.None;
                        else if (Eloi.E_StringUtility.AreEquals(t, "Even", true, true)) parity = Parity.Even;
                        else if (Eloi.E_StringUtility.AreEquals(t, "Odd", true, true)) parity = Parity.Odd;
                        else if (Eloi.E_StringUtility.AreEquals(t, "Mark", true, true)) parity = Parity.Mark;
                        else if (Eloi.E_StringUtility.AreEquals(t, "Space", true, true)) parity = Parity.Space;

                        else if (Eloi.E_StringUtility.AreEquals(t, "None", true, true)) stop = StopBits.None;
                        else if (Eloi.E_StringUtility.AreEquals(t, "One", true, true)) stop = StopBits.One;
                        else if (Eloi.E_StringUtility.AreEquals(t, "OnePointFive", true, true)) stop = StopBits.OnePointFive;
                        else if (Eloi.E_StringUtility.AreEquals(t, "Two", true, true)) stop = StopBits.Two;


                    }

                }
                if (com.Trim().Length > 0) { 

                        if (foundChannel)
                        {
                            Add(new Intent_ReadOpenReadSerialPortGateToChannel() { m_comOrAliasId = alias, m_channel = channel });
                            Add(new Intent_ReadOpenReadSerialPortGateToChannel() { m_comOrAliasId = com, m_channel = channel });
                        }

                        Add(new Intent_CreateWriteSerialPortGate() { 
                            m_comPortId = com,
                            m_baudrate = baud,
                            m_bits = bit,
                            m_parity = parity,
                            m_stopBits = stop

                        });

                        Add(new Intent_CreateAliasOfSerialPort(com, alias) );
                }
            }

           

        }

    }

    private void Add(Intent_CreateGroupOfAliasAndId intent)
    {
        Eloi.E_CodeTag.DirtyCode.Info("I am sleepy, it should be event with and aditionnal class to make the bridge");
        m_groupIdToTarget.Add(intent);
        m_groupToSerialPort.AddToList(intent.m_groupId, intent.m_comIdAndAlias);
    }

    public List<Intent_CreateWriteSerialPortGate> m_comConnection= new List<Intent_CreateWriteSerialPortGate>();
    public List<Intent_CreateAliasOfSerialPort> m_aliasToCom = new List<Intent_CreateAliasOfSerialPort>();
    public List<Intent_ReadOpenReadSerialPortGateToChannel> m_readCom = new List<Intent_ReadOpenReadSerialPortGateToChannel>();
    public List<Intent_CreateGroupOfAliasAndId> m_groupIdToTarget = new List<Intent_CreateGroupOfAliasAndId>();

    private void Add(Intent_CreateAliasOfSerialPort intent)
    {
        Eloi.E_CodeTag.DirtyCode.Info("I am sleepy, it should be event with and aditionnal class to make the bridge");
        m_aliasToCom.Add(intent);
        m_comToAliasRegister.AddToAlias(intent.m_comPortId, intent.m_aliasOfThePort );
    }
    private void Add(Intent_CreateWriteSerialPortGate intent)
    {
        m_comConnection.Add(intent);
        Eloi.E_CodeTag.DirtyCode.Info("I am sleepy, it should be event with and aditionnal class to make the bridge");
        SerialPortUnityGateStatic.GetInstance().OpenSerialPort(intent.m_comPortId, intent.m_baudrate, intent.m_bits, intent.m_parity, intent.m_stopBits);
    }
    private void Add(Intent_ReadOpenReadSerialPortGateToChannel intent)
    {
        Eloi.E_CodeTag.DirtyCode.Info("I am sleepy, it should be event with and aditionnal class to make the bridge");
        m_readCom.Add(intent);
        m_comToChannelRedirection.AddToList(intent.m_comOrAliasId, intent.m_channel);
    }
}


[System.Serializable]
public class Intent_CreateAliasOfSerialPort {

    public string m_comPortId="";
    public List<string> m_aliasOfThePort = new List<string>();

    public Intent_CreateAliasOfSerialPort(string com, params string[] alias)
    {
        m_comPortId = com;
        m_aliasOfThePort.AddRange(alias);
    }
    public Intent_CreateAliasOfSerialPort(string com, IEnumerable<string> alias)
    {
        m_comPortId = com;
        m_aliasOfThePort.AddRange(alias);
    }
}


[System.Serializable]
public class Intent_CreateWriteSerialPortGate
{
    public string m_comPortId="";
    public int m_baudrate = 9600;
    public int m_bits = 8;
    public Parity m_parity = Parity.None;
    public StopBits m_stopBits = StopBits.One;
}

[System.Serializable]
public class Intent_ReadOpenReadSerialPortGateToChannel
{
    public string m_comOrAliasId = "";
    public string m_channel = "default";
}
[System.Serializable]
public class Intent_CreateGroupOfAliasAndId
{
    public string m_groupId= "";
    public List<string> m_comIdAndAlias = new List<string>();
}
