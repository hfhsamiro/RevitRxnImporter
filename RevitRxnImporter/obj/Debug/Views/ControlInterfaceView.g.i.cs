﻿#pragma checksum "..\..\..\Views\ControlInterfaceView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E8E3674E5CCBBA67093B3F90ADB33BBD"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace RevitReactionImporter {
    
    
    /// <summary>
    /// ControlInterfaceView
    /// </summary>
    public partial class ControlInterfaceView : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\..\Views\ControlInterfaceView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ImportRAMReactions;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\Views\ControlInterfaceView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ResetBeamReactions;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\Views\ControlInterfaceView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ImportReactionsText;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\Views\ControlInterfaceView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ResetBeamReactionsText;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/RevitReactionImporterApp;component/views/controlinterfaceview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\ControlInterfaceView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ImportRAMReactions = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\..\Views\ControlInterfaceView.xaml"
            this.ImportRAMReactions.Click += new System.Windows.RoutedEventHandler(this.OnImportBeamReactionsClick);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ResetBeamReactions = ((System.Windows.Controls.Button)(target));
            
            #line 39 "..\..\..\Views\ControlInterfaceView.xaml"
            this.ResetBeamReactions.Click += new System.Windows.RoutedEventHandler(this.OnResetBeamReactionsClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ImportReactionsText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.ResetBeamReactionsText = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

