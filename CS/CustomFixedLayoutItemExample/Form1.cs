using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraLayout;

namespace CustomFixedLayoutItemExample {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            #region #1
            layoutControl1.RegisterCustomPropertyGridWrapper(typeof(MyFixedLabelItem),
                typeof(MyFixedLabelPropertiesWrapper));
            layoutControl1.RegisterFixedItemType(typeof(MyFixedLabelItem));
            #endregion #1
            layoutControl1.ShowCustomizationForm();
        }
    }

    #region #2
    // The custom 'fixed' item.
    public class MyFixedLabelItem : LayoutControlItem, IFixedLayoutControlItem {
        // Must return the name of the item's type
        public override string TypeName { get { return "MyFixedLabelItem"; } }
        string linkCore;
        Control controlCore = null;

        public string Link {
            get { return linkCore; }
            set {
                if (Link == value) return;
                this.linkCore = value;
                OnLinkChanged();
            }
        }
        void label_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            //...
        }
        public override string Text {
            get { return Link; }
            set { Link = value; }
        }
        // This method is called when the Link property is changed.
        // It assigns the new link to the embedded LinkLabel control.
        protected void OnLinkChanged() {
            controlCore.Text = Link;
        }

        // Initialize the item.
        void IFixedLayoutControlItem.OnInitialize() {
            this.linkCore = "www.devexpress.com";
            OnLinkChanged();
            TextVisible = false;
        }
        // Create and return the item's control.
        Control IFixedLayoutControlItem.OnCreateControl() {
            this.controlCore = new LinkLabel();
            ((LinkLabel)controlCore).LinkClicked += label_LinkClicked;
            return controlCore;
        }
        // Destroy the item's control.
        void IFixedLayoutControlItem.OnDestroy() {
            if (controlCore != null) {
                ((LinkLabel)controlCore).LinkClicked -= label_LinkClicked;
                controlCore.Dispose();
                controlCore = null;
            }
        }
        string IFixedLayoutControlItem.CustomizationName { get { return "DevExpress Link"; } }
        Image IFixedLayoutControlItem.CustomizationImage { get { return null; } }
        bool IFixedLayoutControlItem.AllowChangeTextLocation { get { return false; } }
        bool IFixedLayoutControlItem.AllowChangeTextVisibility { get { return false; } }
        bool IFixedLayoutControlItem.AllowClipText { get { return false; } }
        ILayoutControl IFixedLayoutControlItem.Owner { get { return base.Owner; } set { base.Owner = value; } }
    }

    // Specifies which properties to display in the Property Grid
    public class MyFixedLabelPropertiesWrapper : BasePropertyGridObjectWrapper {
        protected MyFixedLabelItem Label { get { return WrappedObject as MyFixedLabelItem; } }
        [Description("The link's text")]
        public string Link { get { return Label.Link; } set { Label.Link = value; } }
        public override BasePropertyGridObjectWrapper Clone() {
            return new MyFixedLabelPropertiesWrapper();
        }
    }
    #endregion #2
}