﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Subs.Data.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=PKLWEBDB01\\mssql2016std;Initial Catalog=tempdb;Integrated Security=Tr" +
            "ue;Enlist=False;Pooling=True;Max Pool Size=10;Connect Timeout=100")]
        public string MIMSConnectionString {
            get {
                return ((string)(this["MIMSConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=PKLWEBDB01\\mssql2016std;Initial Catalog=MIMS3;Integrated Security=Tru" +
            "e;Enlist=False;Max Pool Size=10;Connect Timeout=100")]
        public string MIMS3ConnectionString {
            get {
                return ((string)(this["MIMS3ConnectionString"]));
            }
        }


        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3." +
           "org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <s" +
           "tring>VANDERMERWER</string>\r\n</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection Authority3
        {
            get
            {
                return ((global::System.Collections.Specialized.StringCollection)(this["Authority3"]));
            }
        }

        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3." +
            "org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <s" +
            "tring>SAMSONS</string>\r\n  <string>NKGAUO</string>\r\n</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection Authority2
        {
            get
            {
                return ((global::System.Collections.Specialized.StringCollection)(this["Authority2"]));
            }
        }

        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3." +
            "org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <s" +
            "tring>REITMANNH</string>\r\n  <string>HEIN</string>\r\n  <string>PKLDAM</string>\r\n</" +
            "ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection Authority4
        {
            get
            {
                return ((global::System.Collections.Specialized.StringCollection)(this["Authority4"]));
            }
        }
       
    }
}
