using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BuptAssistant.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryCell : ViewCell
    {
        public static readonly BindableProperty PlaceHolderProperty =
            BindableProperty.Create("PlaceHolder", typeof(string), typeof(EntryCell), null, BindingMode.TwoWay);
        public static readonly BindableProperty LabelProperty =
           BindableProperty.Create("Label", typeof(string), typeof(EntryCell), null, BindingMode.TwoWay);
        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create("Value", typeof(string), typeof(EntryCell), null, BindingMode.TwoWay);
        public static readonly BindableProperty KeyProperty =
            BindableProperty.Create("Key", typeof(string), typeof(EntryCell), null, BindingMode.TwoWay);
        public static readonly BindableProperty IsPasswordProperty =
            BindableProperty.Create("IsPassword", typeof(bool), typeof(EntryCell), false, BindingMode.TwoWay);

        public static readonly BindableProperty KeyboardProperty =
            BindableProperty.Create("KeyboardType", typeof(Keyboard), typeof(EntryCell), null, BindingMode.TwoWay);

        public string PlaceHolder
        {
            get { return (string)GetValue(PlaceHolderProperty); }
            set { SetValue(PlaceHolderProperty, value); }
        }
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public string Key
        {
            get { return (string)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }
        public bool IsPassword
        {
            get { return (bool)GetValue(IsPasswordProperty); }
            set { SetValue(IsPasswordProperty, value); }
        }

        public Keyboard KeyboardType
        {
            get { return (Keyboard) GetValue(KeyboardProperty); }
            set { SetValue(KeyboardProperty,value);}

        }

        public EntryCell()
        {
            InitializeComponent();

            label.BindingContext = this;
            password.BindingContext = this;

        }

    }
}
