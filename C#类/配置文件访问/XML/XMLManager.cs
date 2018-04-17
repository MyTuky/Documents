using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Tools
{
    public class XMLManager
    {
        private static XmlDocument xmlDoc = new XmlDocument();
        private static XmlNode root;

        /// <summary>
        /// 加载指定路径下的xml文件
        /// </summary>
        /// <param name="path"></param>
        internal static void Load(string path)
        {
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                xmlDoc.Load(file.FullName);
                root = xmlDoc.DocumentElement;
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(file.FullName))
                {
                    sw.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?><Resource><Version name=\"XmlVersion\" value=\"1.0.0.0\"></Version><Form name=\"MainForm\" value=\"固件新架构一体化维护系统\"></Form></Resource>");
                }
                xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><Resource><Version name=\"XmlVersion\" value=\"1.0.0.0\"></Version><Form name=\"MainForm\" value=\"固件新架构一体化维护系统\"></Form></Resource>");
                root = xmlDoc.DocumentElement;
            }
        }
        /// <summary>
        /// 获取根节点的指定子节点
        /// </summary>
        /// <param name="childName"></param>
        /// <param name="childAtt1Name"></param>
        /// <param name="childAtt2Value"></param>
        /// <returns></returns>
        public static XmlNode GetRootChild(string childName, string childAtt1Name, string childAtt2Value = "")
        {
            return GetChildNode(root, childName, childAtt1Name, childAtt2Value);
        }
        /// <summary>
        /// 获取父节点的指定子节点
        /// </summary>
        /// <param name="node">父节点</param>
        /// <param name="childName">子节点名称</param>
        /// <param name="childAtt1Name">子节点对应的第一个属性值,name的值</param>
        /// <param name="childAtt2Value"></param>
        /// <returns></returns>
        public static XmlNode GetChildNode(XmlNode node, string childName, string childAtt1Name, string childAtt2Value = "")
        {
            if (node != null)
            {
                XmlNodeList lstNode = node.ChildNodes;
                if (lstNode != null && lstNode.Count > 0)
                {
                    for (int i = 0; i < lstNode.Count; i++)
                    {
                        if (lstNode[i].Name == childName)
                        {
                            if (lstNode[i].Attributes.Count == 2 && lstNode[i].Attributes[0].Value == childAtt1Name)
                            {
                                return lstNode[i];
                            }
                        }
                    }
                }
                XmlNode newNode = xmlDoc.CreateNode(XmlNodeType.Element, childName, "");
                XmlAttribute att = xmlDoc.CreateAttribute("name");
                att.Value = childAtt1Name;
                newNode.Attributes.Append(att);

                att = xmlDoc.CreateAttribute("value");
                att.Value = childAtt2Value;
                newNode.Attributes.Append(att);

                node.AppendChild(newNode);
                //Save(System.Windows.Forms.Application.StartupPath + @"\Language-" + "zh_CN" + ".xml");
                return newNode;
            }
            return null;
        }
        /// <summary>
        /// 获取所有子节点
        /// </summary>
        /// <param name="node">父节点</param>
        /// <returns></returns>
        public static XmlNodeList GetChildList(XmlNode node)
        {
            if (node != null)
            {
                return node.ChildNodes;
            }
            return null;
        }
        /// <summary>
        /// 获取窗体对应子节点的特性值
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Hashtable GetNodeAll(object form, XmlNode node)
        {
            XmlNodeList lstNodes = GetChildList(node);
            Hashtable table = new Hashtable();
            if (lstNodes != null && lstNodes.Count > 0)
            {
                for (int i = 0; i < lstNodes.Count; i++)
                {
                    XmlNode child = lstNodes[i];
                    try
                    {
                        if (child.Attributes != null && child.Attributes.Count == 2 && child.Attributes[0].Name.ToLower() == "name")
                        {
                            if (table.ContainsKey(child.Attributes[0].Value))
                            {
                                table[child.Attributes[0].Value] = child.Attributes[1].Value;
                            }
                            else
                            {
                                table.Add(child.Attributes[0].Value, child.Attributes[1].Value);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                System.Windows.Forms.Form frm = form as System.Windows.Forms.Form;
                SetControlsNodes(frm.Controls, node);
                table = GetNodeAll(form, node);
            }
            return table;
        }
        /// <summary>
        /// 设置控件节点
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="node"></param>
        private static void SetControlsNodes(System.Windows.Forms.Control.ControlCollection cc, XmlNode node)
        {
            foreach (System.Windows.Forms.Control item in cc)
            {
                if (item.HasChildren)
                {
                    SetControlsNodes(item.Controls, node);
                }
                SetNodeValue(item.Name, item.Text, "Control", node);
            }
        }
        /// <summary>
        /// 获取对应子节点的特性值
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetNodeAll(XmlNode node)
        {
            XmlNodeList lstNodes = GetChildList(node);
            Dictionary<string, string> table = new Dictionary<string, string>();
            if (lstNodes != null && lstNodes.Count > 0)
            {
                for (int i = 0; i < lstNodes.Count; i++)
                {
                    XmlNode child = lstNodes[i];
                    try
                    {
                        if (child.Attributes != null && child.Attributes.Count == 2 && child.Attributes[0].Name.ToLower() == "name")
                        {
                            if (table.ContainsKey(child.Attributes[0].Value))
                            {
                                table[child.Attributes[0].Value] = child.Attributes[1].Value;
                            }
                            else
                            {
                                table.Add(child.Attributes[0].Value, child.Attributes[1].Value);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
            return table;
        }
        /// <summary>
        /// 设置子节点的值
        /// </summary>
        /// <param name="nodeAtt1Name">子节点name特性的值</param>
        /// <param name="nodeAtt2Value">子节点value特性的值</param>
        /// <param name="nodeName">子节点名称</param>
        /// <param name="node">父节点</param>
        public static void SetNodeValue(string nodeAtt1Name, string nodeAtt2Value, string nodeName, XmlNode node)
        {
            if (node != null)
            {
                XmlNodeList lstChilds = node.ChildNodes;
                bool isExist = false;
                if (lstChilds != null && lstChilds.Count > 0)
                {
                    for (int i = 0; i < lstChilds.Count; i++)
                    {
                        if (lstChilds[i].Name == nodeName && lstChilds[i].Attributes != null && lstChilds[i].Attributes.Count == 2 && lstChilds[i].Attributes[0].Value == nodeAtt1Name)
                        {
                            if (!LanguageSet.langChange && GlobalVariable.isInstance != 2)
                            {//防止修改语言后把本来默认空的控件设为非空  或者  切换模块时把设备单例模块的本来默认空的控件设为非空
                                lstChilds[i].Attributes[1].Value = nodeAtt2Value;
                            }
                            isExist = true;
                            break;
                        }
                    }
                }
                if (!isExist)
                {
                    XmlNode newNode = xmlDoc.CreateNode(XmlNodeType.Element, nodeName, "");
                    XmlAttribute att = xmlDoc.CreateAttribute("name");
                    att.Value = nodeAtt1Name;
                    newNode.Attributes.Append(att);

                    att = xmlDoc.CreateAttribute("value");
                    att.Value = nodeAtt2Value;
                    newNode.Attributes.Append(att);

                    node.AppendChild(newNode);
                }
            }
        }
        public static void Save(string path)
        {
            xmlDoc.Save(path);
        }
    }
}
