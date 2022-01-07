// https://json2csharp.com/xml-to-csharp

// using System.Xml.Serialization;
// XmlSerializer serializer = new XmlSerializer(typeof(Omixml));
// using (StringReader reader = new StringReader(xml))
// {
//    var test = (Omixml)serializer.Deserialize(reader);
// }

using System.Collections.Generic;
using System.Xml.Serialization;

[System.Serializable]
[XmlRoot(ElementName = "midiconnectionin")]
public class Midiconnectionin
{

	[XmlAttribute(AttributeName = "midiName")]
	public string MidiName;
}
	[System.Serializable]
	[XmlRoot(ElementName = "midiconnectionout")]
	public class Midiconnectionout
	{

		[XmlAttribute(AttributeName = "midiName")]
		public string MidiName;
	}

	[System.Serializable]
	[XmlRoot(ElementName = "analogdigitalcompressioncom")]
	public class Analogdigitalcompressioncom
	{

		[XmlAttribute(AttributeName = "comIdName")]
		public string ComIdName;

		[XmlAttribute(AttributeName = "nomenclatureName")]
		public string NomenclatureName;
	}

	[System.Serializable]
	[XmlRoot(ElementName = "omixml")]
	public class Omixml
	{

		[XmlElement(ElementName = "midiconnectionin")]
		public List<Midiconnectionin> Midiconnectionin;

		[XmlElement(ElementName = "midiconnectionout")]
		public List<Midiconnectionout> Midiconnectionout;

		[XmlElement(ElementName = "analogdigitalcompressioncom")]
		public Analogdigitalcompressioncom Analogdigitalcompressioncom;
	}

