/*****************************************************************
* Class Name: AppSet
* Purpose   :配置文件管理
* Creater   : 杜琦
* Created Date      ：2017-11-23
* Update Date   ：
*****************************************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class AppSet
    {
        /// <summary>
        /// 获取配置文件中对应名称的配置节appSettings信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSetting(string key)
        {
            try
            {
                //string[] test = ConfigurationManager.AppSettings.AllKeys;
                return ConfigurationManager.AppSettings[key];//.Get(key);
            }
            catch
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 设置配置文件配置节appSettings信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetAppSetting(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //string filePath = config.FilePath;
            config.AppSettings.Settings.Remove(key);
            config.AppSettings.Settings.Add(key, value);
            //config.AppSettings.SectionInformation.ForceSave = true;
            config.Save(ConfigurationSaveMode.Modified);
            //config.SaveAs("app.config");//将新建一个app文件存储，验证可以存储
            ConfigurationManager.RefreshSection("appSettings");
        }
        /// <summary>
        /// 设置配置文件配置节connectionStrings信息
        /// </summary>
        /// <param name="DbType"></param>
        /// <param name="connectionString"></param>
        /// <param name="providerName"></param>
        public static void SetConnectionString(string DbType, string connectionString, string providerName)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.ConnectionStrings.ConnectionStrings.Remove("DbType");
            config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("DbType", DbType));
            config.ConnectionStrings.ConnectionStrings.Remove("connectionString");
            config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("connectionString", connectionString, providerName));
            //config.AppSettings.Settings.Add("DBType", DbType);

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");

        }
        /// <summary>
        /// 获取配置文件中对应名称的配置节connectionStrings信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetConnectionString(string name)
        {
            try
            {
                return ConfigurationManager.ConnectionStrings[name].ConnectionString;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
