﻿#pragma checksum "..\..\VkWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "D3E76D9136A92305EDE1F913FD29BD619BB13EF7"
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
using VkApp;


namespace VkApp {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\VkWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbMessage;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\VkWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbtnAdd;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\VkWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbtnSend;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\VkWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbGames;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\VkWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnStart;
        
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
            System.Uri resourceLocater = new System.Uri("/VkApp;component/vkwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\VkWindow.xaml"
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
            
            #line 11 "..\..\VkWindow.xaml"
            ((VkApp.MainWindow)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.tbMessage = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.rbtnAdd = ((System.Windows.Controls.RadioButton)(target));
            
            #line 27 "..\..\VkWindow.xaml"
            this.rbtnAdd.Checked += new System.Windows.RoutedEventHandler(this.rbtnAdd_Checked);
            
            #line default
            #line hidden
            return;
            case 4:
            this.rbtnSend = ((System.Windows.Controls.RadioButton)(target));
            
            #line 28 "..\..\VkWindow.xaml"
            this.rbtnSend.Checked += new System.Windows.RoutedEventHandler(this.rbtnSend_Checked);
            
            #line default
            #line hidden
            return;
            case 5:
            this.cbGames = ((System.Windows.Controls.ComboBox)(target));
            
            #line 42 "..\..\VkWindow.xaml"
            this.cbGames.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cbGames_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnStart = ((System.Windows.Controls.Button)(target));
            
            #line 49 "..\..\VkWindow.xaml"
            this.btnStart.Click += new System.Windows.RoutedEventHandler(this.btnStart_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

