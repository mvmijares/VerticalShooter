﻿using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Schema;
using System.Xml;

namespace BulletMLLib
{
  /// <summary>
  /// This is a complete document that describes a bullet pattern.
  /// </summary>
  public class BulletPattern
  {
    #region Members

    /// <summary>
    /// The root node of a tree structure that describes the bullet pattern
    /// </summary>
    public BulletMLNode RootNode { get; private set; }

    //TODO: move filename class to github and use it here

    /// <summary>
    /// Gets the filename.
    /// This property is only set by calling the parse method
    /// </summary>
    /// <value>The filename.</value>
    public string Filename { get; private set; }

    /// <summary>
    /// the orientation of this bullet pattern: horizontal or veritcal
    /// this is read in from the xml
    /// </summary>
    /// <value>The orientation.</value>
    public EPatternType Orientation { get; private set; }

    #endregion //Members

    #region Methods

    /// <summary>
    /// Initializes a new instance of the <see cref="BulletMLLib.BulletPattern"/> class.
    /// </summary>
    public BulletPattern()
    {
      RootNode = null;
    }

    /// <summary>
    /// convert a string to a pattern type enum
    /// </summary>
    /// <returns>The type to name.</returns>
    /// <param name="str">String.</param>
    private static EPatternType StringToPatternType(string str)
    {
      return (EPatternType)Enum.Parse(typeof(EPatternType), str);
    }

    /// <summary>
    /// Parses a bulletml document into this bullet pattern
    /// </summary>
    /// <param name="xmlFileName">Xml file name.</param>
    public void ParseXML(string xmlFileName)
    {
      using (XmlReader reader = XmlReader.Create(xmlFileName, new XmlReaderSettings()))
      {
        ParseXML(xmlFileName, reader);
      }

    }

    /// <summary>
    /// Parses a bulletml document into this bullet pattern
    /// </summary>
    /// <param name="xmlFileName">Xml file name.</param>
    public void ParseXML(string xmlFileName, XmlReader reader)
    {
      try
      {
        //Open the file.
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(reader);
        XmlNode rootXmlNode = xmlDoc.DocumentElement;

        //make sure it is actually an xml node
        if (rootXmlNode.NodeType == XmlNodeType.Element)
        {
          //eat up the name of that xml node
          string strElementName = rootXmlNode.Name;
          if ("bulletml" != strElementName)
          {
            //The first node HAS to be bulletml
            throw new Exception("Error reading \"" + xmlFileName + "\": XML root node needs to be \"bulletml\", found \"" + strElementName + "\" instead");
          }

          //Create the root node of the bulletml tree
          RootNode = new BulletMLNode(ENodeName.bulletml);

          //Read in the whole bulletml tree
          RootNode.Parse(rootXmlNode, null);

          //Find what kind of pattern this is: horizontal or vertical
          XmlNamedNodeMap mapAttributes = rootXmlNode.Attributes;
          for (int i = 0; i < mapAttributes.Count; i++)
          {
            //will only have the name attribute
            string strName = mapAttributes.Item(i).Name;
            string strValue = mapAttributes.Item(i).Value;
            if ("type" == strName)
            {
              //if  this is a top level node, "type" will be veritcal or horizontal
              Orientation = StringToPatternType(strValue);
            }
          }
        }
      }
      catch (Exception ex)
      {
        //an error ocurred reading in the tree
        throw new Exception("Error reading \"" + xmlFileName + "\"", ex);
      }

      //grab that filename 
      Filename = xmlFileName;

      //validate that the bullet nodes are all valid
      try
      {
        RootNode.ValidateNode();
      }
      catch (Exception ex)
      {
        //an error ocurred reading in the tree
        throw new Exception("Error reading \"" + xmlFileName + "\"", ex);
      }
    }
    #endregion //Methods
  }
}
