﻿#pragma checksum "..\..\..\..\Applications\Converter\Converter.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5840698A62BCD0F726CF953A32CA1BEE"
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


namespace ControlCenter.Applications.Converter {
    
    
    /// <summary>
    /// Converter
    /// </summary>
    public partial class Converter : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 6 "..\..\..\..\Applications\Converter\Converter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbFrom;
        
        #line default
        #line hidden
        
        
        #line 7 "..\..\..\..\Applications\Converter\Converter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbTo;
        
        #line default
        #line hidden
        
        
        #line 8 "..\..\..\..\Applications\Converter\Converter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtInput;
        
        #line default
        #line hidden
        
        
        #line 9 "..\..\..\..\Applications\Converter\Converter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtOutput;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\..\..\Applications\Converter\Converter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnConvert;
        
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
            System.Uri resourceLocater = new System.Uri("/ControlCenter;component/applications/converter/converter.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Applications\Converter\Converter.xaml"
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
            this.cbFrom = ((System.Windows.Controls.ComboBox)(target));
            
            #line 6 "..\..\..\..\Applications\Converter\Converter.xaml"
            this.cbFrom.Loaded += new System.Windows.RoutedEventHandler(this.cbFrom_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.cbTo = ((System.Windows.Controls.ComboBox)(target));
            
            #line 7 "..\..\..\..\Applications\Converter\Converter.xaml"
            this.cbTo.Loaded += new System.Windows.RoutedEventHandler(this.cbTo_Loaded);
            
            #line default
            #line hidden
            return;
            case 3:
            this.txtInput = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.txtOutput = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.btnConvert = ((System.Windows.Controls.Button)(target));
            
            #line 10 "..\..\..\..\Applications\Converter\Converter.xaml"
            this.btnConvert.Click += new System.Windows.RoutedEventHandler(this.btnConvert_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

