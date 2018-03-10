using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Staff_Tab.Services.DataVirtualization
{
    public abstract class VirtualListItemBase
    {
        public static readonly DependencyProperty AutoLoadProperty = DependencyProperty.RegisterAttached(
            "AutoLoad", 
            typeof(bool), 
            typeof(VirtualListItemBase),
            new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnAutoLoadChanged)));

        static void OnAutoLoadChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(ContentControl.ContentProperty, typeof(DependencyObject));
            if (dpd == null)
                return;

            bool isEnabled = (bool)e.NewValue;
            if (isEnabled)
                dpd.AddValueChanged(d, OnContentChanged);
            else
                dpd.RemoveValueChanged(d, OnContentChanged);
        }

        static void OnContentChanged(object sender, EventArgs e)
        {
            VirtualListItemBase item = ((DependencyObject)sender).GetValue(ContentControl.ContentProperty) as VirtualListItemBase;
            if (item != null)
                item.LoadAsync();
        }

        public static bool GetAutoLoad(DependencyObject d)
        {
            return (bool)d.GetValue(AutoLoadProperty);
        }

        public static void SetAutoLoad(DependencyObject d, bool value)
        {
            d.SetValue(AutoLoadProperty, value);
        }

        public abstract bool IsLoaded { get; }

        public object Data
        {
            get { return GetData(); }
        }

        internal abstract object GetData();

        public abstract void Load();

        public abstract void LoadAsync();

    }
}
