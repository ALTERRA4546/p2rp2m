﻿#pragma checksum "..\..\ReceptionBioMaterial.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "74D3DE8144597B556B532F5CA8394D2875D1ACC8DD19446EA7BFB888429B8337"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using MedLaboratory;
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


namespace MedLaboratory {
    
    
    /// <summary>
    /// ReceptionBioMaterial
    /// </summary>
    public partial class ReceptionBioMaterial : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\ReceptionBioMaterial.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox findUser;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\ReceptionBioMaterial.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox selectUser;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\ReceptionBioMaterial.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgridUser;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\ReceptionBioMaterial.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgridUslug;
        
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
            System.Uri resourceLocater = new System.Uri("/MedLaboratory;component/receptionbiomaterial.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ReceptionBioMaterial.xaml"
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
            
            #line 8 "..\..\ReceptionBioMaterial.xaml"
            ((MedLaboratory.ReceptionBioMaterial)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.findUser = ((System.Windows.Controls.TextBox)(target));
            
            #line 24 "..\..\ReceptionBioMaterial.xaml"
            this.findUser.KeyDown += new System.Windows.Input.KeyEventHandler(this.findUser_KeyDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.selectUser = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.dgridUser = ((System.Windows.Controls.DataGrid)(target));
            
            #line 28 "..\..\ReceptionBioMaterial.xaml"
            this.dgridUser.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.dgridUslug_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.dgridUslug = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 6:
            
            #line 32 "..\..\ReceptionBioMaterial.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 33 "..\..\ReceptionBioMaterial.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_1);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 34 "..\..\ReceptionBioMaterial.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_2);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

