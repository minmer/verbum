﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VerbumEssentials.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("VerbumEssentials.Resources.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Aufbauen einer Verbindung mit dem Server.
        /// </summary>
        internal static string ErrorBuildServerConnection {
            get {
                return ResourceManager.GetString("ErrorBuildServerConnection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Klonen einer Verbindung mit dem Server.
        /// </summary>
        internal static string ErrorCloneServerConnection {
            get {
                return ResourceManager.GetString("ErrorCloneServerConnection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Einsetzen der Textinformation.
        /// </summary>
        internal static string ErrorInsertContentText {
            get {
                return ResourceManager.GetString("ErrorInsertContentText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Speichern der Textinformationsinformation.
        /// </summary>
        internal static string ErrorSaveContentTextContent {
            get {
                return ResourceManager.GetString("ErrorSaveContentTextContent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Speichern der Textinformationsfrage.
        /// </summary>
        internal static string ErrorSaveContentTextQuestion {
            get {
                return ResourceManager.GetString("ErrorSaveContentTextQuestion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hochladen von Anfragen.
        /// </summary>
        internal static string ErrorUploadQuery {
            get {
                return ResourceManager.GetString("ErrorUploadQuery", resourceCulture);
            }
        }
    }
}
