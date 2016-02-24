using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ControlCenter
{
    public static class LoadCmds
    {

        public static Dictionary<int, string> load(string nodes)
        {
            Dictionary<int, string> commands = new Dictionary<int, string>();
            XmlDocument doc = new XmlDocument();
            int i = 0;
            
            try
            {
                string path = "Utils/FastCommands.xml";
                doc.Load(path);

                XmlNodeList xml_commands = doc.DocumentElement.SelectNodes(nodes);

                foreach (XmlNode cmd in xml_commands)
                {
                    commands[i] = cmd.InnerText;
                    i++;
                }

            }
            catch (Exception ex)
            {
            }
            return commands;

        }
    }
}
