﻿#pragma checksum "..\..\..\Views\BeamAnnotationToClear.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4E8298FE46B16246D0552E94612079A3"
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
    /// BeamAnnotationToClear
    /// </summary>
    public partial class BeamAnnotationToClear : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\..\Views\BeamAnnotationToClear.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ClearReactions;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\Views\BeamAnnotationToClear.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ClearReactionsText;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\Views\BeamAnnotationToClear.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ClearStuds;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\Views\BeamAnnotationToClear.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ClearStudCountsText;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\Views\BeamAnnotationToClear.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ClearCamber;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\Views\BeamAnnotationToClear.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ClearCamberText;
        
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
            System.Uri resourceLocater = new System.Uri("/RevitReactionImporterApp;component/views/beamannotationtoclear.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\BeamAnnotationToClear.xaml"
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
            this.ClearReactions = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\..\Views\BeamAnnotationToClear.xaml"
            this.ClearReactions.Click += new System.Windows.RoutedEventHandler(this.OnAnnotationToClearClick);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ClearReactionsText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.ClearStuds = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\..\Views\BeamAnnotationToClear.xaml"
            this.ClearStuds.Click += new System.Windows.RoutedEventHandler(this.OnAnnotationToClearClick);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ClearStudCountsText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.ClearCamber = ((System.Windows.Controls.Button)(target));
            
            #line 70 "..\..\..\Views\BeamAnnotationToClear.xaml"
            this.ClearCamber.Click += new System.Windows.RoutedEventHandler(this.OnAnnotationToClearClick);
            
            #line default
            #line hidden
            return;
            case 6:
            this.ClearCamberText = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

