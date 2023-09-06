﻿namespace WpfForM_CRM.Properties;

[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.7.0.0")]
internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
    private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
    public static Settings Default {
        get
        {
            return defaultInstance;
        }
    }
        
    [global::System.Configuration.UserScopedSettingAttribute()]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Configuration.DefaultSettingValueAttribute("")]

    public string Name {
        get {
            return ((string)(this["Name"]));
        }
        set {
            this["Name"] = value;
        }
    }
        
    [global::System.Configuration.UserScopedSettingAttribute()]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Configuration.DefaultSettingValueAttribute("")]
    public string Password {
        get {
            return ((string)(this["Password"]));
        }
        set {
            this["Password"] = value;
        }
    }
        
    [global::System.Configuration.UserScopedSettingAttribute()]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Configuration.DefaultSettingValueAttribute("")]
    public string Owner {
        get {
            return ((string)(this["Owner"]));
        }
        set {
            this["Owner"] = value;
        }
    }
        
    [global::System.Configuration.UserScopedSettingAttribute()]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Configuration.DefaultSettingValueAttribute("False")]
    public bool RememberMe {
        get {
            return ((bool)(this["RememberMe"]));
        }
        set {
            this["RememberMe"] = value;
        }
    }
}